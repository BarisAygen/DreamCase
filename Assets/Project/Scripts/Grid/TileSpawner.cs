using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TileEntry
    {
        public string key;            
        public GameObject prefab;    
    }

    [Header("Tile Prefabs (key-prefab pairs)")]
    [SerializeField] private List<TileEntry> tiles;

    private Dictionary<string, GameObject> _prefabMap;

    private void Awake()
    {
        _prefabMap = new Dictionary<string, GameObject>();

        foreach (var entry in tiles)
        {
            if (string.IsNullOrWhiteSpace(entry.key) || entry.prefab == null)
            {
                Debug.LogWarning($"[TileSpawner] Invalid entry: key='{entry.key}', prefab={(entry.prefab == null ? "null" : entry.prefab.name)}");
                continue;
            }

            if (_prefabMap.ContainsKey(entry.key))
            {
                Debug.LogWarning($"[TileSpawner] Duplicate key found: '{entry.key}'");
                continue;
            }

            _prefabMap[entry.key] = entry.prefab;
        }
    }

    public async Task<GameObject> Spawn(string key, Vector2 position, Transform parent)
    {
        if (!_prefabMap.TryGetValue(key, out var prefab) || prefab == null)
        {
            Debug.LogWarning($"[TileSpawner] No prefab found for key '{key}'");
            return null;
        }

        GameObject obj = GameManager.Instance.PoolManager.Get(key, prefab);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        return await Task.FromResult(obj);
    }

    public void Despawn(string key, GameObject obj)
    {
        GameManager.Instance.PoolManager.Return(key, obj);
    }
}