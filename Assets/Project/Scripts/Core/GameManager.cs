using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileSpawner    TileSpawner;
    public MatchService   MatchService;
    public PhysicsService   PhysicsService;
    public MoveManager   MoveManager;
    public HintService   HintService;
    public ClickHandler ClickHandler;
    
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
        ClickHandler.enabled = true;
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
        ClickHandler.enabled = false;

        if (won)
        {
            int level = PlayerPrefs.GetInt("LastLevel", 1);
            int maxLevel = LevelDataLoader.GetMaxLevel();
            // Win animation
            SceneManager.LoadScene("MainScene");

            if (level <= maxLevel)
            {
                PlayerPrefs.SetInt("LastLevel", level + 1);
                PlayerPrefs.Save();
            }
        }
    }
}