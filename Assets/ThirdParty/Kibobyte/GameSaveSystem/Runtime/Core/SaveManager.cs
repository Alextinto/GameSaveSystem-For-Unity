using System;
using System.Threading.Tasks;
using GameSaveSystem.Serialization.Objects;
using GameSaveSystem.Encryption;
using GameSaveSystem.Tools;
using UnityEngine;

namespace GameSaveSystem.Core
{
    public static class SaveManager
    {
        private static SaveManagerSettings settings;

        public static void Initialize()
        {
            Debug.Log("[SaveManager] Initializing...");
            settings = Resources.Load<SaveManagerSettings>("SaveManagerSettings");
            if (settings == null)
                settings = ScriptableObject.CreateInstance<SaveManagerSettings>();
            Debug.Log("[SaveManager] Initialized");
        }

        public static async Task<SaveResult> Save<T>(string filename, T saveObject, Storage.Storage storage = null, Serializer serializer = null, Encrypter encrypter = null)
        {
            if (storage == null)
                storage = settings.Storage;
            if (serializer == null)
                serializer = settings.Serializer;
            if (encrypter == null)
                encrypter = settings.Encrypter;

            try
            {
                string cleanedFilename = FilenameCleaner.Clean(filename);
                string save = serializer.Serialize<T>(saveObject);
                if (encrypter != null)
                    save = encrypter.Encrypt(save);
                await storage.Save(cleanedFilename, save);
                return SaveResult.Success;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Unexpected error on Save.\nFilename:{filename}\nError{e}");
                return SaveResult.UnknownError;
            }
        }

        public static async Task<SaveResult> Save<T>(string filename, T saveObject)
        {
            return await Save<T>(filename, saveObject, settings.Storage, settings.Serializer, settings.Encrypter);
        }

        public static async Task<T> Load<T>(string filename, Storage.Storage storage = null, Serializer serializer = null, Encrypter encrypter = null)
        {
            if (storage == null)
                storage = settings.Storage;
            if (serializer == null)
                serializer = settings.Serializer;
            if (encrypter == null)
                encrypter = settings.Encrypter;

            try
            {
                string cleanedFilename = FilenameCleaner.Clean(filename);
                string save = await storage.Load(cleanedFilename);
                if (encrypter != null)
                    save = encrypter.Decrypt(save);
                return serializer.Deserialize<T>(save);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Unexpected error on Load.\nFilename:{filename}\nError{e}");
                return default(T);
            }
        }

        public static async Task<T> Load<T>(string filename)
        {
            return await Load<T>(filename, settings.Storage, settings.Serializer, settings.Encrypter);
        }

        public static async Task<DeleteResult> Delete(string filename, Storage.Storage storage = null)
        {
            if (storage == null)
                storage = settings.Storage;

            try
            {
                string cleanedFilename = FilenameCleaner.Clean(filename);
                await storage.DeleteSave(cleanedFilename);
                return DeleteResult.Success;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Unexpected error on Delete.\nFilename:{filename}\nError{e.Message}");
                return DeleteResult.UnknownError;
            }
        }
    }
}