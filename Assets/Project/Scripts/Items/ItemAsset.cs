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
    public Vector2Int size = Vector2Int.one; 
    public GameObject destroyParticlePrefab;
    public string key;                  
    public GameObject prefab;
    public Sprite mainSprite;
    public Sprite hintedSprite;   
    public ItemType type;
    public Sprite damagedSprite;        
    [Min(0)] public int maxHealth;      
    public bool takesDamageFromCube;
    public bool isDamageable; 
    public bool canFall;               
    public bool isVerticalRocket;      
    [Min(10)] public int PoolSize;
}