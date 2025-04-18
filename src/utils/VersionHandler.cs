using System;

namespace Silk
{
    public static class VersionHandler
    {   
        private static readonly Version _version = typeof(VersionHandler).Assembly.GetName().Version;

        /// <summary>
        /// Gets the version of the loader.
        /// </summary>
        /// <returns>The version of the loader</returns>
        public static string GetVersion()
        {
            return _version.ToString();
        }

        /// <summary>
        /// Checks if the current version is newer than the specified version.
        /// </summary>
        /// <param name="version">The version to compare against.</param>
        /// <returns>True if the current version is newer, otherwise false.</returns>
        public static bool IsNewerThan(string version)
        {
            if (Version.TryParse(version, out Version otherVersion))
            {
                return _version.CompareTo(otherVersion) > 0;
            }
            return false;
        }

        /// <summary>
        /// Checks if the current version is older than the specified version.
        /// </summary>
        /// <param name="version">The version to compare against.</param>
        /// <returns>True if the current version is older, otherwise false.</returns>
        public static bool IsOlderThan(string version)
        {
            if (Version.TryParse(version, out Version otherVersion))
            {
                return _version.CompareTo(otherVersion) < 0;
            }
            return false;
        }

        /// <summary>
        /// Checks if the current version is the same as the specified version.
        /// </summary>
        /// <param name="version">The version to compare against.</param>
        /// <returns>True if the current version is the same, otherwise false.</returns>
        public static bool IsSameAs(string version)
        {
            if (Version.TryParse(version, out Version otherVersion))
            {
                return _version.CompareTo(otherVersion) == 0;
            }
            return false;
        }
    }
}

