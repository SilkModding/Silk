using UnityEngine;
using Silk;
using Logger = Silk.Logger;

namespace SilkMod
{
    [SilkMod("SilkMod", "Abstractmelon", "StartMod")]
    public class SilkModMain
    {
        public static void StartMod()
        {
            // Mod initialization logic here
            Logger.Log("SilkMod initialized!");

            // Start listing and making GameObjects visible
            try {
                // log stuff
                Logger.Log("Listing and making GameObjects visible...");
                
                 // Get all GameObjects in the current scene
                // GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>();

                // Logger.Log($"Found {allGameObjects.Length} GameObjects in the scene:");

                // // Loop through each GameObject and log its name, then make it visible
                // foreach (var obj in allGameObjects)
                // {
                //     Logger.Log($"GameObject: {obj.name}");

                //     Make the GameObject visible
                //     obj.SetActive(true);
                // }
            } catch (System.Exception ex) {    
                Logger.LogError($"Error in mod: {ex.Message}");
            }
        }
    }
}
