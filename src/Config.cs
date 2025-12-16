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
        private static Dictionary<string, object> _config = new();
        private static Dictionary<string, object> _defaultConfig = new();
        private static readonly Dictionary<string, Dictionary<string, object>> _modConfigs = new();
        private static readonly Dictionary<string, Dictionary<string, object>> _defaultModConfigs = new();

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

                var raw = deserializer.Deserialize<object>(File.ReadAllText(ConfigFile));
                var userConfig = NormalizeYamlObjects(raw) as Dictionary<string, object> ?? new Dictionary<string, object>();
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

        public static T GetConfigValue<T>(string key, T? defaultValue = default)
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
                        return defaultValue ?? throw new InvalidOperationException($"Failed to get config value for key '{key}'");
                    }
                }

                return (T)Convert.ChangeType(currentNode, typeof(T));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error getting config value for key '{key}': {ex.Message}");
                return defaultValue ?? throw new InvalidOperationException($"Failed to get config value for key '{key}'");
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

        private static object NormalizeYamlObjects(object obj)
        {
            if (obj is IDictionary<object, object> dict)
                return dict.ToDictionary(
                    kv => kv.Key.ToString(),
                    kv => NormalizeYamlObjects(kv.Value)
                );
            if (obj is IList<object> list)
                return list.Select(NormalizeYamlObjects).ToList();
            return obj;
        }

        // Mod Config Methods

        public static string GetModConfigPath(string modId)
        {
            return Path.Combine(ConfigPath, "Mods", $"{modId}.yaml");
        }

        public static void LoadModConfig(string modId, Dictionary<string, object> defaultConfig)
        {
            if (string.IsNullOrEmpty(modId))
                throw new ArgumentException("Mod ID cannot be null or empty", nameof(modId));

            _defaultModConfigs[modId] = defaultConfig ?? new Dictionary<string, object>();
            var configPath = GetModConfigPath(modId);
            var configDir = Path.GetDirectoryName(configPath);
            
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            if (!File.Exists(configPath))
            {
                Logger.LogInfo($"Config file for {modId} not found, creating it with default values.");
                _modConfigs[modId] = new Dictionary<string, object>(_defaultModConfigs[modId]);
                SaveModConfig(modId);
            }
            else
            {
                try
                {
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    var raw = deserializer.Deserialize<object>(File.ReadAllText(configPath));
                    var userConfig = NormalizeYamlObjects(raw) as Dictionary<string, object> ?? new Dictionary<string, object>();
                    _modConfigs[modId] = MergeConfigs(new Dictionary<string, object>(_defaultModConfigs[modId]), userConfig);
                    SaveModConfig(modId); // Save to add new default values to the user's file
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to load config for {modId}: {ex.Message}");
                    _modConfigs[modId] = new Dictionary<string, object>(_defaultModConfigs[modId]);
                }
            }
        }

        public static T GetModConfigValue<T>(string modId, string key, T defaultValue = default)
        {
            try
            {
                if (!_modConfigs.TryGetValue(modId, out var modConfig))
                    return defaultValue;

                var keys = key.Split('.');
                object currentNode = modConfig;

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
                Logger.LogError($"Error getting config value for mod {modId}, key '{key}': {ex.Message}");
                return defaultValue;
            }
        }

        public static void SetModConfigValue(string modId, string key, object value)
        {
            if (!_modConfigs.TryGetValue(modId, out var modConfig))
            {
                modConfig = new Dictionary<string, object>();
                _modConfigs[modId] = modConfig;
            }

            var keys = key.Split('.');
            Dictionary<string, object> currentNode = modConfig;

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
            SaveModConfig(modId);
        }

        private static void SaveModConfig(string modId)
        {
            if (!_modConfigs.TryGetValue(modId, out var config))
                return;

            var configPath = GetModConfigPath(modId);
            var configDir = Path.GetDirectoryName(configPath);
            
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(config);
            File.WriteAllText(configPath, yaml);
        }
    }
}