using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

public class SaveManager
{
    private ISaveSystem _activeSaveSystem;
    private static readonly Lazy<SaveManager> _lazy = new Lazy<SaveManager>(() => new SaveManager());

    public static SaveManager Instance => _lazy.Value;

    static readonly string[] SlotNames = { "SaveSlot_0", "SaveSlot_1", "SaveSlot_2" };
    private int currentSaveSlotIndex = 0;
    public int CurrentSaveSlotIndex {
        get => currentSaveSlotIndex;
        set {
            if (value < 0 || value >= SlotNames.Length) {
                Debug.LogError($"[SaveManager] Invalid save slot index: {value}. Must be between 0 and {SlotNames.Length - 1}, or add a new slot.");
                return;
            }
            currentSaveSlotIndex = value;
        }
    }

    public string CurrentSlotName => SlotNames[currentSaveSlotIndex];

    private SaveManager() 
    {
        InitializeSystem();
    }

    private void InitializeSystem() {
        #if UNITY_EDITOR 
            _activeSaveSystem = new LocalSaveSystem();
        #elif UNITY_STANDALONE
            _activeSaveSystem = new LocalSaveSystem(); 
        #else
            _activeSaveSystem = new LocalSaveSystem();
        #endif
    }

    private readonly ConcurrentDictionary<string, SemaphoreSlim> slotSemaphore = new ConcurrentDictionary<string, SemaphoreSlim>();

    private SemaphoreSlim GetSaveLock(string savename)
    {
        return slotSemaphore.GetOrAdd(savename, _ => new SemaphoreSlim(1, 1));
    }

#region Save 
    public static async Task<bool> Save<T>(string savename, T data) where T : SaveData
    {
        return await Instance.Save<T>(savename, data);
    }
    public async Task<bool> Save<T>(string savename, T data) where T : SaveData
    {
        string cleanedName = FilenameCleaner.Clean(savename);
        SemaphoreSlim semaphore = GetSaveLock(cleanedName);
        await semaphore.WaitAsync();
        try 
        {
            return await _activeSaveSystem.Save<T>(cleanedName, data);
        } 
        catch (Exception ex) 
        {
            Debug.LogError($"[SaveManager] Error saving data to slot '{cleanedName}': {ex.Message}");
            return false;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public Task<bool> Save<T>(int slotIndex, T data) where T : SaveData
        => Save<T>(SlotNames[Mathf.Clamp(slotIndex, 0, SlotNames.Length - 1)], data);

    public Task<bool> Save<T>(T data) where T : SaveData
        => Save<T>(CurrentSlotName, data);
#endregion

#region Load
    public async Task<T> Load<T>(string savename) where T : SaveData
    {
        string cleanedName = FilenameCleaner.Clean(savename);
        SemaphoreSlim semaphore = GetSaveLock(cleanedName);
        await semaphore.WaitAsync();
        try 
        {
            return await _activeSaveSystem.Load<T>(cleanedName);
        } 
        catch (Exception ex) 
        {
            Debug.LogError($"[SaveManager] Error loading data from slot '{cleanedName}': {ex.Message}");
            return null;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public Task<T> Load<T>(int slotIndex) where T : SaveData
        => Load<T>(SlotNames[Mathf.Clamp(slotIndex, 0, SlotNames.Length - 1)]);
#endregion

#region Delete
    public async Task<bool> Delete(string savename)
    {
        SemaphoreSlim semaphore = GetSaveLock(savename);
        await semaphore.WaitAsync();
        try 
        {
            return await _activeSaveSystem.DeleteSave(savename);
        } 
        catch (Exception ex) 
        {
            Debug.LogError($"[SaveManager] Error deleting data from slot '{savename}': {ex.Message}");
            return false;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public Task<bool> Delete(int slotIndex)
        => Delete(SlotNames[Mathf.Clamp(slotIndex, 0, SlotNames.Length - 1)]);
#endregion
}