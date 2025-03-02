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
        public static string? GetCallingClass()
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
        /// Retrieves the class name that is three levels up in the call stack.
        /// If A() calls B(), B() calls C(), and C() calls this, it will return the class of A().
        /// </summary>
        /// <returns>The class name that is three levels up in the call stack.</returns>
        public static string? GetCallingStack()
        {
            StackTrace stackTrace = new();
            if (stackTrace.FrameCount >= 4)
            {
                Type callingClass = stackTrace.GetFrame(3).GetMethod().DeclaringType;
                return callingClass?.Name;
            }
            else
            {
                return "n/a";
            }
        }

        /// <summary>
        /// Gets the path of the assembly where the calling class is from.
        /// </summary>
        /// <returns>The path of the assembly of the calling class</returns>
        public static string? GetCallingAssemblyPath()
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
            StackFrame stackFrame = new(2, false);
            Assembly callingAssembly = Assembly.GetAssembly(stackFrame.GetMethod().DeclaringType);
            string? assemblyPath = callingAssembly?.Location;
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

        /// <summary>
        /// Announces a message in the game with the specified text and color.
        /// </summary>
        /// <param name="text">The message to announce.</param>
        /// <param name="colorR">The red component of the color.</param>
        /// <param name="colorG">The green component of the color.</param>
        /// <param name="colorB">The blue component of the color.</param>
        /// <param name="gameTypeRef">A reference type related to the game context.</param>
        public static void Announce(string text, int colorR, int colorG, int colorB, Type gameTypeRef) {
            var color = new UnityEngine.Color(colorR, colorG, colorB);
            Announcer.instance.Announce(text, color, true);
        }

        /// <summary>
        /// Returns a relative path from the given absolute path.
        /// For example, if the absolute path is C:\Foo\Bar\Baz.txt and the current directory is C:\Foo,
        /// the relative path returned would be "Bar\Baz.txt".
        /// </summary>
        /// <param name="path">The absolute path to get the relative path from</param>
        /// <returns>The relative path</returns>
        public static string GetRelativePath(string path) {
            return path.Substring(Directory.GetCurrentDirectory().Length + 1);
        }

        /// <summary>
        /// Replaces all backslashes with forward slashes and all spaces with underscores in the given path.
        /// </summary>
        /// <param name="path">The path to sanitize</param>
        /// <returns>The sanitized path</returns>
        public static string GetPathSafe(string path) {
            return path.Replace('\\', '/').Replace(" ", "_");
        }

        /// <summary>
        /// Deletes the specified directory and all its contents.
        /// </summary>
        /// <param name="path">The path of the directory to delete</param>
        public static void DeleteDirectory(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Copies the contents of a source directory to a destination directory.
        /// If the destination directory does not exist, it is created.
        /// All files in the source directory are copied to the destination directory.
        /// All subdirectories of the source directory are copied recursively.
        /// </summary>
        /// <param name="source">The source directory to copy from</param>
        /// <param name="destination">The destination directory to copy to</param>
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

        /// <summary>
        /// Gets the path of the config folder.
        /// </summary>
        /// <returns>The path of the config folder</returns>
        public static string GetConfigPath() {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Config");
        }

        /// <summary>
        /// Gets the path of a config file.
        /// </summary>
        /// <param name="fileName">The name of the config file.</param>
        /// <returns>The path of the config file.</returns>
        public static string GetConfigFile(string fileName) {
            return Path.Combine(GetConfigPath(), fileName);
        }
    }
}

