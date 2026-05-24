using UnityEngine;
using System;
using TMPro;

public class SaveController : MonoBehaviour
{
    [SerializeField] private TMP_Text saveFileName = null;

    public async void Save()
    {
        FooSaveData saveData = new FooSaveData(5, 100.0f, "PlayerOne");
        bool success = await SaveManager.Instance.Save("testSave", saveData);
        if (!success)
            Debug.LogError("[SaveController] Failed to save data.");
    }

    public async void Load()
    {
        FooSaveData loadedData = await SaveManager.Instance.Load<FooSaveData>("testSave");
        if (loadedData != null)
            Debug.Log($"[SaveController] Loaded: level={loadedData.level}, health={loadedData.health}, name={loadedData.playerName}");
        else
            Debug.LogWarning("[SaveController] No save data found.");
    }
}


public class FooSaveData : SaveData
{
    public int level;
    public float health;
    public string playerName;

    public FooSaveData(int level, float health, string playerName)
    {
        this.level = level;
        this.health = health;
        this.playerName = playerName;
    }
}