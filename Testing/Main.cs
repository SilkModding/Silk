using System.Collections;
using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using Silk.API;
using UnityEngine.InputSystem;

namespace SilkTestMod
{
    [SilkMod("Silk Example Mod", new[] { "Abstractmelon", "Wackymoder" }, "1.0.0", "0.5.0", "silk-example-mod")]
    public class TestMod : SilkMod
    {
        private float timer = 0;

        // This is called when unity loads
        public override void Initialize()
        {
            Logger.LogInfo("Initializing the Silk Example Mod...");

            CustomWeapon longSwordWeapon = new CustomWeapon("Long Sword", Weapons.WeaponType.ParticleBlade);
            Weapons.AddNewWeapon(longSwordWeapon);
            Weapons.OnInitCompleted += () =>
            {
                ParticleBlade particleBladeComponent = longSwordWeapon.WeaponObject.GetComponent<ParticleBlade>();
                particleBladeComponent.baseSize = new Vector2(10, 100);
            };
        }

        // This is called when unity is ready
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

        private bool timerStarted = false;
        private bool autoKillEnabled = false;

        // This is called every frame
        public void Update()
        {
            if (Keyboard.current.xKey.wasPressedThisFrame && !timerStarted)
            {
                Logger.LogInfo("Auto-kill enabled.");
                timerStarted = true;
                autoKillEnabled = true;
            }

            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                autoKillEnabled = false;
                timerStarted = false;
                Logger.LogInfo("Auto-kill disabled.");
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                KillEnemies();
            }

            if (timerStarted && autoKillEnabled)
            {
                timer += Time.deltaTime;
                if (timer > 0.3)
                {
                    timer = 0;
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
            __result = true;
            return false;
        }
    }
}