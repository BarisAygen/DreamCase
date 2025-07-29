using UnityEngine;

public enum ItemType
{
    Cube,
    Obstacle,
    Special
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Tile/Item Asset")]
public class ItemAsset : ScriptableObject
{
    public string key;                  
    public GameObject prefab;
    public Sprite mainSprite;
    public Sprite hintedSprite;   
    public ItemType type;
    public Sprite damagedSprite;        
    [Min(0)] public int maxHealth;              
    public bool canFall;               
    public bool isVerticalRocket;      
    [Min(5)] public int PoolSize = 10;
}