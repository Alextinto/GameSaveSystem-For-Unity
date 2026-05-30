using GameSaveSystem.Encryption;
using GameSaveSystem.Serialization;
using GameSaveSystem.Storage;
using UnityEngine;

namespace GameSaveSystem.Core
{
    [CreateAssetMenu(menuName = "Game Save System/Save Manager Settings")]
    public class SaveManagerSettings : ScriptableObject
    {
        [SerializeField] private SaveSerializer serializer;
        public SaveSerializer Serializer => serializer;
        [SerializeField] private SaveEncrypter encrypter;
        public SaveEncrypter Encrypter => encrypter;
        [SerializeField] private SaveStorage storage;
        public SaveStorage Storage => storage;
    }
}
