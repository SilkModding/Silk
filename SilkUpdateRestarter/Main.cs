using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

class SilkUpdateRestarter
{
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

