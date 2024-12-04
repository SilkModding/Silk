using System;
using System.IO;

namespace SilkLoader
{
    public static class Logger
    {
        private static string logPath;

        static Logger()
        {
            string silkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk");

            // Create Silk directory if not exists
            if (!Directory.Exists(silkPath))
            {
                Directory.CreateDirectory(silkPath);
            }

            // Log file path
            logPath = Path.Combine(silkPath, "Loader.log");

            // Create log file if it doesn't exist
            if (!File.Exists(logPath))
            {
                File.Create(logPath).Close();
            }
        }

        public static void Log(string message)
        {
            string logMessage = $"[{DateTime.Now}] {message}\n";
            File.AppendAllText(logPath, logMessage);
        }
    }
}
