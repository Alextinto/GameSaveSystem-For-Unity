using System.Threading.Tasks;
using GameSaveSystem.Storage;
using GameSaveSystem.Serialization;
using GameSaveSystem.Encryption;
using UnityEngine;
using System.Collections.Generic;

namespace GameSaveSystem.Core
{
    public static class ValuesManager
    {
        private static ValuesManagerSettings settings;
        private static Dictionary<string, string> valueBuffer;
        private static Task initializationTask;

        public static Task Initialize()
        {
            if (initializationTask != null)
                return initializationTask;

            initializationTask = InitializeInternal();
            return initializationTask;
        }

        private static async Task InitializeInternal()
        {
            settings = Resources.Load<ValuesManagerSettings>("ValuesManagerSettings");
            if (settings == null)
                settings = ScriptableObject.CreateInstance<ValuesManagerSettings>();

            valueBuffer = await SaveManager.Load<Dictionary<string, string>>(
                settings.ValuesFileName, settings.Storage, settings.Serializer, settings.Encrypter);
            valueBuffer ??= new Dictionary<string, string>();
        }

        private static async Task EnsureInitializedAsync()
        {
            if (initializationTask == null)
                initializationTask = InitializeInternal();
            await initializationTask;
        }
    }
}