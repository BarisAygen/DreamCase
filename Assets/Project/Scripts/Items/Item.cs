using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Key => _asset.key;
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    protected ItemAsset _asset;

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
    
    public virtual void OnClicked() { }

    public virtual bool CanFall => _asset.canFall;
}