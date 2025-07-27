using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> poolMap = new();

    public GameObject Get(string key, GameObject prefab)
    {
        if (!poolMap.ContainsKey(key))
            poolMap[key] = new Queue<GameObject>();

        if (poolMap[key].Count > 0)
        {
            var obj = poolMap[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(prefab);
    }

    public void Return(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (!poolMap.ContainsKey(key))
            poolMap[key] = new Queue<GameObject>();

        poolMap[key].Enqueue(obj);
    }
}