using System;

namespace Silk
{
    public static class Main
    {
        public static void Run()
        {
            Logger.Log("Starting Silk...");
            Loader.Initialize();

            // Logger test
            Logger.Log("This is a log message");
            Logger.LogInfo("This is an info message");
            Logger.LogWarning("This is a warning message");
            Logger.LogError("This is an error message");
        }
    }
}