using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TileSpawner : MonoBehaviour
{
    private Dictionary<string, GameObject> loadedPrefabs = new();

    public async Task<GameObject> Spawn(string key, Vector2 position, Transform parent)
    {
        GameObject prefab = await LoadPrefab(key);
        GameObject obj = GameManager.Instance.PoolManager.Get(key, prefab);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        return obj;
    }

    private async Task<GameObject> LoadPrefab(string key)
    {
        if (!loadedPrefabs.TryGetValue(key, out GameObject prefab))
        {
            prefab = await Addressables.LoadAssetAsync<GameObject>(key).Task;
            loadedPrefabs[key] = prefab;
        }
        return prefab;
    }

    public void Despawn(string key, GameObject obj)
    {
        GameManager.Instance.PoolManager.Return(key, obj);
    }
    
    public async Task PreloadAll()
    {
        string[] keysToPreload = new[]
        {
            "red", "green", "blue", "yellow",
            "vertical_rocket_0", "horizontal_rocket_0",
            "box_0", "stone_0", "vase_01_0"
        };

        foreach (string key in keysToPreload)
        {
            if (!loadedPrefabs.ContainsKey(key))
            {
                GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(key).Task;
                loadedPrefabs[key] = prefab;
            }
        }
    }
}