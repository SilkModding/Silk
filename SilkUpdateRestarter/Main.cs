
using System;
using System.Diagnostics;

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

        Console.WriteLine("This program is a WIP, it does not work in its current state as winhttp.dll/doorstop does not seem to cooperate.");
        return;

        Console.WriteLine("eep");
        try
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory+@"..\SpiderHeckApp.exe";

            try
            {
                Process process = Process.GetProcessById(int.Parse(args[args.Length - 1]));

                process.Kill();
            } catch (Exception e)
            {
                Console.WriteLine("process didn't exist, proceeding anyways");
            }

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
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }

}