using System;
using System.Threading.Tasks;

public interface ISaveSystem 
{
    Task<bool> Save<T>(string savename, T data) where T : SaveData;
    Task<T> Load<T>(string savename) where T : SaveData;
    Task<bool> DeleteSave(string savename);
}