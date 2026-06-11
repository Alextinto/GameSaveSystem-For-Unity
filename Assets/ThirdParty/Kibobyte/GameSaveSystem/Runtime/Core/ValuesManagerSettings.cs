using GameSaveSystem.Encryption;
using GameSaveSystem.Serialization.Values;
using GameSaveSystem.Storage;
using UnityEngine;

namespace GameSaveSystem.Core
{
    [CreateAssetMenu(menuName = "Game Save System/Values Manager Settings", fileName = "ValuesManagerSettings", order = 0)]
    public class ValuesManagerSettings : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] private string valuesfileName = "values";
        public string ValuesFileName => valuesfileName;            

        [SerializeField] private bool autoSaveAfterSet = true;
        public bool AutoSaveAfterSet => autoSaveAfterSet;


        [Header("Module Settings")]
        [SerializeField] private ValuesSerializer valuesSerializer;
        public ValuesSerializer ValuesSerializer => valuesSerializer;
        [SerializeField] private Encrypter encrypter;
        public Encrypter Encrypter => encrypter;
        [SerializeField] private Storage.Storage storage;
        public Storage.Storage Storage => storage;
    }
}
