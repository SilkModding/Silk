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
            // Log to verify the injection works
            var logFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "doorstop_hello.log");
            File.AppendAllText(logFile, $"Hello from Unity at {DateTime.Now:yyyy-MM-dd_HH-mm-ss}! (Process ID: {Process.GetCurrentProcess().Id}){Environment.NewLine}");

            // Call the mod manager
            ModManager.Loader.Initialize();
        }
    }
}

