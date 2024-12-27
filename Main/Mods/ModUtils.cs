using UnityEngine;
using UnityEngine.SceneManagement;

namespace Silk.Mods
{
    public static class Utils
    {
        // This method checks if Unity has loaded a scene and if the scene is not the default one
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

        // Optionally, you could check if a specific mod scene is loaded
        public static bool IsModSceneLoaded(string modSceneName)
        {
            // Loop through all loaded scenes and check if any matches the mod scene name
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == modSceneName)
                {
                    Debug.Log($"Mod scene {modSceneName} is loaded.");
                    return true;
                }
            }
            return false;
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
