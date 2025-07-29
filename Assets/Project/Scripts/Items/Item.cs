using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public static event System.Action<Item> OnAnyItemClicked;
    public static event System.Action<Item> OnAnyItemDestroyed;

    public string Key => _asset.key;
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    protected ItemAsset _asset;
    public ItemAsset Asset => _asset;

    public virtual void Initialize(ItemAsset asset, int x, int y)
    {
        _asset = asset;
        GridX = x;
        GridY = y;
    }

    public void SetGridPosition(int x, int y)
    {
        GridX = x;
        GridY = y;
    }

    public void OnClicked()
    {
        GameEventManager.ItemClicked(this);
    }

    public void DestroySelf()
    {
        GameEventManager.ItemDestroyed(this);
    }

    public virtual bool TryActivate()
    {
        return false; 
    }
    
    public bool CanFall => _asset.canFall;
}