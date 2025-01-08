
namespace Silk
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class Updater
    {
        private const string LatestVersionUrl = "https://raw.githubusercontent.com/SilkModding/Silk/master/version";
        private const string DownloadUrl = "https://github.com/SilkModding/Silk/releases/download/{0}/Silk.zip";
        private const string TempDownloadPath = "SilkUpdate.zip";

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        // does not work. winhttp.dll/doorstop does not cooperate
        // it *does* restart the game, but Silk is not started along with it.
        public static async Task RestartAfterUpdate()
        {
            await Task.Delay(5000);

            string path = AppDomain.CurrentDomain.BaseDirectory + "/Silk/SilkUpdateRestarter.exe";
            /*
            // Process.Start(procPath);
            // honestly probably not needed...
            Process.Start(new ProcessStartInfo
            {
                FileName = procPath,
                UseShellExecute = true, // Keeps user permissions
            });
            */

            Console.WriteLine(path);
            string currentPID = Process.GetCurrentProcess().Id.ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.Arguments = currentPID;
            startInfo.UseShellExecute = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = false;
            FreeConsole();
            Process.Start(startInfo);

            Logger.LogWarning("adfadsfas");
            //Environment.Exit(0);

            Application.Quit(0);

        }

        // updater entrypoint
        public static async Task CheckForUpdates()
        {
            var latestVersion = await GetLatestVersion();
            if (VersionHandler.IsOlderThan(latestVersion))
            {
                //var changelog = await GetChangelog();
                var downloadUrl = string.Format(DownloadUrl, latestVersion);

                Logger.LogInfo($"A new version of Silk is available: {latestVersion}");
                //Logger.LogInfo($"Changelog: {changelog}");
                Logger.LogInfo($"Download: {downloadUrl}");

                Announcer.TwoOptionsPopup(
                    "A new Silk version is available! Do you want to update?\nNote: you will need to restart the game to apply the update",
                    "Yes", "No",
                    async () => {
                        Announcer.InformationPopup($"Downloading and installing Silk version {latestVersion}...\nDo not close your game...");
                        //Task.Delay(3000);
                        await DownloadAndInstallUpdate(downloadUrl);
                        Announcer.InformationPopup($"Download Finished! You must restart the game manually for this update to apply\n(We know its not ideal, but we are working on something to do this automatically.)");
                        //RestartAfterUpdate();
                    }, () => { },
                null);
            }
        }

        private static async Task<string> GetLatestVersion()
        {
            using (WebClient client = new WebClient())
            {
                var resp = client.DownloadString("https://raw.githubusercontent.com/SilkModding/Silk/refs/heads/main/version");
                return resp.Trim();
            }
        }
        /*
        private static async Task<string> GetChangelog()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"https://raw.githubusercontent.com/MyUser/MyRepo/master/Changelog.txt");
                return response.Trim();
            }
        }
        */
        private static async Task DownloadAndInstallUpdate(string downloadUrl)
        {

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(downloadUrl), AppDomain.CurrentDomain.BaseDirectory);
            }


            // Extract and install the update
            ExtractAndInstallUpdate(TempDownloadPath);
        }

        private static void ExtractAndInstallUpdate(string zipPath)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, AppDomain.CurrentDomain.BaseDirectory);
            File.Delete(zipPath);
            Logger.LogInfo("Update installed successfully.");
        }
    }
}

