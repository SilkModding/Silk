using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Silk
{
    public static class Main
    {
        public static bool BepInExPresent { get { return Directory.Exists(@"BepInEx\core\"); } }

        // Flag to make sure we only hook into the scene loading once
        private static bool hooked = false;

        public static void Run()
        {
            
            Logger.LogInfo("Starting Silk...");
            
            // Hook into Unity's scene loading process after Unity has loaded
            HookIntoSceneLoading();

            // Test logging
            Logger.Log("This is a log message");
            Logger.LogInfo("This is an info message");
            Logger.LogWarning("This is a warning message");
            Logger.LogError("This is an error message");

            if (BepInExPresent)
            {
                Logger.LogInfo("BepInEx was found, attempting to load it...");
                try
                {
                    Loader.LoadBepInEx();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load BepInEx!");
                    Logger.LogError(ex.Message);
                    Logger.LogError(ex.StackTrace);
                }
            }
            else
            {
                Logger.LogWarning("BepInEx was not found, skipping...");
            }
        }

        private static void HookIntoSceneLoading()
        {
            if (!hooked)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                hooked = true;
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Only execute mod loading code once the scene is fully loaded
            Logger.LogInfo("Unity scene loaded. Starting mod initialization...");

            // Call the mod loader after Unity finishes its scene loading process
            Load();

            // Steal logs
            UnityLogSniper.Initialize();

            // Unsubscribe from the event to prevent it from firing again
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static void Load()
        {
            // Steal the console back
            Logger.LogInfo("Stealing the console back...");
            Logger.StealConsoleBack();

            // Initialize the mod loader
            Logger.LogInfo("Initializing the mod loader...");
            Loader.Initialize();
        }
    }
}
