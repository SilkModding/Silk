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
        public string ModGameVersion { get; }
        /// <summary>
        /// The id of the mod.
        /// </summary>
        public string ModId { get; }

        /// <summary>
        /// The constructor to pass the mod name, authors, version, game version, and id.
        /// </summary>
        /// <param name="modName">The name of the mod.</param>
        /// <param name="modAuthors">The authors of the mod.</param>
        /// <param name="modVersion">The version of the mod.</param>
        /// <param name="modGameVersion">The version of the game that the mod supports.</param>
        /// <param name="modId">The id of the mod. Defaults to the mod name.</param>
        public SilkModAttribute(string modName, string[] modAuthors, string modVersion, string modGameVersion, string modId = null)
        {
            ModName = modName;
            ModAuthors = modAuthors;
            ModVersion = modVersion;
            ModGameVersion = modGameVersion;
            ModId = modId ?? modName;
        }
    }
}

