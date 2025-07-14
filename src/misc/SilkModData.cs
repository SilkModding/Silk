namespace Silk
{
    public class SilkModData
    {
        public string ModName { get; }
        public string[] ModAuthors { get; set; }
        public string ModVersion { get; set; }
        public string ModSilkVersion { get; set; }
        public string ModId { get; }
        public NetworkingType ModNetworkingType { get; }
        public string ModEntryPoint { get; set; }

        public SilkModData(string modName, string[] modAuthors, string modVersion, string modSilkVersion, string modId, NetworkingType modNetworkingType)
        {
            ModName = modName;
            ModAuthors = modAuthors;
            ModVersion = modVersion;
            ModSilkVersion = modSilkVersion;
            ModId = modId;
            ModNetworkingType = modNetworkingType;
        }

        public static SilkModData GetSilkModData()
        {
            return new SilkModData("Silk", new[] { "Abstractmelon" }, "1.0.0", "0.5.0", "silk", NetworkingType.None);
        }
    }
}

