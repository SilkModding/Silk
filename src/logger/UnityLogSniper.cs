using System.Linq;
using UnityEngine;
using LogType = UnityEngine.LogType;

namespace Silk {
    public static class UnityLogSniper {

        /// <summary>
        /// Initializes the UnityLogSniper by attaching a log message received listener that redirects Unity logs to the Silk logging system.
        /// </summary>
        public static void Initialize() {
            if (!Config.GetConfigValue<bool>("logger.redirectUnityLogs")) return;
            Logger.LogInfo("Initializing the UnityLogSniper...");
            Application.logMessageReceived += RedirectUnityLogs;
            Logger.LogInfo("UnityLogSniper has been initialized and is now listening for logs.");
        }

        public static void RedirectUnityLogs(string logMessage, string stackTrace, LogType type)
        {
            string logOutput = $"[{System.DateTime.Now:HH:mm:ss}] [Unity] [{type.ToString().ToUpper()}] {logMessage}";
            Logger.UnityLog(logOutput, type.ToString());
        }
    }
}

