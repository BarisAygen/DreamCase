using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TileSpawner    TileSpawner;
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
}