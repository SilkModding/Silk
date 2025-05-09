using System;
using System.Text;
using HarmonyLib;

namespace Silk {
    public static class Patches {
        static Harmony harmony = new Harmony("com.Silk.Patcher");
        public static void Patch() {
            try {
                Logger.LogInfo("Patching Silk...");
                harmony.PatchAll();

                Logger.LogInfo("Patching Silk API...");
                harmony.PatchAll(typeof(API.Weapons));

                Logger.LogInfo("Patching complete.");
            } catch (Exception e) {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
            }
        }
    }

    [HarmonyPatch(typeof(CustomTiersScreen), "Start")]
    public static class AddModMenu {
        [HarmonyPostfix]
        public static void Postfix() {
            Updater.CheckForUpdates();
            ModsUI.Initialize();
        }
    }

    [HarmonyPatch(typeof(SteamLeaderboards), "UpdateScore")]
    internal class DisableLeaderboard {
        public static bool Prefix(int score) {
            if (Utils.onlineMods) {
                Logger.LogInfo("Leaderboard update disabled.");
                Utils.Announce("Mods effecting the gameplay have been detected. Leaderboard scores for this current session have been disabled.", 255, 0, 0);
                return false;
            }
            return true;
        }
    }
}