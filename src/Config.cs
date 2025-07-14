using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;

namespace Silk
{
    public static class Config
    {
        private static Dictionary<string, object> _config = new Dictionary<string, object>();
        private static Dictionary<string, object> _defaultConfig = new Dictionary<string, object>();

        public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Silk", "Config");
        public static string ConfigFile => Path.Combine(ConfigPath, "silk.yaml");

        public static void LoadConfig(Dictionary<string, object> defaultConfig)
        {
            _defaultConfig = defaultConfig;
            Logger.LogInfo("Loading config...");

            if (!File.Exists(ConfigFile))
            {
                Logger.LogInfo("Config file not found, creating it with default values.");
                _config = _defaultConfig;
                SaveConfig();
            }
            else
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var userConfig = deserializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(ConfigFile)) ?? new Dictionary<string, object>();
                _config = MergeConfigs(_defaultConfig, userConfig);
                SaveConfig(); // Save to add new default values to the user's file
            }
        }

        private static Dictionary<string, object> MergeConfigs(Dictionary<string, object> defaultConfig, Dictionary<string, object> userConfig)
        {
            var mergedConfig = new Dictionary<string, object>(defaultConfig);

            foreach (var item in userConfig)
            {
                if (mergedConfig.TryGetValue(item.Key, out var defaultValue) && defaultValue is Dictionary<string, object> defaultDict && item.Value is Dictionary<string, object> userDict)
                {
                    mergedConfig[item.Key] = MergeConfigs(defaultDict, userDict);
                }
                else
                {
                    mergedConfig[item.Key] = item.Value;
                }
            }

            return mergedConfig;
        }

        public static T GetConfigValue<T>(string key, T defaultValue = default)
        {
            try
            {
                var keys = key.Split('.');
                object currentNode = _config;

                foreach (var k in keys)
                {
                    if (currentNode is Dictionary<string, object> dict && dict.TryGetValue(k, out var value))
                    {
                        currentNode = value;
                    }
                    else
                    {
                        return defaultValue;
                    }
                }

                return (T)Convert.ChangeType(currentNode, typeof(T));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error getting config value for key '{key}': {ex.Message}");
                return defaultValue;
            }
        }

        public static void SetConfigValue(string key, object value)
        {
            var keys = key.Split('.');
            Dictionary<string, object> currentNode = _config;

            for (int i = 0; i < keys.Length - 1; i++)
            {
                var k = keys[i];
                if (!currentNode.TryGetValue(k, out var nextNode) || !(nextNode is Dictionary<string, object>))
                {
                    nextNode = new Dictionary<string, object>();
                    currentNode[k] = nextNode;
                }
                currentNode = (Dictionary<string, object>)nextNode;
            }

            currentNode[keys.Last()] = value;
            SaveConfig();
        }

        public static void SaveConfig()
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(_config);
            File.WriteAllText(ConfigFile, yaml);
        }


    }
}