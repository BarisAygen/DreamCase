using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject redCubePrefab;
    [SerializeField] private GameObject greenCubePrefab;
    [SerializeField] private GameObject blueCubePrefab;
    [SerializeField] private GameObject yellowCubePrefab;
    [SerializeField] private GameObject rocketVPrefab;
    [SerializeField] private GameObject rocketHPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject vasePrefab;

    private Dictionary<string, GameObject> _prefabMap;

    private void Awake()
    {
        _prefabMap = new Dictionary<string, GameObject>
        {
            { "r", redCubePrefab },
            { "g", greenCubePrefab },
            { "b", blueCubePrefab },
            { "y", yellowCubePrefab },
            { "vro", rocketVPrefab },
            { "hro", rocketHPrefab },
            { "bo", boxPrefab },
            { "s", stonePrefab },
            { "v", vasePrefab },
        };
    }

    public async Task<GameObject> Spawn(string key, Vector2 position, Transform parent)
    {
        if (!_prefabMap.TryGetValue(key, out GameObject prefab) || prefab == null)
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