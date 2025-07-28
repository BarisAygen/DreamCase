using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Managers")]
    public GridManager GridManager;
    public TileSpawner TileSpawner;
    public ObjectPoolManager PoolManager;
    public LevelInitializer LevelInitializer;
    public ClickHandler ClickHandler;
    public LevelUI LevelUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }
}