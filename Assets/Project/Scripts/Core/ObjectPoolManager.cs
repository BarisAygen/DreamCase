using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private class PoolData
    {
        public GameObject prefab;
        public Queue<GameObject> objects = new();
        public int maxSize;
        public Transform holder;
    }

    private readonly Dictionary<string, PoolData> _pools = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void WarmPool(string key, GameObject prefab, int initialCount, int maxSize = int.MaxValue)
    {
        if (!_pools.ContainsKey(key))
        {
            
            GameObject holderObj = new GameObject($"Pool_{key}_Holder");
            holderObj.transform.SetParent(this.transform); 

            _pools[key] = new PoolData
            {
                prefab = prefab,
                maxSize = maxSize,
                holder = holderObj.transform
            };
        }

        var pool = _pools[key];

        for (int i = 0; i < initialCount; i++)
        {
            GameObject go = Instantiate(pool.prefab, pool.holder);
            go.SetActive(false);
            pool.objects.Enqueue(go);
        }
    }

    public GameObject Get(string key)
    {
        if (!_pools.TryGetValue(key, out var pool))
            return null;

        GameObject go;

        if (pool.objects.Count > 0)
        {
            go = pool.objects.Dequeue();
        }
        else
        {
            go = Instantiate(pool.prefab, pool.holder);
        }

        go.SetActive(true);
        return go;
    }

    public void Return(string key, GameObject go)
    {
        if (go == null) return;

        go.SetActive(false);

        if (!_pools.TryGetValue(key, out var pool))
        {
            Destroy(go); // ❗ Geçerli pool yoksa yine fallback destroy
            return;
        }

        go.transform.SetParent(pool.holder);

        if (pool.objects.Count >= pool.maxSize)
        {
            pool.objects.Enqueue(go); // ❗ yine de pool'a al
        }
        else
        {
            pool.objects.Enqueue(go);
        }
    }
}