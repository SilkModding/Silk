using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Silk
{
    public static class Loader
    {
        private static readonly string ModsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Mods");

        public static void Initialize()
        {
            Logger.Log("Silk Loader Initialized");

            // Ensure the mods folder exists
            if (!Directory.Exists(ModsFolder))
            {
                Logger.Log("Mods folder does not exist, creating it");
                Directory.CreateDirectory(ModsFolder);
            }

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
                    Logger.Log($"Loading mod: {Path.GetFileName(modFile)}");

                    // Inspect mod with Mono.Cecil
                    using (var assemblyDefinition = AssemblyDefinition.ReadAssembly(modFile))
                    {
                        Logger.Log($"Mod assembly name: {assemblyDefinition.Name.Name}");

                        foreach (var type in assemblyDefinition.MainModule.Types)
                        {
                            Logger.Log($"Mod type: {type.Name}");

                            // Look for a type with the SilkMod attribute
                            var silkModAttribute = type.CustomAttributes
                                .Where(attr => attr.AttributeType.FullName == typeof(SilkModAttribute).FullName)
                                .FirstOrDefault();

                            if (silkModAttribute != null)
                            {
                                var modName = silkModAttribute.ConstructorArguments[0].Value.ToString();
                                var modAuthor = silkModAttribute.ConstructorArguments[1].Value.ToString();
                                var modEntryPoint = silkModAttribute.ConstructorArguments.Count > 2 
                                    ? silkModAttribute.ConstructorArguments[2].Value.ToString() 
                                    : "Initialize";

                                Logger.Log($"Found entry point in type: {type.Name}");
                                Logger.Log($"Mod Name: {modName}");
                                Logger.Log($"Mod Author: {modAuthor}");

                                var assembly = Assembly.LoadFrom(modFile);
                                var entryPointClass = assembly.GetType(type.FullName);

                                if (entryPointClass != null)
                                {
                                    var entryPointMethod = entryPointClass.GetMethod(modEntryPoint);

                                    if (entryPointMethod != null)
                                    {
                                        Logger.Log($"Found {modEntryPoint}() method");
                                        entryPointMethod.Invoke(null, null); // Call the entry point method
                                    }
                                    else
                                    {
                                        Logger.Log($"No method named {modEntryPoint} found in {type.Name}");
                                    }
                                }
                                else
                                {
                                    Logger.Log($"Failed to load class {type.FullName}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to load mod {Path.GetFileName(modFile)}: {ex.Message}");
                }
            }
        }
    }
}