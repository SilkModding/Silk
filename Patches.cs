using HarmonyLib;

namespace SilkLoader
{
    public static class Patcher
    {
        public static void ApplyPatches()
        {
            try
            {
                // Apply Harmony patches
                var harmony = new Harmony("com.SilkLoader.Main");
                harmony.PatchAll();
                Logger.Log("Harmony patches applied.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error applying patches: {ex}");
            }
        }
    }
}
