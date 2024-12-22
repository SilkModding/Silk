using System;
using System.IO;

namespace Silk
{
    public static class Main
    {
        public static bool BepInExPresent { get { return Directory.Exists(@"BepInEx\core\"); } }
        public static void Run()
        {
            Logger.Log("Starting Silk...");
            Loader.Initialize();

            // Logger test
            Logger.Log("This is a log message");
            Logger.LogInfo("This is an info message");
            Logger.LogWarning("This is a warning message");
            Logger.LogError("This is an error message");

            if (BepInExPresent)
            {
                Logger.Log("BepInEx was found, attempting to load it...");
                try
                {
                    Loader.LoadBepInEx();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load BepInEx!");
                    Logger.LogError(ex.Message);
                    Logger.LogError(ex.StackTrace);
                }
            }
            else
            {
                Logger.LogWarning("BepInEx was not found, skipping...");
            }
        }
    }
}