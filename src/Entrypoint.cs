using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

// Entrypoint for Doorstop.
// Explanation: Doorstep calls Doorstop.Entrypoint.Start() when it loads. This file is what gets called by Doorstop, and then calls Silk. This is effectively the pre-preloader.
namespace Doorstop
{
    public class Entrypoint
    {
        /// <summary>
        /// Called when Doorstop injects into the process. This does some logging and then calls <see cref="Silk.Main.Run"/>.
        /// </summary>
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
                             $"Loaded Assemblies: {string.Join(", ", AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name))}\n" +
                             $"Current Directory: {Directory.GetCurrentDirectory()}\n" +
                             $"Command Line Args: {string.Join(" ", Environment.GetCommandLineArgs().Skip(1))}\n" +
                             $"Framework: {typeof(string).Assembly.ImageRuntimeVersion}\n" +
                             $"Runtime: {RuntimeInformation.FrameworkDescription}\n" +
                             $"OS: {RuntimeInformation.OSDescription}\n" +
                             $"Process Architecture: {RuntimeInformation.ProcessArchitecture}\n";

            File.AppendAllText(logFile, logMessage + Environment.NewLine);

            // Call the mod manager
            Silk.Main.Run();
        }
    }
}

