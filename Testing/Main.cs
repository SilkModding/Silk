using UnityEngine;
using Silk;
using System;
using Logger = Silk.Logger;

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