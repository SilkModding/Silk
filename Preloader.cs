using System;
using System.IO;

namespace SilkLoader
{
    public static class Preloader
    {
        public static void Initialize()
        {
            try
            {
                string silkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk");

                // Ensure Silk directory exists
                if (!Directory.Exists(silkPath))
                {
                    Directory.CreateDirectory(silkPath);
                }

                // Initialize log file
                Logger.Log("SilkLoader initialized.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Fatal error in preloading: {ex}");
            }
        }
    }
}
