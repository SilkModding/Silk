namespace Silk
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class Updater
    {
        private const string LatestVersionUrl = "https://raw.githubusercontent.com/SilkModding/Silk/master/version";
        private const string DownloadUrl = "https://github.com/SilkModding/Silk/releases/download/{0}/Silk.zip";
        private const string TempDownloadPath = "SilkUpdate.zip";

        public static async Task CheckForUpdates()
        {
            var latestVersion = await GetLatestVersion();
            if (VersionHandler.IsOlderThan(latestVersion))
            {
                var changelog = await GetChangelog();
                var downloadUrl = string.Format(DownloadUrl, latestVersion);

                Logger.LogInfo($"A new version of Silk is available: {latestVersion}");
                Logger.LogInfo($"Changelog: {changelog}");
                Logger.LogInfo($"Download: {downloadUrl}");

                Announcer.TwoOptionsPopup(
                    "A new Silk version is available! Do you want to update?\nNote: you will need to restart the game to apply the update",
                    "Yes", "No",
                    () => DownloadAndInstallUpdate(downloadUrl), () => { },
                null);
            }
        }

        private static async Task<string> GetLatestVersion()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(LatestVersionUrl);
                return response.Trim();
            }
        }

        private static async Task<string> GetChangelog()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"https://raw.githubusercontent.com/MyUser/MyRepo/master/Changelog.txt");
                return response.Trim();
            }
        }

        private static async Task DownloadAndInstallUpdate(string downloadUrl)
        {
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(downloadUrl))
            using (var fileStream = new FileStream(TempDownloadPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await response.Content.CopyToAsync(fileStream);
            }

            // Extract and install the update
            ExtractAndInstallUpdate(TempDownloadPath);
        }

        private static void ExtractAndInstallUpdate(string zipPath)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, AppDomain.CurrentDomain.BaseDirectory, true);
            File.Delete(zipPath);
            Logger.LogInfo("Update installed successfully.");
        }
    }
}

