using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int GridX { get; protected set; }
    public int GridY { get; protected set; }
    public string Key { get; protected set; }
    
    public void SetGridPosition(int x, int y)
    {
        GridX = x;
        GridY = y;
    }
    
    public virtual void Initialize(string key, int x, int y)
    {
        Key = key;
        GridX = x;
        GridY = y;
    }

    public virtual void OnClicked() {}
}