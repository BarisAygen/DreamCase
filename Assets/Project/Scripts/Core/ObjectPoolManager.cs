using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private readonly Dictionary<string, Queue<GameObject>> _pool = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void WarmPool(string key, GameObject prefab, int count)
    {
        if (!_pool.ContainsKey(key))
            _pool[key] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(false);
            _pool[key].Enqueue(go);
        }
    }
    
    public GameObject Get(string key, GameObject fallbackPrefab)
    {
        GameObject go = null;

        if (_pool.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            go = queue.Dequeue();
        }
        else if (fallbackPrefab != null)
        {
            go = Instantiate(fallbackPrefab);
        }

        if (go != null)
        {
            go.SetActive(true);
        }

        return go;
    }
    
    public void Return(string key, GameObject go)
    {
        if (go is null) return;

        go.SetActive(false);

        if (!_pool.ContainsKey(key))
        {
            _pool[key] = new Queue<GameObject>();
        }

        _pool[key].Enqueue(go);
    }
}