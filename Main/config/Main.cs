using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Silk
{
    public class Config
    {
        private static Dictionary<string, string> config = new Dictionary<string, string>();
        
        // Config path (uses utils)
        public static string ConfigPath => Utils.GetConfigPath();

        // Config File (uses utils)
        public static string ConfigFile => Utils.GetConfigFile("silk.cfg");

        public static void LoadConfig()
        {
            // Log starting
            Logger.LogInfo("Loading config...");

            // Log Paths
            Logger.LogInfo($"Config file: {ConfigFile}");
            Logger.LogInfo($"Config path: {ConfigPath}");
    
            // Check if config file exists
            if (!File.Exists(ConfigFile))
            {   
                // If not, create it
                Logger.LogInfo("Config file not found, creating it");
                CreateConfigFile(ConfigFile, ConfigPath);
            }

            // Load config
            string[] lines = File.ReadAllLines(ConfigFile);

            // Parse config
            foreach (string line in lines)
            {
                // Skip comments
                if (line.StartsWith("#"))
                {
                    continue;
                }
                
                Logger.LogInfo($"Parsing config line: {line}");
                string[] split = line.Split('=');

                // Check if split is valid
                if (split.Length != 2)
                {
                    Logger.LogError($"Invalid config line: {line}");
                    continue;
                }

                // Get key and value
                string key = split[0].Trim();
                string value = split[1].Trim();

                // Check if key already exists
                if (config.ContainsKey(key))
                {
                    Logger.LogError($"Config key {key} already exists, skipping");
                    continue;
                }

                // Add key and value to config
                config.Add(key, value);
            }
        }

        public static void CreateConfigFile(string ConfigFile, string ConfigPath)
        {
            // Make sure the directory exists
            string configDir = Path.GetDirectoryName(ConfigPath);
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            // Make sure the file exists
            if (!File.Exists(ConfigFile))
            {
                File.Create(ConfigFile).Dispose();
            }

            StringBuilder sb = new StringBuilder();

            FieldInfo[] fields = typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                string defaultValue = field.GetValue(null).ToString();

                sb.AppendLine($"{key}={defaultValue}");
            }

            File.WriteAllText(ConfigFile, sb.ToString());
        }

        public static string GetConfigValue(string key)
        {
            if (config.ContainsKey(key))
            {
                return config[key];
            }

            return null;
        }

        public static void SetConfigValue(string key, string value)
        {
            if (config.ContainsKey(key))
            {
                config[key] = value;
            }
            else
            {
                Logger.LogError($"Config key {key} does not exist, skipping");
            }
        }

        public static void SaveConfig()
        {
            string configPath = ConfigFile;

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in config)
            {
                sb.AppendLine($"{pair.Key}={pair.Value}");
            }

            File.WriteAllText(configPath, sb.ToString());
        }
    }
}


