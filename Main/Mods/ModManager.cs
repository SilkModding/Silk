using System;
using System.Collections.Generic;
using Silk;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silk.Mods
{
    public static class Manager
    {
        public static List<SilkMod> Mods = new List<SilkMod>();

        public static void SetupMods()
        {
            Logger.LogInfo("Setting up mods...");
            GameObject modsParent = GameObject.Find("Mods");
            if (modsParent == null)
            {
                Logger.LogInfo("Mods parent not found, creating...");
                modsParent = new GameObject("Mods");
                Object.DontDestroyOnLoad(modsParent);
            }

            Logger.LogInfo("Initializing mods...");
            Logger.LogInfo($"Found {Mods.Count} mods");
            Logger.LogInfo(string.Join(", ", Mods.Select(m => m.GetType().Name)));
            foreach (var mod in Mods)
            {
                Logger.LogInfo($"Initializing mod: {mod.GetType().Name}");
                GameObject modGameObject = new GameObject(mod.GetType().Name);
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

        public static void AddMod(SilkMod mod)
        {
            Logger.LogInfo($"Adding mod: {mod.GetType().Name}");
            Mods.Add(mod);
            Logger.LogInfo($"Mod {mod.GetType().Name} added");
        }
    }
}

