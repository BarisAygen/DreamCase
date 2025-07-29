using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private List<ItemAsset> itemAssets;
    private Dictionary<string, ItemAsset> _assetMap;

    private void Start()
    {
        _assetMap = new();

        foreach (var asset in itemAssets)
        {
            if (!_assetMap.ContainsKey(asset.key))
                _assetMap[asset.key] = asset;

            GameManager.Instance.PoolManager.WarmPool(asset.key, asset.prefab, asset.PoolSize);
        }
    }

    public Item Spawn(string key, int x, int y, Vector2 position, Transform parent)
    {
        if (!_assetMap.TryGetValue(key, out var asset)) return null;

        GameObject obj = GameManager.Instance.PoolManager.Get(key, asset.prefab);
        if (obj == null) return null;

        obj.transform.SetParent(parent);
        obj.transform.position = position;

        if (!obj.TryGetComponent<Item>(out var item)) return null;

        item.Initialize(asset, x, y);
        return item;
    }

    public void Despawn(string key, GameObject go)
    {
        GameManager.Instance.PoolManager.Return(key, go);
    }
    
    public ItemAsset GetAssetByKey(string key)
    {
        _assetMap.TryGetValue(key, out var asset);
        return asset;
    }
}