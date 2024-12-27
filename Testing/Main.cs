using UnityEngine;
using System.IO;
using System.Text;
using System;
using Silk;
using System;
using Logger = Silk.Logger;
using Object = UnityEngine.Object;
using HarmonyLib;

namespace TestMod
{
    [SilkMod("SilkMod", "Abstractmelon")]
    public class TestMod : SilkMod
    {   
        // This is called when your mod is loaded
        public void Initialize()
        {   
            // Log that your mod loaded
            Logger.LogInfo("Doin cool stuff");
            Harmony harmony = new Harmony("com.SilkModding.SilkExampleMod");
        }

        
        // This is called every frame
        public void Update()
        {
            
        }

        public void Unload()
        {
            
        }
    }
}