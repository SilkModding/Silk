
using System;
using System.Diagnostics;
using System.IO.Compression;

class SilkUpdateRestarter
{
    static bool IsProcessRunning(int pid)
    {
        try
        {
            Process process = Process.GetProcessById(pid);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
    
    static void Main(string[] args)
    {

        Console.WriteLine("eep");
        try
        {
            //string filePath = AppDomain.CurrentDomain.BaseDirectory+@"\SpiderHeckApp.exe";

            try
            {
                Process process = Process.GetProcessById(int.Parse(args[args.Length - 1]));

                process.Kill();
            } catch (Exception e)
            {
                Console.WriteLine("process didn't exist, proceeding anyways");
            }

            Console.WriteLine("Extracting and installing update...");

            //Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "/SilkUpdate.zip", true );

            using (ZipArchive archive = ZipFile.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "/SilkUpdate.zip"))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, entry.FullName);

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    // Overwrite files if they already exist
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                }
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory+"/SilkUpdate.zip", AppDomain.CurrentDomain.BaseDirectory);

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/SilkUpdate.zip");
            Console.WriteLine("Update package fully installed!");
            
            Task.Delay(5000);
            /*
            Task.Delay(2000);
            Console.WriteLine("Restarting game");
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(filePath))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(filePath);
                    startInfo.UseShellExecute = true;
                    startInfo.CreateNoWindow = false;
                    Process.Start(startInfo);

                    Console.WriteLine($"Successfully started the file: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting the file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("The specified file does not exist.");
            }*/
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }

}