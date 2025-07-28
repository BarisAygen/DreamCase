using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GridManager    GridManager;
    public TileSpawner    TileSpawner;
    public ObjectPoolManager PoolManager;
    public MatchService   MatchService;
    public PhysicsService   PhysicsService;
    public MoveManager   MoveManager;
    public HintService   HintService;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        TileSpawner.Initialize();
    }
    
}