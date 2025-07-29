using UnityEngine;

public enum ItemType { Cube, Obstacle, Special }

[CreateAssetMenu(fileName = "NewItem", menuName = "Tile/Item Asset")]
public class ItemAsset : ScriptableObject
{
    public string key;                  // "r", "g", "vro", "bo", vs.
    public ItemType type;              // Enum: Cube, Rocket, Obstacle
    public Sprite mainSprite;
    public Sprite hintedSprite;        
    public Sprite damagedSprite;        
    public int maxHealth;              
    public bool canFall;               
    public bool isVerticalRocket;      
    public GameObject prefab;
    [Min(1)] public int PoolSize = 10;
}