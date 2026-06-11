using System.Threading.Tasks;
using UnityEngine;

namespace GameSaveSystem.Storage
{
    public abstract class Storage : ScriptableObject
    {
        public abstract Task Save(string savename, string data);
        public abstract Task<string> Load(string savename);
        public abstract Task DeleteSave(string savename);
    }
}
