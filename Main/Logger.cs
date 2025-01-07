using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Epic.OnlineServices.Stats;

namespace Silk
{
    public static class Logger
    {
        // Import AllocConsole function from kernel32.dll for Windows
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(uint dwProcessId);

        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Logs", "mod_manager.log");

        /// <summary>
        /// Static constructor for Logger class.
        /// 
        /// This class is a utility for logging messages to a file.
        /// It creates a new process with a console window and redirects the standard output and error streams to the console window.
        /// It also deletes the previous log file if it exists.
        /// </summary>
        static Logger()
        {
            // Ensure the log directory exists
            var logDirectory = Path.GetDirectoryName(LogFile);
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            // Create a new process
            Process process = new Process();

            // Set the process start info
            process.StartInfo.FileName = "cmd.exe";

            // Redirect the standard input, output, and error streams
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;

            // Start the process
            process.Start();

            // It breaks if this isn't here
            Thread.Sleep(1000);

            // Connects the console window to spiderheck
            AttachConsole((uint)process.Id);

            // Sets the output to the console window thing
            StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
            Console.SetOut(streamWriter);

            // Delete the previous log file
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

            // Improve the console appearance
            Console.Title = "Silk Mod Manager";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }
        
        /// <summary>
        /// Re-attaches the console to the process and sets the output to the console.
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
            var logMessage = $"[{DateTime.Now:HH:mm:ss}] [{Utils.GetCallingStack()}] {message}";
            Console.WriteLine(logMessage); // Write to console (works for both Windows and Linux)
            File.AppendAllText(LogFile, logMessage + Environment.NewLine); // Write to file
        }

        // General log method
        /// <summary>
        /// Logs a message with the current timestamp and the caller class.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void UnityLog(string message, string logType = "None")
        {
            switch (logType)
            {
                case "Error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "Warning":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "Info":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine(message); // Write to console (works for both Windows and Linux)
            File.AppendAllText(LogFile, message + Environment.NewLine); // Write to file
            Console.ResetColor();
        }

        /// <summary>
        /// Logs an informational message with cyan color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Log($"[INFO] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Logs a warning message with yellow color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log($"[WARNING] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Logs an error message with red color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log($"[ERROR] {message}");
            Console.ResetColor();
        }
    }
}


