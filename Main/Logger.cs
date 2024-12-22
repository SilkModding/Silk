using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Silk
{
    public static class Logger
    {
        // Import AllocConsole function from kernel32.dll for Windows
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Logs", "mod_manager.log");

        static Logger()
        {
            // Ensure the log directory exists
            var logDirectory = Path.GetDirectoryName(LogFile);
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            // Allocate console for Windows, or open terminal for Linux
            if (Utils.IsWindows())
            {
                AllocConsole(); // Windows-specific console allocation
            }
            else if (Utils.IsLinux())
            {
                OpenLinuxConsole(); // Linux-specific console handling
            }

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

        // General log method
        /// <summary>
        /// Logs a message with the current timestamp and the caller class.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            var logMessage = $"[{DateTime.Now:HH:mm:ss}] [{Utils.GetCallingClass()}] {message}";
            Console.WriteLine(logMessage); // Write to console (works for both Windows and Linux)
            File.AppendAllText(LogFile, logMessage + Environment.NewLine); // Write to file
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

        // Open a terminal for Linux
        private static void OpenLinuxConsole()
        {
            try
            {
                // Try opening the terminal using common terminal emulators
                bool success = false;
                success = Process.Start(new ProcessStartInfo
                {
                    FileName = "gnome-terminal", // For GNOME-based desktops
                    UseShellExecute = true
                }) != null;

                if (!success)
                {
                    success = Process.Start(new ProcessStartInfo
                    {
                        FileName = "xterm", // Fallback to Xterm
                        UseShellExecute = true
                    }) != null;
                }

                if (!success)
                {
                    success = Process.Start(new ProcessStartInfo
                    {
                        FileName = "konsole", // For KDE-based desktops
                        UseShellExecute = true
                    }) != null;
                }

                if (success)
                {
                    Console.WriteLine("Console opened for Linux.");
                }
                else
                {
                    Console.WriteLine("Error opening terminal.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening terminal: {ex.Message}");
            }
        }
    }
}


