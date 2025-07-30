using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileSpawner    TileSpawner;
    public MatchService   MatchService;
    public PhysicsService   PhysicsService;
    public MoveManager   MoveManager;
    public HintService   HintService;
    
    public bool IsGameOver { get; private set; } = false;

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
        IsGameOver = false;
    }
    
    private void OnEnable()
    {
        GameEventManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEventManager.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver(bool won)
    {
        IsGameOver = true;
    }
}