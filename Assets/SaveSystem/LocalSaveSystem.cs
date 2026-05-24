using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class LocalSaveSystem : ISaveSystem 
{
    const string SaveExtension = ".lv";

    private string GetPath(string savename) => 
        Path.Combine(Application.persistentDataPath, $"{savename}{SaveExtension}");

    public async Task<bool> DeleteSave(string savename)
    {
        string path = GetPath(savename);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[LocalSaveSystem] No save file found for '{savename}' to delete.");
            return false;
        }
        await Task.Run(() => File.Delete(path));
        Debug.Log($"[LocalSaveSystem] Save file '{savename}' deleted.");
        return true;
    }

    public async Task<T> Load<T>(string savename) where T : SaveData
    {
        string path = GetPath(savename);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[LocalSaveSystem] No save file found for '{savename}'.");
            return null;
        }
        string json = await Task.Run(() => File.ReadAllText(path));
        T data = JsonConvert.DeserializeObject<T>(json);
        Debug.Log($"[LocalSaveSystem] Data loaded from '{savename}'.");
        return data;
    }

    public async Task<bool> Save<T>(string savename, T data) where T : SaveData
    {
        string path = GetPath(savename);
        string json = JsonConvert.SerializeObject(data, typeof(T), Formatting.Indented);
        await Task.Run(() => File.WriteAllText(path, json));
        Debug.Log($"[LocalSaveSystem] Data saved to '{savename}'.");
        return true;
    }
}