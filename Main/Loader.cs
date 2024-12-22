using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace ModManager
{
    public static class Loader
    {
        private static readonly string ModsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Mods");

        public static void Initialize()
        {
            Console.WriteLine("Mod Manager Initialized");

            // Ensure the mods folder exists
            if (!Directory.Exists(ModsFolder))
                Directory.CreateDirectory(ModsFolder);

            // Load mods
            LoadMods();
        }

        private static void LoadMods()
        {
            var modFiles = Directory.GetFiles(ModsFolder, "*.dll");

            foreach (var modFile in modFiles)
            {
                try
                {
                    Console.WriteLine($"Loading mod: {Path.GetFileName(modFile)}");

                    // Inspect mod with Mono.Cecil
                    using (var assemblyDefinition = AssemblyDefinition.ReadAssembly(modFile))
                    {
                        foreach (var type in assemblyDefinition.MainModule.Types)
                        {
                            if (type.Name == "ModEntryPoint") // Check for `ModEntryPoint`
                            {
                                var assembly = Assembly.LoadFrom(modFile);
                                var entryPoint = assembly.GetType(type.FullName)?.GetMethod("Initialize");

                                entryPoint?.Invoke(null, null); // Call `ModEntryPoint.Initialize()`
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load mod {Path.GetFileName(modFile)}: {ex.Message}");
                }
            }
        }
    }
}
