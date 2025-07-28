using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CubeData
    {
        public string key;                    // e.g., "r", "g", "b", "y"
        public Sprite normalSprite;           // Default appearance
        public Sprite hintedSprite;           // Hint appearance
    }

    [System.Serializable]
    public class KeyPrefabPair
    {
        public string key;                    // e.g., "r", "hro", "vro", etc.
        public GameObject prefab;             // Corresponding prefab
    }

    [Header("Prefab Mapping")]
    [SerializeField] private List<KeyPrefabPair> prefabPairs;

    [Header("Cube Sprites")]
    [SerializeField] private List<CubeData> cubeSprites;

    private Dictionary<string, GameObject> _prefabMap;   // key → prefab
    private Dictionary<string, CubeData> _spriteMap;     // key → sprite data
    
    public void Initialize()
    {
        _prefabMap = new();
        _spriteMap = new();

        foreach (var pair in prefabPairs)
        {
            _prefabMap[pair.key] = pair.prefab;

            if (GameManager.Instance?.PoolManager == null)
            {
                continue;
            }

            GameManager.Instance.PoolManager.Preload(pair.key, pair.prefab);
        }

        foreach (var cube in cubeSprites)
            _spriteMap[cube.key] = cube;
    }

    // Spawns an object at given grid position using pooling
    public async Task<Item> Spawn(string key, int x, int y, Vector2 position, Transform parent)
    {
        var pool = GameManager.Instance.PoolManager;

        // Safer pattern matching and fallback
        GameObject obj = pool.Get(key);
        if (obj is null && _prefabMap.TryGetValue(key, out var fallbackPrefab))
        {
            obj = Instantiate(fallbackPrefab);
        }

        if (obj is null)
        {
            return null;
        }

        obj.transform.SetParent(parent);
        obj.transform.position = position;

        // Prefer TryGetComponent over GetComponent + type check
        if (!obj.TryGetComponent<Item>(out var item))
        {
            return null;
        }

        item.Initialize(key, x, y);

        if (item is Cube cube && _spriteMap.TryGetValue(key, out var data))
            cube.SetSprites(data.normalSprite, data.hintedSprite);

        return await Task.FromResult(item);
    }

    // Despawns (returns) object back to pool
    public void Despawn(string key, GameObject obj)
    {
        GameManager.Instance.PoolManager.Return(key, obj);
    }
}