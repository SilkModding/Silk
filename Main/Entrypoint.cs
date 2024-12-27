using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            // Log detailed injection information
            var logFile = Path.Combine(logDirectory, "doorstop-start.log");
            var logMessage = $"Injection successful at {DateTime.Now:yyyy-MM-dd_HH-mm-ss}!\n" +
                             $"Process ID: {Process.GetCurrentProcess().Id}\n" +
                             $"App Domain: {AppDomain.CurrentDomain.FriendlyName}\n" +
                             $"Base Directory: {AppDomain.CurrentDomain.BaseDirectory}\n" +
                             $"Loaded Assemblies: {string.Join(", ", AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name))}\n";

            File.AppendAllText(logFile, logMessage + Environment.NewLine);

            // Call the mod manager
            Silk.Main.Run();
        }
    }
}

