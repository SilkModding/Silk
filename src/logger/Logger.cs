using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Silk
{
    public static class Logger
    {
        // Import AllocConsole function from kernel32.dll for Windows
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Logs", "mod_manager.log");
        private static readonly bool DebugEnabled = Config.GetConfigValue("EnableDebugLogging")?.ToLower() == "true";

        static Logger()
        {
            AllocConsole();
            
            var logDirectory = Path.GetDirectoryName(LogFile);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (File.Exists(LogFile))
            {
                try
                {
                    File.Delete(LogFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting previous log file: {ex.Message}");
                }
            }

            Console.Title = "Silk Mod Manager";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            // Redirect standard output to the new console
            var writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
            Console.SetOut(writer);
        }

        /// <summary>
        /// Re-attaches the console to the process and sets the output to the console.
        /// This is necessary because Unity spawns a console by default, but if it exists, Unity takes control of it.
        /// We want the console to be used by Silk instead, so we have to take it back.
        /// </summary>
        public static void StealConsoleBack() {
            // Re-attach the console to the process
            AttachConsole((uint)Process.GetCurrentProcess().Id);

            // Set the output to the console
            StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
            Console.SetOut(streamWriter);
        }

        // General log method
        /// <summary>
        /// Logs a message with the current timestamp and the caller class.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            WriteLog(message, Console.ForegroundColor);
        }

        public static void UnityLog(string message, string logType = "None")
        {
            ConsoleColor color = logType switch
            {
                "Error" => ConsoleColor.Red,
                "Warning" => ConsoleColor.Yellow,
                "Info" => ConsoleColor.Cyan,
                _ => Console.ForegroundColor
            };
            WriteLog(message, color, false);
        }

        public static void LogInfo(string message)
        {
            WriteLog($"[INFO] {message}", ConsoleColor.Cyan);
        }

        public static void LogWarning(string message)
        {
            WriteLog($"[WARNING] {message}", ConsoleColor.Yellow);
        }

        public static void LogError(string message)
        { 
            WriteLog($"[ERROR] {message}", ConsoleColor.Red);
        }

        public static void LogDebug(string message)
        {
            if (DebugEnabled) WriteLog($"[DEBUG] {message}", ConsoleColor.Gray);
        }

        private static void WriteLog(string message, ConsoleColor color, bool includeMetadata = true)
        {
            var logMessage = includeMetadata ? $"[{DateTime.Now:HH:mm:ss}] [{Utils.GetCallingStack()}] {message}" : message;
            
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(logMessage);
            Console.ForegroundColor = originalColor;

            File.AppendAllText(LogFile, logMessage + Environment.NewLine);
        }
    }
}
