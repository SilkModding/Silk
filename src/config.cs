using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;

namespace Silk
{
    public class Config
    {
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        // Config path (uses utils)
        public static string ConfigPath => Utils.GetConfigPath();

        // Config File (uses utils)
        public static string ConfigFile => Utils.GetConfigFile("silk.yaml");

        public static void LoadConfig()
        {
            Logger.LogInfo("Loading config...");

            if (!File.Exists(ConfigFile))
            {
                Logger.LogInfo("Config file not found, creating it");
                CreateConfigFile();
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            config = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(ConfigFile));
        }

        public static void CreateConfigFile()
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }

            if (!File.Exists(ConfigFile))
            {
                File.Create(ConfigFile).Dispose();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# Silk configuration file");
            sb.AppendLine("# Add your configuration key-value pairs below");

            File.WriteAllText(ConfigFile, sb.ToString());
        }

        public static string? GetConfigValue(string key)
        {
            config.TryGetValue(key, out var value);
            return value;
        }

        public static void SetConfigValue(string key, string value)
        {
            if (config.ContainsKey(key))
            {
                config[key] = value;
                SaveConfig();
            }
            else
            {
                Logger.LogError($"Config key {key} does not exist, skipping");
            }
        }

        public static void SaveConfig()
        {
            var yaml = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build()
                .Serialize(config);

            File.WriteAllText(ConfigFile, yaml);
        }

        public static bool CheckAndAddConfig(string key, string defaultValue)
        {
            if (!config.ContainsKey(key))
            {
                config[key] = defaultValue;
                SaveConfig();
                return true;
            }

            return false;
        }
    }
}