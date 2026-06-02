using UnityEngine;

namespace GameSaveSystem.Core
{
    public static class SaveSystemsInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static async void Initialize()
        {
            SaveManager.Initialize();
            await ValuesManager.Initialize();
        }
    }
}