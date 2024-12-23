using UnityEngine;
using LogType = UnityEngine.LogType;

namespace Silk {
    public static class UnityLogSniper {

        public static event System.Action<string, string, LogType> OnLogReceived;

        public static void Initialize() {
            Logger.LogInfo("Initializing UnityLogSniper...");
            Application.logMessageReceived += HandleLog;
            Logger.LogInfo("UnityLogSniper initialized and listening for logs.");
        }

        public static void RedirectUnityLogs(string logMessage, string stackTrace, LogType type)
        {
            string logOutput = $"[{System.DateTime.Now:HH:mm:ss}] [Unity] {logMessage}";
            if (type == LogType.Warning)
            {
                logOutput = $"[WARNING] {logOutput}";
            }
            else if (type == LogType.Error || type == LogType.Exception)
            {
                logOutput = $"[ERROR] {logOutput}";
            }
            
            Logger.UnityLog(logOutput); 
        }

        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Redirect the log to the Logger
            RedirectUnityLogs(logString, stackTrace, type);
        }
    }
}
