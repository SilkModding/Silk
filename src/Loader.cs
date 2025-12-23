using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Logger = Silk.Logger;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Silk
{
    public static class Loader
    {
        private static readonly string ModsFolder = Utils.GetModsFolder();

        public static List<SilkModData> LoadedMods { get; } = new List<SilkModData>();
        public static List<SilkModData> FailedMods { get; } = new List<SilkModData>();

        public static void Initialize()
        {
            Logger.LogInfo("Silk Loader Initialized");

            if (!Directory.Exists(ModsFolder))
            {
                Logger.LogInfo("Mods folder does not exist, creating it");
                try
                {
                    Directory.CreateDirectory(ModsFolder);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to create mods folder!" + Environment.NewLine + ex.ToString());
                }
            }

            LoadMods();
        }

        private static void LoadMods()
        {
            var modFiles = DiscoverModFiles();
            Logger.LogInfo($"Found {modFiles.Length} mods to load");

            foreach (var modFile in modFiles)
            {
                try
                {
                    ProcessModFile(modFile);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error loading mod: {Path.GetFileName(modFile)} {ex}");
                }
            }

            LogSummary();

            // Create the mods Gameobject and setup mods
            Mods.Manager.SetupMods();

            if (FailedMods.Any())
            {
                LogFailedModsDetails();
            }
        }

        private static string[] DiscoverModFiles()
        {
            try
            {
                var searchPattern = Config.GetConfigValue<string>("loader.modFilePattern");
                return Directory.GetFiles(ModsFolder, searchPattern, SearchOption.AllDirectories)
                    .Where(file => file.IndexOf("Disabled", StringComparison.OrdinalIgnoreCase) < 0)
                    .ToArray();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to find mods! Check your mods folder." + Environment.NewLine + ex.ToString());
                return Array.Empty<string>();
            }
        }

        private static void ProcessModFile(string modFile)
        {
            try
            {
                Logger.LogInfo($"Loading mod: {Path.GetFileName(modFile)}");
                using var assemblyDefinition = AssemblyDefinition.ReadAssembly(modFile);
                var modTypes = assemblyDefinition.MainModule.Types
                    .Where(type => type.CustomAttributes.Any(attr => attr.AttributeType.FullName == typeof(SilkModAttribute).FullName));

                foreach (var type in modTypes)
                {
                    LoadSingleMod(type, modFile);
                }
            }
            catch (Exception ex)
            {
                var modName = Path.GetFileNameWithoutExtension(modFile);
                Logger.LogError($"Failed to load mod: {modName}");
                Logger.LogError(ex.ToString());
                FailedMods.Add(new SilkModData(modName, Array.Empty<string>(), "Unknown", "Unknown", modName, NetworkingType.None));
            }
        }

        private static void LoadSingleMod(TypeDefinition type, string modFile)
        {
            var silkModAttribute = type.CustomAttributes.First(attr => attr.AttributeType.FullName == typeof(SilkModAttribute).FullName);
            var modData = ExtractModData(silkModAttribute, type.Name);

            LogModInfo(modData);

            // Check version compatibility
            var currentSilkVersion = Main.GetSilkVersion();
            var ignoreVersionMismatch = Config.GetConfigValue<bool>("loader.ignoreVersionMismatch");
            
            if (currentSilkVersion == null)
            {
                Logger.LogWarning($"Unable to determine current Silk version. Cannot verify compatibility for mod '{modData.ModName}'.");
            }
            else if (!VersionsMatch(modData.ModSilkVersion, currentSilkVersion))
            {
                Logger.LogWarning($"Version mismatch: Mod '{modData.ModName}' targets Silk v{modData.ModSilkVersion}, but current version is v{currentSilkVersion}");
                
                if (!ignoreVersionMismatch)
                {
                    Logger.LogWarning($"Skipping mod '{modData.ModName}' due to version mismatch. Set 'loader.ignoreVersionMismatch' to true in config to load anyway.");
                    FailedMods.Add(modData);
                    return;
                }
                else
                {
                    Logger.LogWarning($"Attempting to load mod '{modData.ModName}' despite version mismatch...");
                }
            }

            if (modData.ModNetworkingType == NetworkingType.Server || modData.ModNetworkingType == NetworkingType.Both)
            {
                Utils.onlineMods = true;
            }

            if (InvokeModEntrypoint(type, modFile, modData.ModEntryPoint))
            {
                LoadedMods.Add(modData);
            }
            else
            {
                FailedMods.Add(modData);
            }
        }

        private static SilkModData ExtractModData(CustomAttribute attribute, string typeName)
        {
            var modName = attribute.ConstructorArguments[0].Value?.ToString() ?? "Unnamed";
            var modAuthorsRaw = attribute.ConstructorArguments[1].Value as CustomAttributeArgument[];
            var authors = modAuthorsRaw?.Select(a => a.Value?.ToString() ?? "Unknown").ToArray() ?? Array.Empty<string>();
            var modVersion = attribute.ConstructorArguments[2].Value?.ToString() ?? "Unknown";
            var modSilkVersion = attribute.ConstructorArguments[3].Value?.ToString() ?? "Unknown";
            var modId = attribute.ConstructorArguments[4].Value?.ToString() ?? modName;
            var modEntryPoint = attribute.Properties.FirstOrDefault(p => p.Name == "EntryPoint").Argument.Value?.ToString() ?? "Initialize";
            var modNetworkingType = (NetworkingType)(attribute.ConstructorArguments[5].Value ?? NetworkingType.None);

            return new SilkModData(modName, authors, modVersion, modSilkVersion, modId, modNetworkingType) { ModEntryPoint = modEntryPoint };
        }

        private static bool InvokeModEntrypoint(TypeDefinition type, string modFile, string entryPointName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(modFile);
                var modClass = assembly.GetType(type.FullName);

                if (modClass == null)
                {
                    Logger.LogError($"Could not load mod class: {type.FullName}");
                    return false;
                }

                var entryPoint = modClass.GetMethod(entryPointName, BindingFlags.Public | BindingFlags.Instance);
                if (entryPoint == null)
                {
                    Logger.LogError($"Entry point '{entryPointName}' not found in {modClass.FullName}");
                    return false;
                }

                var modInstance = Activator.CreateInstance(modClass);
                
                // Register with manager
                if (modInstance is SilkMod silkMod)
                {
                    Mods.Manager.AddMod(silkMod);
                }
                else
                {
                    Logger.LogWarning($"Mod {modClass.FullName} is not a SilkMod instance. Skipping registration.");
                }

                entryPoint.Invoke(modInstance, null);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception invoking entry point for {type.FullName}: {ex}");
                return false;
            }
        }

        private static void LogModInfo(SilkModData modData)
        {
            var authors = string.Join(", ", modData.ModAuthors);
            Logger.LogInfo($"━━━ {modData.ModName} v{modData.ModVersion} by {authors} ━━━");
            Logger.LogInfo($"Silk Version: {modData.ModSilkVersion}");
            Logger.LogInfo($"ID: {modData.ModId}");
            
            // Only log networking type if it's not None
            if (modData.ModNetworkingType != NetworkingType.None)
            {
                Logger.LogInfo($"Networking: {modData.ModNetworkingType}");
            }
        }

        /// <summary>
        /// Compares two version strings, ignoring build metadata (everything after '+').
        /// </summary>
        private static bool VersionsMatch(string version1, string version2)
        {
            // Strip build metadata (everything after '+') from both versions
            var v1 = version1?.Split('+')[0] ?? string.Empty;
            var v2 = version2?.Split('+')[0] ?? string.Empty;
            
            return v1.Equals(v2, StringComparison.OrdinalIgnoreCase);
        }

        private static void LogSummary()
        {
            Logger.LogInfo("Finished loading mods");
            Logger.LogInfo($"Mods loaded: {LoadedMods.Count}");
            Logger.LogInfo($"Mods failed to load: {FailedMods.Count}");
        }

        private static void LogFailedModsDetails()
        {
            Logger.LogInfo("Mods that failed to load:");
            foreach (var failedMod in FailedMods)
            {
                Logger.LogInfo($"  {failedMod.ModName} by {string.Join(", ", failedMod.ModAuthors)}");
                Utils.Announce($"Failed to load mod: {failedMod.ModName}", 255, 0, 0);
            }
        }

        /// <summary>
        /// Loads BepInEx.Preloader.dll, which is used to initialize BepInEx.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method sets the <c>DOORSTOP_INVOKE_DLL_PATH</c> environment variable to the path of
        /// <c>BepInEx.Preloader.dll</c> before calling the entry point method of the assembly. This is
        /// necessary because the preloader assembly looks for other BepInEx assemblies in the same directory
        /// as the value of <c>DOORSTOP_INVOKE_DLL_PATH</c>.
        /// </para>
        /// <para>
        /// If an exception occurs while loading the preloader assembly or calling the entry point method,
        /// the exception is logged to the console and the method returns without doing anything else.
        /// </para>
        /// <para>
        /// After loading the preloader assembly and calling the entry point method, the
        /// <c>DOORSTOP_INVOKE_DLL_PATH</c> environment variable is reset to its original value.
        /// </para>
        /// </remarks>
        public static void LoadBepInEx()
        {
            // Load BepInEx
            Logger.LogInfo("Loading BepInEx.Preloader.dll...");
            string path = Path.Combine("BepInEx", "core", "BepInEx.Preloader.dll");

            if (!File.Exists(path))
            {
                Logger.LogError($"BepInEx.Preloader.dll not found at {path}");
                return;
            }

            string actualInvokePath = Environment.GetEnvironmentVariable("DOORSTOP_INVOKE_DLL_PATH");
            Environment.SetEnvironmentVariable("DOORSTOP_INVOKE_DLL_PATH", Path.Combine(Directory.GetCurrentDirectory(), path));

            try
            {
                Assembly bepInExPreloader = Assembly.LoadFrom(path);
                var entryPointMethod = bepInExPreloader.GetType("Doorstop.Entrypoint", true)
                    ?.GetMethod("Start");

                entryPointMethod?.Invoke(null, null);
                Logger.LogInfo("Successfully loaded BepInEx.Preloader.dll");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception while loading BepInEx.Preloader.dll: {ex.Message}");
                Logger.LogError(ex.StackTrace);
            }
            finally
            {
                Environment.SetEnvironmentVariable("DOORSTOP_INVOKE_DLL_PATH", actualInvokePath);
            }
        }
    }
}
