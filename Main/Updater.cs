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
            Logger.LogInfo("Restarting game after update...");
            await Task.Delay(5000);

            var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "SilkUpdateRestarter.exe");
            var startInfo = new ProcessStartInfo(exePath, Process.GetCurrentProcess().Id.ToString())
            {
                UseShellExecute = true,
                CreateNoWindow = false
            };

            FreeConsole();
            Process.Start(startInfo);
            Application.Quit(0);
        }

        public static void RunUpdateExtractor()
        {
            var updateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Updater");
            var updaterDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater");

            // Move the update extractor from /Silk/Updates/* to /Updater/
            foreach (var file in Directory.GetFiles(updateDir))
            {
                var newFile = Path.Combine(updaterDir, Path.GetFileName(file));
                File.Move(file, newFile);
            }
            Directory.Delete(updateDir);

            var path = Path.Combine(updaterDir, "SilkUpdateRestarter.exe");
            var currentProcessId = Process.GetCurrentProcess().Id;

            var startInfo = new ProcessStartInfo(path, currentProcessId.ToString())
            {
                UseShellExecute = true,
                CreateNoWindow = false
            };

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
                    () =>
                    {
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
                    }, () => {
                        Logger.LogInfo("Update declined, continuing with current version.");
                    },
                null);
            }
            else
            {
                Logger.LogInfo("No updates available.");
            }
        }

        private static Task<string> GetLatestVersion()
        {
            Logger.LogInfo("Checking for latest version...");
            using WebClient client = new();
            var resp = client.DownloadString(LatestVersionUrl);
            return Task.FromResult(resp.Trim());
        }

        private static void DownloadAndInstallUpdate(string downloadUrl)
        {
            Logger.LogInfo("Starting download...");
            Logger.Log(TempDownloadPath);
            Logger.Log(downloadUrl);
            using (WebClient client = new())
            {
                client.DownloadFile(new Uri(downloadUrl), TempDownloadPath);
            }
            Logger.LogInfo($"downloaded to {TempDownloadPath}");

            RunUpdateExtractor();
        }
    }
}
