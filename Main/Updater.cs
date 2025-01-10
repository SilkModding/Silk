using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO.Compression;

namespace Silk
{
    public static class Updater
    {
        private const string LatestVersionUrl = "https://raw.githubusercontent.com/SilkModding/Silk/master/version";
        private const string DownloadUrl = "https://github.com/SilkModding/Silk/releases/download/v{0}/Silk-v{0}.zip"; 
        private const string TempDownloadPath = "SilkUpdate.zip";

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        // TODO: This does not work. winhttp.dll/doorstop does not cooperate.
        // It *does* restart the game, but Silk is not started along with it.
        public static async Task RestartAfterUpdate()
        {
            Logger.LogInfo("RestartAfterUpdate called");
            await Task.Delay(5000);

            string path = AppDomain.CurrentDomain.BaseDirectory + "/Silk/SilkUpdateRestarter.exe";
            
            Logger.LogInfo($"path: {path}");
            string currentPID = Process.GetCurrentProcess().Id.ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.Arguments = currentPID;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            FreeConsole();
            Process.Start(startInfo);

            Logger.LogWarning("adfadsfas");
            Application.Quit(0);
        }

        public static void runUpdateExtractor()
        {
            Logger.LogInfo("runUpdateExtractor called");
            string path = AppDomain.CurrentDomain.BaseDirectory + "/SilkUpdateRestarter.exe";

            Logger.LogInfo($"path: {path}");
            string currentPID = Process.GetCurrentProcess().Id.ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.Arguments = currentPID;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            //FreeConsole();
            Process.Start(startInfo);
            Application.Quit(0);
        }

        // updater entrypoint
        public static async void CheckForUpdates()
        {
            Logger.LogInfo("Checking for updates...");
            var latestVersion = await GetLatestVersion();
            Logger.LogInfo($"Latest version: {latestVersion}");
            if (VersionHandler.IsOlderThan(latestVersion))
            {
                Logger.LogInfo("A new version of Silk is available!");
                var downloadUrl = string.Format(DownloadUrl, latestVersion);
                Logger.LogInfo($"Download: {downloadUrl}");

                Announcer.TwoOptionsPopup(
                    "A new Silk version is available! Do you want to update?\nNote: you will need to restart the game to apply the update",
                    "Yes", "No",
                    async () => {
                        try
                        {
                            Announcer.InformationPopup($"Downloading and installing Silk version {latestVersion}...\nDo not close your game...");
                            DownloadAndInstallUpdate(downloadUrl);
                            Announcer.InformationPopup($"Download Finished! You must restart the game manually for this update to apply\n(We know it's not ideal, but we are working on something to do this automatically.)");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("An error occurred during the update process.");
                            Logger.LogError(ex.Message);
                            Announcer.InformationPopup("An error occurred while downloading the update. Please try again later.");
                        }
                    }, () => { },
                null);
            }
            else
            {
                Logger.LogInfo("No updates available.");
            }
        }

        private static async Task<string> GetLatestVersion()
        {
            Logger.LogInfo("Checking for latest version...");
            using (WebClient client = new WebClient())
            {
                var resp = client.DownloadString(LatestVersionUrl);
                return resp.Trim();
            }
        }

        private static void DownloadAndInstallUpdate(string downloadUrl)
        {
            Logger.LogInfo("Starting download...");
            Logger.Log(TempDownloadPath);
            Logger.Log(downloadUrl);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(downloadUrl), TempDownloadPath);
            }
            Logger.LogInfo($"downloaded to {TempDownloadPath}");

            runUpdateExtractor();

            //ExtractAndInstallUpdate(TempDownloadPath);
        }

        private static void ExtractAndInstallUpdate(string zipPath)
        {
            Logger.LogInfo("Installing update...");

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempPath);

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(tempPath, entry.FullName);

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    // Overwrite files if they already exist
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }

                    entry.ExtractToFile(destinationPath, overwrite: true);
                }
            }

            // Copy the files from the temp folder to the actual folder
            foreach (var file in Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories))
            {
                string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.Substring(tempPath.Length));
                string destinationDir = Path.GetDirectoryName(destinationPath);
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }
                File.Copy(file, destinationPath, overwrite: true);
            }

            Directory.Delete(tempPath, true);
            File.Delete(zipPath);
            Logger.LogInfo("Update installed successfully.");
        }

    }
}
