using System.Linq;
using UnityEngine;
using LogType = UnityEngine.LogType;

namespace Silk {
    public static class UnityLogSniper {

        public static void Initialize() {
            Logger.LogInfo("Initializing UnityLogSniper...");
            Application.logMessageReceived += RedirectUnityLogs;
            Logger.LogInfo("UnityLogSniper initialized and listening for logs.");
        }

        public static void RedirectUnityLogs(string logMessage, string stackTrace, LogType type)
        {
            string logOutput = $"[{System.DateTime.Now:HH:mm:ss}] [Unity] [{type.ToString().ToUpper()}] {logMessage}";
            Logger.UnityLog(logOutput, type.ToString());
        }
    }
}

