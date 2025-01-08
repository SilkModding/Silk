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

    [HarmonyPatch(typeof(GameController), "Start")]
    public static class CheckUpdates {
        public static void Postfix() {
            // Check for updates
            // Updater.CheckForUpdates();
        }
        
    }
}