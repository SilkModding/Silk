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
                Directory.CreateDirectory(ModsFolder);
            }

            LoadMods();
        }

        private static void LoadMods()
        {
            var modFiles = Directory.GetFiles(ModsFolder, "*.dll", SearchOption.AllDirectories)
                .Where(file => !file.Contains("Disabled") && !file.Contains("disabled"))
                .ToArray();

            var modsToLoad = modFiles.Length;
            var modsLoaded = 0;
            var modsFailed = 0;

            Logger.LogInfo($"Found {modsToLoad} mods to load");

            foreach (var modFile in modFiles)
            {
                try
                {
                    Logger.LogInfo($"Loading mod: {Path.GetFileName(modFile)}");
                    using (var assemblyDefinition = AssemblyDefinition.ReadAssembly(modFile))
                    {
                        var modTypes = assemblyDefinition.MainModule.Types
                            .Where(type => type.CustomAttributes.Any(attr => attr.AttributeType.FullName == typeof(SilkModAttribute).FullName));

                        foreach (var type in modTypes)
                        {
                            var silkModAttribute = type.CustomAttributes.First(attr => attr.AttributeType.FullName == typeof(SilkModAttribute).FullName);

                            var modName = silkModAttribute.ConstructorArguments[0].Value?.ToString() ?? "Unnamed";
                            var modAuthors = silkModAttribute.ConstructorArguments[1].Value as CustomAttributeArgument[];
                            var authors = modAuthors?.Select(a => a.Value?.ToString() ?? "Unknown").ToArray() ?? Array.Empty<string>();
                            var modVersion = silkModAttribute.ConstructorArguments[2].Value?.ToString() ?? "Unknown";
                            var modSilkVersion = silkModAttribute.ConstructorArguments[3].Value?.ToString() ?? "Unknown";
                            var modId = silkModAttribute.ConstructorArguments[4].Value?.ToString() ?? modName;
                            var modEntryPoint = silkModAttribute.Properties.Count > 5
                                ? silkModAttribute.Properties[5].Argument.Value?.ToString() ?? "Initialize"
                                : "Initialize";
                            var modNetworkingType = (NetworkingType)silkModAttribute.ConstructorArguments[5].Value;

                            // Print mod info
                            Logger.LogInfo($"Found Mod: {modName} by {string.Join(", ", authors)}");
                            Logger.LogInfo($"Mod Version: {modVersion}");
                            Logger.LogInfo($"Mod Silk Version: {modSilkVersion}");
                            Logger.LogInfo($"Mod Id: {modId}");
                            Logger.LogInfo($"Mod EntryPoint: {modEntryPoint}");
                            Logger.LogInfo($"Mod NetworkingType: {modNetworkingType}");

                            if (modNetworkingType == NetworkingType.Server || modNetworkingType == NetworkingType.Both)
                            {
                                Utils.onlineMods = true;
                            }

                            // Load the actual mod
                            var assembly = Assembly.LoadFrom(modFile);
                            var modClass = assembly.GetType(type.FullName);

                            if (modClass == null)
                            {
                                Logger.LogError($"Failed to load type: {type.FullName}");
                                FailedMods.Add(new SilkModData(modName, authors, modVersion, modSilkVersion, modId, modNetworkingType));
                                modsFailed++;
                                continue;
                            }

                            if (typeof(SilkMod).IsAssignableFrom(modClass))
                            {
                                var modInstance = Activator.CreateInstance(modClass) as SilkMod;
                                modInstance?.Initialize();

                                // Add the mod
                                Mods.Manager.AddMod(modInstance); // This is fine that it can be null, dont belive the compiler
                                Logger.LogInfo($"Added mod: {modName}");

                                Logger.LogInfo($"Initialized mod: {modName}");
                            }
                            else
                            {
                                var entryPointMethod = modClass.GetMethod(modEntryPoint, BindingFlags.Public | BindingFlags.Static);
                                if (entryPointMethod != null)
                                {
                                    entryPointMethod.Invoke(null, null);
                                    Logger.LogInfo($"Executed entry point: {modEntryPoint} for mod: {modName}");
                                }
                                else
                                {
                                    Logger.LogError($"Entry point method {modEntryPoint} not found in {modClass.FullName}");
                                    FailedMods.Add(new SilkModData(modName, authors, modVersion, modSilkVersion, modId, modNetworkingType));
                                    modsFailed++;
                                    continue;
                                }
                            }

                            LoadedMods.Add(new SilkModData(modName, authors, modVersion, modSilkVersion, modId, modNetworkingType));
                            modsLoaded++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to load mod {Path.GetFileName(modFile)}: {ex.Message}");
                    Logger.LogError(ex.StackTrace);
                    var modName = Path.GetFileNameWithoutExtension(modFile);
                    FailedMods.Add(new SilkModData(modName, Array.Empty<string>(), "Unknown", "Unknown", modName, NetworkingType.None));
                    modsFailed++;
                }
            }

            Logger.LogInfo($"Finished loading mods");
            Logger.LogInfo($"Mods loaded: {modsLoaded}");
            Logger.LogInfo($"Mods failed to load: {modsFailed}");

            Logger.LogInfo("Setting up mods...");
            Mods.Manager.SetupMods();

            if (modsFailed > 0)
            {
                Logger.LogInfo("Mods failed to load:");
                foreach (var failedMod in FailedMods)
                {
                    Logger.LogInfo($"  {failedMod.ModName} by {string.Join(", ", failedMod.ModAuthors)}");
                    Utils.Announce($"Failed to load mod: {failedMod.ModName}", 255, 0, 0);
                }
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
                var entryPointMethod = bepInExPreloader.GetType("BepInEx.Preloader.Entrypoint", true)
                    ?.GetMethod("Main");

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

    public class SilkModData
    {
        public string ModName { get; }
        public string[] ModAuthors { get; }
        public string ModVersion { get; }
        public string ModSilkVersion { get; }
        public string ModId { get; }
        public NetworkingType ModNetworkingType { get; }

        public SilkModData(string modName, string[] modAuthors, string modVersion, string modSilkVersion, string modId, NetworkingType modNetworkingType)
        {
            ModName = modName;
            ModAuthors = modAuthors;
            ModVersion = modVersion;
            ModSilkVersion = modSilkVersion;
            ModId = modId;
            ModNetworkingType = modNetworkingType;
        }
    }
}
