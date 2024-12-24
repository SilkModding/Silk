using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Logger = Silk.Logger;

namespace Silk
{
    
public static class Loader
    {
        private static readonly string ModsFolder = Utils.GetModsFolder();

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
            var modFiles = Directory.GetFiles(ModsFolder, "*.dll");

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

                            var modName = silkModAttribute.ConstructorArguments[0].Value.ToString();
                            var modAuthor = silkModAttribute.ConstructorArguments[1].Value.ToString();
                            var modEntryPoint = silkModAttribute.ConstructorArguments.Count > 2
                                ? silkModAttribute.ConstructorArguments[2].Value.ToString()
                                : "Initialize";

                            Logger.LogInfo($"Found Mod: {modName} by {modAuthor}");

                            var assembly = Assembly.LoadFrom(modFile);
                            var modClass = assembly.GetType(type.FullName);

                            if (modClass == null)
                            {
                                Logger.LogError($"Failed to load type: {type.FullName}");
                                modsFailed++;
                                continue;
                            }

                            if (typeof(SilkMod).IsAssignableFrom(modClass))
                            {
                                var modInstance = Activator.CreateInstance(modClass) as SilkMod;
                                modInstance?.Initialize();
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
                                    modsFailed++;
                                }
                            }

                            modsLoaded++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to load mod {Path.GetFileName(modFile)}: {ex.Message}");
                    Logger.LogError(ex.StackTrace);
                    modsFailed++;
                }
            }

            Logger.LogInfo($"Mods loaded: {modsLoaded}");
            Logger.LogInfo($"Mods failed to load: {modsFailed}");
        }

        public static void LoadBepInEx()
        {
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
}
