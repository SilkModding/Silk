using System.Collections.Generic;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Silk.Mods
{
    public class ModUtils {
        public static HashSet<string> loadedMods = new HashSet<string>();
        public static HashSet<string> enabledMods = new HashSet<string>();
        public static HashSet<string> activeMods = new HashSet<string>();
        public static HashSet<string> runningMods = new HashSet<string>();

        public static bool IsUnityLoaded() {
            return (bool)typeof(Application).GetProperty("isPlaying", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
        }

        public static bool IsModEnabled(string modName) {
            return enabledMods.Contains(modName);
        }

        public static bool IsModLoaded(string modName) {
            return loadedMods.Contains(modName);
        }

        public static bool IsModActive(string modName) {
            return activeMods.Contains(modName);
        }

        public static bool IsModRunning(string modName) {
            return runningMods.Contains(modName);
        }

        public static void SetModEnabled(string modName, bool enabled) {
            if (enabled) {
                enabledMods.Add(modName);
            } else {
                enabledMods.Remove(modName);
            }
        }

        public static void SetModLoaded(string modName, bool loaded) {
            if (loaded) {
                loadedMods.Add(modName);
            } else {
                loadedMods.Remove(modName);
            }
        }

        public static void SetModActive(string modName, bool active) {
            if (active) {
                activeMods.Add(modName);
            } else {
                activeMods.Remove(modName);
            }
        }

        public static void SetModRunning(string modName, bool running) {
            if (running) {
                runningMods.Add(modName);
            } else {
                runningMods.Remove(modName);
            }
        }
    }
}
