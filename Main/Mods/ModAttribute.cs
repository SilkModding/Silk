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
        public string ModAuthor { get; }
        /// <summary>
        /// The entry point method of the mod.
        /// </summary>
        public string ModEntryPoint { get; }

        /// <summary>
        /// The constructor to pass the mod name, author, and entry point method.
        /// </summary>
        /// <param name="modName">The name of the mod.</param>
        /// <param name="modAuthor">The author of the mod.</param>
        /// <param name="modEntryPoint">The entry point method of the mod. Defaults to "Initialize".</param>
        public SilkModAttribute(string modName, string modAuthor, string modEntryPoint = "Initialize")
        {
            ModName = modName;
            ModAuthor = modAuthor;
            ModEntryPoint = modEntryPoint;
        }
    }
}
