using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SilkLoader
{
    public static class Loader
    {
        public static void Main()
        {
            try
            {
                // Initialize the preloader (logging and directory setup)
                Preloader.Initialize();

                // Load mods
                string silkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk");
                string modsPath = Path.Combine(silkPath, "Mods");
                string[] modFiles = Directory.GetFiles(modsPath, "*.dll");

                foreach (string modFile in modFiles)
                {
                    try
                    {
                        Assembly modAssembly = Assembly.LoadFrom(modFile);
                        Logger.Log($"Loaded mod: {modFile}");

                        // Look for entry points and initialize them
                        var entryTypes = modAssembly.GetTypes().Where(t => t.GetMethod("Initialize") != null);
                        foreach (var entryType in entryTypes)
                        {
                            MethodInfo initializeMethod = entryType.GetMethod("Initialize");
                            initializeMethod.Invoke(null, null);
                            Logger.Log($"Initialized mod: {entryType.FullName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Failed to load mod {modFile}: {ex}");
                    }
                }

                // Apply patches using Harmony
                Patcher.ApplyPatches();
            }
            catch (Exception ex)
            {
                Logger.Log($"Fatal error: {ex}");
            }
        }
    }
}
