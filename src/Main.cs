using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Silk
{
    public static class Main
    {
        // Detect if bepinex is present
        public static bool BepInExPresent => Directory.Exists(@"BepInEx\core\");

        // Flag to make sure we only hook into the scene loading once
        private static bool hooked = false;

        /// <summary>
        /// The main entry point of the mod loader.
        /// Initializes the logging system, hooks into Unity's scene loading process, and loads BepInEx if present.
        /// </summary>
        public static void Run()
        {
            // Initialize - This also starts the logger
            Logger.LogInfo("Starting Silk...");

            // Hook into Unity's scene loading process after Unity has loaded -- https://harmony.pardeike.net/articles/patching-edgecases.html#patching-too-early-missingmethodexception-in-unity
            HookIntoSceneLoading();

            // Test logging
            var testLogging = Config.GetConfigValue<bool>("debug.testLogging");
            Logger.LogInfo($"Config: debug.testLogging = {testLogging}");
            if (testLogging)
            {
                TestLogging();
            }

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

        /// <summary>
        /// Hooks into Unity's scene loading process by attaching a scene loaded listener to the SceneManager.
        /// This is necessary because we need to wait until Unity has finished loading before we can load mods.
        /// </summary>
        private static void HookIntoSceneLoading()
        {
            if (!hooked)
            {   
                // Hook into Unity's scene loading process, calls OnSceneLoaded when unity broadcasts that a scene has been loaded
                SceneManager.sceneLoaded += OnSceneLoaded;
                hooked = true;
            }
        }

        /// <summary>
        /// Called by Unity when a scene is loaded.
        /// This is how we hook into Unity's scene loading process.
        /// </summary>
        /// <param name="scene">The scene that was loaded</param>
        /// <param name="mode">The mode in which the scene was loaded</param>
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // If we haven't hooked into Unity's scene loading process yet, don't do anything
            if (!hooked) return;

            // Unsubscribe from the event to prevent it from firing multiple times
            SceneManager.sceneLoaded -= OnSceneLoaded;
            hooked = false; // Reset the hook flag to avoid re-entry

            // Only execute mod loading code once the scene is fully loaded
            Logger.LogInfo("Unity scene loaded. Starting mod initialization...");

            // Call the mod loader after Unity finishes its scene loading process
            Load();
        }

        /// <summary>
        /// Loads the mods by initializing the mod loader and stealing the console back from Unity.
        /// </summary>
        public static void Load()
        {
            // Steal the console back
            Logger.LogInfo("Stealing the console back...");
            Logger.StealConsoleBack();

            // Initialize the mod loader
            Logger.LogInfo("Initializing the mod loader...");
            Loader.Initialize();

            // Start the harmony patches
            Logger.LogInfo("Starting Patches... ");
            Patches.Patch();

            // Steal logger back from unity
            UnityLogSniper.Initialize();

            Logger.LogInfo("Silk initialization complete.");
        }

        private static void TestLogging()
        {
            Logger.Log("This is a log message");
            Logger.LogInfo("This is an info message");
            Logger.LogWarning("This is a warning message");
            Logger.LogError("This is an error message");
        }
    }
}
