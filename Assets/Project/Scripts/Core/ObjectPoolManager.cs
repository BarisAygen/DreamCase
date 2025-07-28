using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private readonly Dictionary<string, Queue<GameObject>> _poolMap = new();

    // Instantiate and store one instance of the prefab for future use
    public void Preload(string key, GameObject prefab)
    {
        if (!_poolMap.ContainsKey(key))
            _poolMap[key] = new Queue<GameObject>();

        var obj = Instantiate(prefab);
        obj.SetActive(false);
        _poolMap[key].Enqueue(obj);
    }

    // Get an object from the pool if available, otherwise return null
    public GameObject Get(string key)
    {
        if (_poolMap.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.SetActive(true); // Reactivate before use
            return obj;
        }

        return null;
    }

    // Return object to the pool for reuse
    public void Return(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (!_poolMap.ContainsKey(key))
            _poolMap[key] = new Queue<GameObject>();

        _poolMap[key].Enqueue(obj);
    }
}