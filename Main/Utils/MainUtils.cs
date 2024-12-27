using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Silk {
    public static class Utils {

        // Check if the OS is Windows
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        // Check if the OS is Linux
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// It gets the function that called the function thats happening rn.
        /// Basically if A() calls B() then B() calls this, then this will tell you
        /// what class A() is from
        /// </summary>
        /// <returns>The class that called your function</returns>
        public static string GetCallingClass()
        {
            // Basically its meant for errors but it also logs every function that is called!
            StackTrace stackTrace = new StackTrace();
            if (stackTrace.FrameCount >= 3)
            {
                Type callingClass = stackTrace.GetFrame(2).GetMethod().DeclaringType;
                return callingClass?.Name;
            }
            else
            {
                // This should never happen, but if it does here it is.
                return "n/a";
            }
        }

        /// <summary>
        /// Gets the path of the assembly where the calling class is from.
        /// </summary>
        /// <returns>The path of the assembly of the calling class</returns>
        public static string GetCallingAssemblyPath()
        {
            StackFrame stackFrame = new StackFrame(2, false);
            Assembly callingAssembly = Assembly.GetAssembly(stackFrame.GetMethod().DeclaringType);
            return callingAssembly?.Location;
        }

        /// <summary>
        /// Gets the path of the assembly where the calling class is from, but with the mods folder.
        /// </summary>
        /// <returns>The path of the assembly of the calling class</returns>
        public static string GetCallingModPath()
        {
            StackFrame stackFrame = new StackFrame(2, false);
            Assembly callingAssembly = Assembly.GetAssembly(stackFrame.GetMethod().DeclaringType);
            string assemblyPath = callingAssembly?.Location;
            string modsFolder = GetModsFolder();
            string modPath = Path.Combine(modsFolder, assemblyPath);
            return modPath;
        }

        /// <summary>
        /// Gets the path of the mods folder.
        /// </summary>
        /// <returns>The path of the mods folder</returns>
        public static string GetModsFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Silk", "Mods");
        }

        /// <summary>
        /// Checks if the assembly is a mod.
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <returns>True if the assembly is a mod</returns>
        public static bool IsMod(Assembly assembly)
        {
            var modAttribute = assembly.GetCustomAttributes(typeof(SilkModAttribute), false).FirstOrDefault() as SilkModAttribute;
            return modAttribute != null;
        }

        public static void Announce(string text, int colorR, int colorG, int colorB, Type gameTypeRef) {
            var color = new UnityEngine.Color(colorR, colorG, colorB);
            Announcer.instance.Announce(text, color, true);
        }

        public static string GetRelativePath(string path) {
            return path.Substring(Directory.GetCurrentDirectory().Length + 1);
        }

        public static string GetPathSafe(string path) {
            return path.Replace('\\', '/').Replace(" ", "_");
        }

        public static void DeleteDirectory(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
        }

        public static void CopyDirectory(string source, string destination) {
            if (!Directory.Exists(destination)) {
                Directory.CreateDirectory(destination);
            }

            foreach (string file in Directory.GetFiles(source)) {
                string destPath = Path.Combine(destination, Path.GetFileName(file));
                File.Copy(file, destPath, true);
            }

            foreach (string folder in Directory.GetDirectories(source)) {
                string destPath = Path.Combine(destination, Path.GetFileName(folder));
                CopyDirectory(folder, destPath);
            }
        }
    }
}

