using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private readonly Dictionary<string, Queue<GameObject>> _pool = new();

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
        if (_pool.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            GameObject go = queue.Dequeue();
            go.SetActive(true);
            return go;
        }

        return fallbackPrefab != null ? Instantiate(fallbackPrefab) : null;
    }

    public void Return(string key, GameObject go)
    {
        go.SetActive(false);
        if (!_pool.ContainsKey(key))
            _pool[key] = new Queue<GameObject>();

        _pool[key].Enqueue(go);
    }
}