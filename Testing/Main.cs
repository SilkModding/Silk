using UnityEngine;
using System.IO;
using System.Text;
using System;
using Silk;
using System.Collections;
using Logger = Silk.Logger;
using Object = UnityEngine.Object;

namespace SilkMod
{
    [SilkMod("SilkMod", "Abstractmelon", "StartMod")]
    public class SilkModMain
    {
        public static void StartMod()
        {
            // Mod initialization logic here
            Logger.Log("SilkMod initialized!");
            
            ModUtils.IsUnityLoaded();
            
            // Start listing and making GameObjects visible
            IEnumerator MakeObjectsVisible()
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Logs", "silk-test.log");
                while (true)
                {
                    try {
                
                        // log stuff
                        Logger.Log("Listing and making GameObjects visible...");

                        // Get all GameObjects in the current scene
                        GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>();

                        Logger.Log($"Found {allGameObjects.Length} GameObjects in the scene:");

                        // Loop through each GameObject and log its name, then make it visible
                        foreach (var obj in allGameObjects)
                        {
                            Logger.Log($"GameObject: {obj.name}");

                            // Make the GameObject visible
                            obj.SetActive(true);
                        }
                    } catch (System.Exception ex) {    
                        string errorMessage = $"Error in mod at {DateTime.Now:yyyy-MM-dd_HH-mm-ss}:\n{ex.Message}\n{ex.StackTrace}";
                        Logger.LogError(errorMessage);
                        File.AppendAllText(logPath, errorMessage + Environment.NewLine);
                    }
                    yield return new WaitForSeconds(5);
                }
            }

            GameObject monoBehaviourObject = new GameObject("SilkModMonoBehaviour");
            SilkModMonoBehaviour monoBehaviour = monoBehaviourObject.AddComponent<SilkModMonoBehaviour>();
            monoBehaviour.StartCoroutine(MakeObjectsVisible());
        }
    }
}

public class SilkModMonoBehaviour : MonoBehaviour
{
    public void Start()
    {
        
    }
}

