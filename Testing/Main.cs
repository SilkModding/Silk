using System.Collections;
using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;

namespace TestMod
{
    [SilkMod("Silk Example Mod", new[] { "Abstractmelon", "Wackymoder" }, "1.0.0", "1.6a", "silk-example-mod")]
    public class TestMod : SilkMod
    {
        private float timer = 0;

        // This is called when unity loads
        public override void Initialize()
        {
            Logger.LogInfo("Initializing Silk Example Mod...");
            Harmony harmony = new Harmony("com.SilkModding.SilkExampleMod");
            harmony.PatchAll();
            Logger.LogInfo("Harmony patches applied.");
        }

        // This is called when unity is ready
        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        // This is called every frame
        public void Update()
        {
            Logger.LogInfo("Update called.");
            timer += Time.deltaTime;
            Logger.LogInfo($"Timer updated: {timer}");
            if (timer > 1)
            {
                Logger.LogInfo("Timer exceeded 1 second, resetting timer.");
                timer = 0;
                EnemyHealthSystem[] array = UnityEngine.Object.FindObjectsOfType<EnemyHealthSystem>();
                Logger.LogInfo($"Found {array.Length} enemies.");
                for (int i = 0; i < array.Length; i++)
                {
                    Logger.LogInfo($"Disintegrating enemy {i + 1}.");
                    array[i].Disintegrate();
                }
            }
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Silk Example Mod...");
        }
    }
}

