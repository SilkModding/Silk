using System.Collections;
using System.Collections.Generic;
using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using Silk.API;
using UnityEngine.InputSystem;

namespace SilkTestMod
{
    [SilkMod("Silk Example Mod", new[] { "Abstractmelon", "Wackymoder" }, "1.0.0", "0.6.0", "silk-example-mod", 1)]
    public class TestMod : SilkMod
    {
        public const string ModId = "silk-example-mod";

        // This is called when unity loads
        public override void Initialize()
        {
            Logger.LogInfo("Initializing the Silk Example Mod...");
            
            // Initialize mod config with default values
            var defaultConfig = new Dictionary<string, object>
            {
                { "enableChristmas", true },
                { "autoKill", new Dictionary<string, object>
                    {
                        { "enabled", false },
                        { "interval", 0.3f }
                    }
                }
            };
            
            Config.LoadModConfig(ModId, defaultConfig);

            CustomWeapon longSwordWeapon = new CustomWeapon("Long Sword", Weapons.WeaponType.ParticleBlade);
            Weapons.AddNewWeapon(longSwordWeapon);
            Weapons.OnInitCompleted += () =>
            {
                ParticleBlade particleBladeComponent = longSwordWeapon.WeaponObject.GetComponent<ParticleBlade>();
                particleBladeComponent.baseSize = new Vector2(10, 100);
            };
        }

        // This is called when unity is readys
        public void Awake()
        {
            // This is called when unity is ready
            Logger.LogInfo("Awake called.");

            // Apply Harmony patches
            Logger.LogInfo("Applying Harmony patches...");

            // Initialize Harmony
            Harmony harmony = new Harmony("com.SilkModding.SilkExampleMod");
            harmony.PatchAll(typeof(Patches));

            // Apply Harmony patches
            Logger.LogInfo("Harmony patches applied.");
        }

        private bool _timerStarted = false;
        private float _timer = 0f;

        // This is called every frame
        public void Update()
        {
            if (Keyboard.current.xKey.wasPressedThisFrame && !_timerStarted)
            {
                _timerStarted = true;
                Config.SetModConfigValue(ModId, "autoKill.enabled", true);
                Logger.LogInfo("Auto-kill enabled.");
            }

            if (Keyboard.current.zKey.wasPressedThisFrame && _timerStarted)
            {
                _timerStarted = false;
                Config.SetModConfigValue(ModId, "autoKill.enabled", false);
                Logger.LogInfo("Auto-kill disabled.");
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                KillEnemies();
            }

            if (_timerStarted && Config.GetModConfigValue<bool>(ModId, "autoKill.enabled", false))
            {
                _timer += Time.deltaTime;
                float interval = Config.GetModConfigValue<float>(ModId, "autoKill.interval", 0.3f);
                if (_timer > interval)
                {
                    _timer = 0;
                    KillEnemies();
                }
            }
        }


        private void KillEnemies()
        {
            EnemyHealthSystem[] array = FindObjectsOfType<EnemyHealthSystem>();
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Disintegrate();
            }
            Logger.LogInfo($"Killed {array.Length} enemies.");
        }

        /// <summary>
        /// Unloads the Silk Example Mod.
        /// </summary>
        /// <remarks>
        /// This method is called by Silk when the mod is unloaded.
        /// </remarks>
        public override void Unload()
        {
            Logger.LogInfo("Unloading Silk Example Mod...");
        }

        /// <summary>
        /// Loads a texture from a file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The loaded texture.</returns>
        private Texture2D LoadTextureFromFile(string filePath)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(fileData);

            return texture;
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(SeasonChecker), nameof(SeasonChecker.IsItChristmas))]
        [HarmonyPrefix]
        public static bool MakeItChristmas(ref bool __result)
        {
            __result = Config.GetModConfigValue(TestMod.ModId, "enableChristmas", true);
            return false;
        }
    }
}
