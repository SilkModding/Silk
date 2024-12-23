using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Doorstop
{
    public class Entrypoint
    {
        public static void Start()
        {
            // Create the log directory if it doesn't exist
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Log to verify the injection works
            var logFile = Path.Combine(logDirectory, $"doorstop-start_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
            File.AppendAllText(logFile, $"Hello from Unity at {DateTime.Now:yyyy-MM-dd_HH-mm-ss}! (Process ID: {Process.GetCurrentProcess().Id}){Environment.NewLine}");

            // Call the mod manager
            Silk.Main.Run();
        }
    }
}

