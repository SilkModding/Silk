using System;
using System.Text;
using HarmonyLib;

namespace Silk {
    public static class Patches {
        static Harmony harmony = new Harmony("com.Silk.Patcher");
        public static void Patch() {
            try {
                harmony.PatchAll();
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
            ModsUI.Initialize();
        }
    }

    [HarmonyPatch(typeof(GameController), "Start")]
    public static class CheckUpdates {
        public static void Postfix() {
            // Check for updates
            // Updater.CheckForUpdates();
        }
        
    }
}