using Silk;
using Logger = Silk.Logger;
using HarmonyLib;

namespace TestMod
{
    [SilkMod("Silk Example Mod", new[] { "Abstractmelon", "Wackymoder" }, "1.0.0", "1.6a", "silk-example-mod")]
    public class TestMod : SilkMod
    {   
        // This is called when your mod is loaded
        public void Initialize()
        {   
            // Log that your mod loaded
            Logger.LogInfo("Doin cool stuff");

            Harmony harmony = new Harmony("com.SilkModding.SilkExampleMod");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(CustomTiersScreen), "Start")]
        public static class Log
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Logger.Log("sigma!!!");
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