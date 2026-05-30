using System;
using System.Threading.Tasks;
using GameSaveSystem.Storage;
using GameSaveSystem.Serialization;
using GameSaveSystem.Encryption;
using GameSaveSystem.Tools;
using UnityEngine;

namespace GameSaveSystem.Core
{
    public class SaveManager
    {
        private SaveManagerSettings settings;

        private static readonly Lazy<SaveManager> lazy = new(() => new SaveManager());
        public static SaveManager Instance => lazy.Value;


        private SaveManager()
        {
            InitializeSystem();
        }

        private void InitializeSystem()
        {
            settings = Resources.Load<SaveManagerSettings>("SaveManagerSettings");
            //Create one
            if (settings == null)
                settings = ScriptableObject.CreateInstance<SaveManagerSettings>();
        }


        public async Task<SaveResult> Save<T>(string filename, T saveObject, SaveStorage storage = null, SaveSerializer serializer = null, SaveEncrypter encrypter = null)
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
                //optional encrypter
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

        public async Task<SaveResult> Save<T>(string filename, T saveObject)
        {
            return await Save<T>(filename, saveObject, settings.Storage, settings.Serializer, settings.Encrypter);
        }

        public async Task<T> Load<T>(string filename, SaveStorage storage = null, SaveSerializer serializer = null, SaveEncrypter encrypter = null)
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
                //optional encrypter
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

        public async Task<T> Load<T>(string filename)
        {
            return await Load<T>(filename, settings.Storage, settings.Serializer, settings.Encrypter);
        }

        public async Task<DeleteResult> Delete(string filename, SaveStorage storage = null)
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
