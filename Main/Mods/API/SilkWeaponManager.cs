using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;
using static Silk.API.Weapons;

namespace Silk.API {
    public static class Weapons {
        // 
        private static uint _prefabHandlerId = 7777;
        public static List<Weapon> NewWeapons = new List<Weapon>();
        private static List<CustomWeapon> _newUserWeapons = new List<CustomWeapon>();
        private static bool _weaponsAdded = false;

        // Events
        public delegate void CallbackDelegate();
        public static event CallbackDelegate OnInitCompleted;


        // Add weapon function, mods will call this to add their weapons
        /// <summary>
        /// Adds a new custom weapon to the list of weapons
        /// </summary>
        /// <param name="weapon">The custom weapon to be added.</param>
        public static void AddNewWeapon(CustomWeapon weapon) {
            Logger.LogInfo($"Adding new weapon: {weapon.Name}");
            _newUserWeapons.Add(weapon);
        }

        // Create the new weapons list
        [HarmonyPatch(typeof(SelectedCustomTierScreen), nameof(SelectedCustomTierScreen.Awake))] // TODO: make it dynamicly fit screen
        public static class FixWeaponsVisualPatch {
            [HarmonyPostfix]
            public static void Postfix(ref SelectedCustomTierScreen __instance) {
                GameObject grid = GameObject.Find("View - VersusWeapons/Grid");
                var gridLayout = grid.GetComponent<GridLayoutGroup>();
                gridLayout.spacing = new Vector2(5, 15);
                gridLayout.constraintCount = 8;
                gridLayout.cellSize = new Vector2(76, 40);
            }
        }

        // Add weapons patches
        [HarmonyPatch(typeof(CustomMapUI), nameof(CustomMapUI.Awake))]
        [HarmonyPatch(typeof(CustomTiersScreen), nameof(CustomTiersScreen.Awake))]
        [HarmonyPatch(typeof(SelectedCustomTierScreen), nameof(SelectedCustomTierScreen.Awake))]
        [HarmonyPatch(typeof(CustomMapEditor), nameof(CustomMapEditor.Awake))]
        [HarmonyPatch(typeof(CustomTierWavesScreen), nameof(CustomTierWavesScreen.Setup))]
        public static class AddWeaponsPatch {
            [HarmonyPrefix]
            public static void Prefix() => AddWeapons();
        }
        [HarmonyPatch(typeof(VersusMode), "Awake")]
        public static class WeaponsPatch6 {

            [HarmonyPostfix]
            public static void Postfix(ref VersusMode __instance) {
                AddWeapons();
                __instance.weapons.AddRange(NewWeapons.Select(x => new SpawnableWeapon(x, 3)));
            }
        }

        [HarmonyPatch(typeof(CustomTierWeaponButton), "Setup")]
        public static class FixNewWeaponName {
            [HarmonyPostfix]
            public static void Postfix(ref CustomTierWeaponButton __instance) { // TODO: save original translation
                AccessTools.FieldRefAccess<CustomTierWeaponButton, TextMeshProUGUI>("titleText")(__instance).text =
                    AccessTools.FieldRefAccess<CustomTierWeaponButton, Weapon>("_weaponObj")(__instance).name;
            }
        }


        // Function to actually add the weapons
        static void AddWeapons() {
            var tmp = Resources.FindObjectsOfTypeAll<ElementLists>();

            if (tmp.Length == 0 || _weaponsAdded) {
                return;
            }

            ElementLists list = tmp[0];
            SerializationWeaponName maxName = (SerializationWeaponName)Enum.GetValues(typeof(SerializationWeaponName)).Cast<int>().Max();
            NewWeapons.AddRange(list.allWeapons);
            List<Weapon> tmpList = new List<Weapon>();

            foreach (var weapon in _newUserWeapons) {
                Weapon newWeapon = NewWeapons[(int)weapon.Type];
                GameObject copiedWeapon = UnityEngine.Object.Instantiate(newWeapon.gameObject);
                GameObject.DontDestroyOnLoad(copiedWeapon.gameObject);
                copiedWeapon.SetActive(false);
                weapon.WeaponObject = copiedWeapon;

                newWeapon = copiedWeapon.GetComponent<Weapon>();
                newWeapon.serializationWeaponName = ++maxName;
                newWeapon.name = weapon.Name;
                newWeapon.usedInCustomTiers = true;

            }
            OnInitCompleted?.Invoke();
            foreach (var weapon in _newUserWeapons) {
                var no = weapon.WeaponObject.GetComponent<NetworkObject>();
                var handler = new WeaponNetworkPrefabInstanceHandler(_prefabHandlerId++, no);
                NetworkManager.Singleton.PrefabHandler.AddHandler(handler.Id, handler);

                weapon.WeaponObject.transform.TransformPoint(9999 + tmpList.Count * 1000, 9999, 9999);
                tmpList.Add(weapon.WeaponObject.GetComponent<Weapon>());
            }
            NewWeapons = tmpList;
            list.allWeapons.AddRange(NewWeapons);
            _weaponsAdded = true;
        }

        [HarmonyPatch(typeof(WeaponSpawner), nameof(WeaponSpawner.SpawnWeapon))]
        public static class SpawnWeaponFix { // This just in case there will be timer weapon to live

            [HarmonyPrefix]
            public static void Prefix(ref WeaponSpawner __instance) {
                foreach (var w in NewWeapons) {
                    w.gameObject.SetActive(true);
                }
            }
            [HarmonyPostfix]
            public static void Postfix(ref WeaponSpawner __instance) {
                foreach (var w in NewWeapons) {
                    w.gameObject.SetActive(false);
                }
            }
        }

        // Weapon types
        public enum WeaponType {
            BigGrenade, BigParticleBlade, BurstLauncher, DeathRay, DoubleParticleBlade,
            DoubleRailvolver, GrenadeLauncher, GrenadeParkour, Grenade, HeckSaw, KhepriStaff,
            LaserCannonParkour, LaserCannon, LaserCube, Mine, MineParkour, MiniShotgun,
            BeachBall, FireworkLauncherParkour, FireworkLauncher, Flare, Flashlight,
            ParticleBladeLauncher, ParticleBladeParkour, ParticleBlade, ParticleSpearParkour,
            ParticleSpearVariant, PreArmedGrenade, PreArmedParticleBlade, Railvolver,
            RailvolverParkour, RocketLauncher, RocketLauncherParkour, Shotgun, ShotgunParkour,
            AutoShotgun, GravityGrenade, GravitySaw, BoomStick, DeathCube, Snowball,
        }
    }

    // Custom weapon class
    public class CustomWeapon {

        public WeaponType Type;
        public GameObject WeaponObject;
        public string Name;
        public CustomWeapon(string name, WeaponType inheritFrom) {
            Name = name;
            Type = inheritFrom;
        }

    }
}
