using UnityEngine;

namespace GameSaveSystem.Serialization
{
    public abstract class Serializer : ScriptableObject
    {
        public abstract string Serialize<T>(T data);
        public abstract T Deserialize<T>(string serializedData);
    }
}
