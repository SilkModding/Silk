using System;

namespace Silk {
    [AttributeUsage(AttributeTargets.Class)]
    public class SilkModAttribute : Attribute
    {
        public string ModName { get; }
        public string ModAuthor { get; }
        public string ModEntryPoint { get; }

        // Constructor to pass the mod name, author, and entry point method
        public SilkModAttribute(string modName, string modAuthor, string modEntryPoint = "Initialize")
        {
            ModName = modName;
            ModAuthor = modAuthor;
            ModEntryPoint = modEntryPoint;
        }
    }
}