using UnityEngine;
using System.IO;
using System.Text;
using System;
using Silk;
using System;
using Logger = Silk.Logger;
using Object = UnityEngine.Object;
using HarmonyLib;
using System.Reflection;

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
            //MethodInfo original;
            //MethodInfo patch;
            //original = AccessTools.Method(typeof(PlayerHandler), "HasAliveTeammate");
            //patch = AccessTools.Method(typeof(MyPatches), "HasAliveTeammate");
            //harmony.Patch(original, new HarmonyMethod(patch));
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(CustomTiersScreen), "Start")]
        public static class AddModMenu
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Logger.LogError("sigma!!!");
                ModsUI.Initialize();
            }
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