using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using GameSaveSystem.Tools;

namespace GameSaveSystem.Core
{
    public static class ValuesManager
    {
        private static ValuesManagerSettings settings;
        private static Dictionary<string, string> valueBuffer;
        private static TaskCompletionSource<bool> initTcs;

        public static async Task Initialize()
        {
            Debug.Log("[ValuesManager] Initializing...");
            initTcs = new TaskCompletionSource<bool>();
            settings = Resources.Load<ValuesManagerSettings>("ValuesManagerSettings");
            if (settings == null)
                settings = ScriptableObject.CreateInstance<ValuesManagerSettings>();
            Debug.Log("[ValuesManager] Loading buffer...");
            valueBuffer = await LoadValues();

            if(valueBuffer == null)
            {
                Debug.LogWarning("[ValuesManager] No existing buffer found, created new one.");
                valueBuffer = new Dictionary<string, string>();
            }
            initTcs.SetResult(true);
            Debug.Log("[ValuesManager] Initialized");
        }

        private static async Task<Dictionary<string, string>> LoadValues()
        {
            try
            {
                string filename = FilenameCleaner.Clean(settings.ValuesFileName);
                string data = await settings.Storage.Load(filename);
                if (string.IsNullOrEmpty(data))
                    return null;
                if (settings.Encrypter != null)
                    data = settings.Encrypter.Decrypt(data);
                return settings.ValuesSerializer.Deserialize<Dictionary<string, string>>(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"[ValuesManager] Unexpected error on Load.\nFilename:{settings.ValuesFileName}\nError{e}");
                return null;
            }
        }

        private static async Task SaveValues()
        {
            try
            {
                string filename = FilenameCleaner.Clean(settings.ValuesFileName);
                string data = settings.ValuesSerializer.Serialize<Dictionary<string, string>>(valueBuffer);
                if (settings.Encrypter != null)
                    data = settings.Encrypter.Encrypt(data);
                await settings.Storage.Save(filename, data);
            }
            catch (Exception e)
            {
                Debug.LogError($"[ValuesManager] Unexpected error on Save.\nFilename:{settings.ValuesFileName}\nError{e}");
            }
        }

        private static Task WaitForInitialization() => initTcs.Task;

        public static async Task SetInt(string key, int value)
        {
            await WaitForInitialization();
            valueBuffer[key] = value.ToString();
            if (settings.AutoSaveAfterSet)
                await SaveValues();
        }

        public static async Task<int> GetInt(string key, int defaultValue)
        {
            await WaitForInitialization();
            if (valueBuffer.TryGetValue(key, out string valueStr) && int.TryParse(valueStr, out int value))
                return value;
            return defaultValue;
        }

        public static async Task SetFloat(string key, float value)
        {
            await WaitForInitialization();
            valueBuffer[key] = value.ToString();
            if (settings.AutoSaveAfterSet)
                await SaveValues();
        }

        public static async Task<float> GetFloat(string key, float defaultValue)
        {
            await WaitForInitialization();
            if (valueBuffer.TryGetValue(key, out string valueStr) && float.TryParse(valueStr, out float value))
                return value;
            return defaultValue;
        }

        public static async Task SetDouble(string key, double value)
        {
            await WaitForInitialization();
            valueBuffer[key] = value.ToString();
            if (settings.AutoSaveAfterSet)
                await SaveValues();
        }

        public static async Task<double> GetDouble(string key, double defaultValue)
        {
            await WaitForInitialization();
            if (valueBuffer.TryGetValue(key, out string valueStr) && double.TryParse(valueStr, out double value))
                return value;
            return defaultValue;
        }

        public static async Task SetBool(string key, bool value)
        {
            await WaitForInitialization();
            valueBuffer[key] = value.ToString();
            if (settings.AutoSaveAfterSet)
                await SaveValues();
        }

        public static async Task<bool> GetBool(string key, bool defaultValue)
        {
            await WaitForInitialization();
            if (valueBuffer.TryGetValue(key, out string valueStr) && bool.TryParse(valueStr, out bool value))
                return value;
            return defaultValue;
        }

        public static async Task SetString(string key, string value)
        {
            await WaitForInitialization();
            valueBuffer[key] = value;
            if (settings.AutoSaveAfterSet)
                await SaveValues();
        }

        public static async Task<string> GetString(string key, string defaultValue)
        {
            await WaitForInitialization();
            if (valueBuffer.TryGetValue(key, out string value))
                return value;
            return defaultValue;
        }
    }
}