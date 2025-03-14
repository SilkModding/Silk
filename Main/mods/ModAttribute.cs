using System;

namespace Silk {
    /// <summary>
    /// The attribute used to mark a mod class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SilkModAttribute : Attribute
    {
        /// <summary>
        /// The name of the mod.
        /// </summary>
        public string ModName { get; }
        /// <summary>
        /// The author of the mod.
        /// </summary>
        public string[] ModAuthors { get; }
        /// <summary>
        /// The version of the mod.
        /// </summary>
        public string ModVersion { get; }
        /// <summary>
        /// The version of the game that the mod supports.
        /// </summary>
        public string ModSilkVersion { get; }
        /// <summary>
        /// The id of the mod.
        /// </summary>
        public string ModId { get; }
        /// <summary>
        /// The networking type of the mod.
        /// </summary>
        public NetworkingType ModNetworkingType { get; }

        /// <summary>
        /// The constructor to pass the mod name, authors, version, game version, id, and networking type.
        /// </summary>
        /// <param name="modName">The name of the mod.</param>
        /// <param name="modAuthors">The authors of the mod.</param>
        /// <param name="modVersion">The version of the mod.</param>
        /// <param name="modSilkVersion">The version of the game that the mod supports.</param>
        /// <param name="modId">The id of the mod. Defaults to the mod name.</param>
        /// <param name="modNetworkingType">The networking type of the mod.</param>
        public SilkModAttribute(string modName, string[] modAuthors, string modVersion, string modSilkVersion, string modId, int modNetworkingType)
        {
            ModName = modName;
            ModAuthors = modAuthors;
            ModVersion = modVersion;
            ModSilkVersion = modSilkVersion;
            ModId = modId ?? modName;
            ModNetworkingType = (NetworkingType)modNetworkingType;
        }
    }
}