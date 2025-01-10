using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Silk.Mods
{
    /// <summary>
    /// Utility methods for mods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Checks if Unity has loaded a scene and if the scene is not the default one.
        /// </summary>
        /// <returns>True if a scene is loaded, false otherwise.</returns>
        public static bool IsUnityLoaded()
        {
            // Check if the active scene is valid and loaded
            Scene currentScene = SceneManager.GetActiveScene();
            
            // Unity ensures at least one scene is loaded, so check if it's valid
            if (currentScene.IsValid())
            {
                Logger.LogInfo($"Current Active Scene: {currentScene.name} is loaded.");
                return true;
            }
            else
            {
                Logger.LogWarning("No scene is currently loaded.");
                return false;
            }
        }

        /// <summary>
        /// Checks if a specific mod scene is loaded.
        /// </summary>
        /// <param name="modSceneName">The name of the mod scene to check.</param>
        /// <returns>True if the mod scene is loaded, false otherwise.</returns>
        public static bool IsModSceneLoaded(string modSceneName)
        {
            // Loop through all loaded scenes and check if any matches the mod scene name
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == modSceneName)
                {
                    Logger.LogInfo($"Mod scene {modSceneName} is loaded.");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads a scene by name.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Waits for a scene to load.
        /// </summary>
        /// <param name="sceneName">The name of the scene to wait for.</param>
        /// <returns>An asynchronous operation that completes when the scene is loaded.</returns>
        public static IEnumerator WaitUntilSceneLoaded(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Waits for a scene to unload.
        /// </summary>
        /// <param name="sceneName">The name of the scene to wait for.</param>
        /// <returns>An asynchronous operation that completes when the scene is unloaded.</returns>
        public static IEnumerator WaitUntilSceneUnloaded(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }
}

