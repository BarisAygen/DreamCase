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
        public Sprite sprite;
    }

    [SerializeField] private List<TileEntry> tiles;

    private Dictionary<string, TileEntry> _tileMap;

    private void Awake()
    {
        _tileMap = new();

        foreach (var tile in tiles)
        {
            if (string.IsNullOrEmpty(tile.key) || tile.prefab == null || tile.sprite == null)
            {
                Debug.LogWarning($"Invalid tile entry for key '{tile.key}'");
                continue;
            }

            _tileMap[tile.key] = tile;
        }
    }

    public async Task<GameObject> Spawn(string key, int x, int y, Vector2 position, Transform parent)
    {
        if (!_tileMap.TryGetValue(key, out var entry))
        {
            Debug.LogWarning($"[TileSpawner] No prefab found for key '{key}'");
            return null;
        }

        GameObject obj = GameManager.Instance.PoolManager.Get(key, entry.prefab);
        obj.transform.SetParent(parent);
        obj.transform.position = position;

        Cube cube = obj.GetComponent<Cube>();
        cube.Initialize(key, x, y, entry.sprite);

        return await Task.FromResult(obj);
    }

    public void Despawn(string key, GameObject obj)
    {
        GameManager.Instance.PoolManager.Return(key, obj);
    }

    public Sprite GetSpriteForKey(string key)
    {
        return _tileMap.TryGetValue(key, out var entry) ? entry.sprite : null;
    }
}