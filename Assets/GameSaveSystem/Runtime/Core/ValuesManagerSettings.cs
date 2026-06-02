using GameSaveSystem.Encryption;
using GameSaveSystem.Serialization;
using GameSaveSystem.Storage;
using UnityEngine;

namespace GameSaveSystem.Core
{
    [CreateAssetMenu(menuName = "Game Save System/Values Manager Settings", fileName = "ValuesManagerSettings", order = 0)]
    public class ValuesManagerSettings : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] private string valuesfileName = "values.sav";
        public string ValuesFileName => valuesfileName;            

        [SerializeField] private bool autoSaveAfterSet = true;
        public bool AutoSaveAfterSet => autoSaveAfterSet;


        [Header("Module Settings")]
        [SerializeField] private Serializer serializer;
        public Serializer Serializer => serializer;
        [SerializeField] private Encrypter encrypter;
        public Encrypter Encrypter => encrypter;
        [SerializeField] private Storage.Storage storage;
        public Storage.Storage Storage => storage;
    }
}
