using System;
using System.Collections.Generic;
using Silk;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silk.Mods
{
    public static class Manager
    {
        public static List<SilkMod> Mods = new();

        /// <summary>
        /// Sets up the mods by creating a parent GameObject and adding each mod as a component.
        /// Logs the number of mods found and the names of the mods.
        /// </summary>
        public static void SetupMods()
        {   
            Logger.LogInfo("Setting up mods...");

            // Create a parent GameObject to hold all the mods
            GameObject modsParent = GameObject.Find("Mods");

            // If the parent GameObject doesn't exist, create it
            if (modsParent == null)
            {
                Logger.LogInfo("Mods parent not found, creating...");

                // Create the parent GameObject
                modsParent = new GameObject("Mods");

                // Don't destroy the parent GameObject
                Object.DontDestroyOnLoad(modsParent);
            }

            // Some logging
            Logger.LogInfo("Initializing mods...");
            Logger.LogInfo($"Found {Mods.Count} mods");
            Logger.LogInfo(string.Join(", ", Mods.Select(m => m.GetType().Name)));

            // Initialize each mod
            foreach (var mod in Mods)
            {
                Logger.LogInfo($"Initializing mod: {mod.GetType().Name}");

                // Create a GameObject for the mod
                GameObject modGameObject = new(mod.GetType().Name);

                // Add the mod as a component
                modGameObject.transform.SetParent(modsParent.transform);
                try
                {
                    modGameObject.AddComponent(mod.GetType());
                    Logger.LogInfo($"Mod {mod.GetType().Name} initialized successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to initialize mod: {mod.GetType().Name}");
                    Logger.LogError(ex.Message);
                    Logger.LogError(ex.StackTrace);
                }
            }
            
            Logger.LogInfo("Mods initialized");
            Logger.LogInfo("Mods loaded: " + string.Join(", ", Mods.Select(m => m.GetType().Name)));
        }

        /// <summary>
        /// Adds a mod to the list of mods to load.
        /// </summary>
        /// <param name="mod">The mod to add</param>
        public static void AddMod(SilkMod mod)
        {
            Logger.LogInfo($"Adding mod: {mod.GetType().Name}");
            Mods.Add(mod);
            Logger.LogInfo($"Mod {mod.GetType().Name} added");
        }
    }
}


