using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

class Updater
{
    /// <summary>
    /// The entry point of the updater executable.
    /// 
    /// This program is launched by the Updater class in the main mod loader assembly.
    /// It takes one or more arguments, the last of which should be the process ID of the SpiderHeck process
    /// that launched the updater.
    /// 
    /// The program will kill the SpiderHeck process, extract the contents of the "SilkUpdate.zip" file to the
    /// parent directory of the directory that it is running in, and then exit.
    /// </summary>
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting update process...");
            Console.WriteLine($"Arguments: {string.Join(", ", args)}");

            // Kill the SpiderHeck process if it exists
            if (args.Length > 0 && int.TryParse(args[args.Length - 1], out int pid) && IsProcessRunning(pid))
            {
                Console.WriteLine($"Killing SpiderHeck process with ID {pid}...");
                Process.GetProcessById(pid).Kill();
            }

            // Extract and install the update
            Console.WriteLine("Extracting and installing update...");
            using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SilkUpdate.zip")))
            {
                archive.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory + "../", overwriteFiles: true);
            }

            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SilkUpdate.zip"));
            Console.WriteLine("Update package fully installed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            // Wait for 5 seconds before exiting
            Task.Delay(5000).Wait();
        }
    }

    /// <summary>
    /// Checks if a process with the specified process ID is currently running.
    /// </summary>
    /// <param name="pid">The process ID to check.</param>
    /// <returns>True if the process is running, otherwise false.</returns>
    static bool IsProcessRunning(int pid)
    {
        try
        {
            Process.GetProcessById(pid);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}

