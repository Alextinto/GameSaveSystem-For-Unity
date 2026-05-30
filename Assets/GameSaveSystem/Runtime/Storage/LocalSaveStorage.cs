using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSaveSystem.Storage
{
    [CreateAssetMenu(menuName = "Game Save System/Storage/Local Save Storage")]
    public class LocalStorage : SaveStorage
    {
        [SerializeField] private string folderName = "Saves";
        //Examples .save .world .data, etc
        [SerializeField] private string defaultExtension = "";

        public override async Task Save(string savename, string data)
        {
            string path = GetPath(savename);
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(path, data);
        }

        public override async Task<string> Load(string savename)
        {
            string path = GetPath(savename);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"[LocalSaveStorage] Save not found: {path}");
                return null;
            }

            return await File.ReadAllTextAsync(path);
        }

        public override async Task DeleteSave(string savename)
        {
            string path = GetPath(savename);

            if (File.Exists(path))
            {
                File.Delete(path);
                await Task.CompletedTask;
            }
            else
            {
                Debug.LogWarning($"[LocalSaveStorage] Cannot delete, save not found: {path}");
                await Task.CompletedTask;
            }
        }

        private string GetPath(string savename)
        {
            string extension = string.IsNullOrEmpty(defaultExtension) ? "" : $".{defaultExtension}";
            string relativePath = Path.Combine(folderName, $"{savename}{extension}");
            return Path.Combine(Application.persistentDataPath, relativePath);
        }
    }
}
