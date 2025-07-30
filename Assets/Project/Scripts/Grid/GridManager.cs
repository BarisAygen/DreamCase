using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform gridParent;
    public Transform GridParent => gridParent;
    [SerializeField] private SpriteRenderer gridBackground;
    [SerializeField] private float tileSize = 1.4f;
    [SerializeField] private float gridPadding = 0.04f;

    private int _width, _height;
    public int Width => _width;
    public int Height => _height;

    private Item[,] _grid;
    public Item[,] Grid => _grid;

    public Vector2 GridOffset => gridBackground.transform.localPosition;
    public bool isInputLocked = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEventManager.OnItemClicked += HandleItemClicked;
        GameEventManager.OnItemDestroyed += HandleItemDestroyed;
        GameEventManager.OnAllActionsComplete += OnAllActionsDone;
    }

    private void OnDisable()
    {
        GameEventManager.OnItemClicked -= HandleItemClicked;
        GameEventManager.OnItemDestroyed -= HandleItemDestroyed;
        GameEventManager.OnAllActionsComplete -= OnAllActionsDone;
    }

    public async Task InitGrid(LevelData data)
    {
        _width = data.grid_width;
        _height = data.grid_height;
        _grid = new Item[_width, _height];

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        float step = tileSize + gridPadding;
        Vector2 boardSize = new Vector2(_width, _height) * step;
        gridBackground.size = boardSize;
        gridBackground.transform.localPosition = new Vector3(0f, -2.8f, 0f);

        Vector2 origin = GridOffset - boardSize * 0.5f;

        for (int i = 0; i < data.grid.Count; i++)
        {
            string key = data.grid[i] == "rand"
                ? new[] { "r", "g", "b", "y" }[Random.Range(0, 4)]
                : data.grid[i];

            int x = i % _width;
            int y = i / _width;
            Vector2 pos = origin + new Vector2(x + 0.5f, y + 0.5f) * step;

            var item = GameManager.Instance.TileSpawner.Spawn(key, x, y, pos, gridParent);
            _grid[x, y] = item;
        }

        GameManager.Instance.PhysicsService.Initialize(_grid, _width, _height, tileSize, gridPadding, gridParent, GridOffset);
        GameManager.Instance.MoveManager.Initialize(data.move_count);
        GameManager.Instance.HintService.ApplyHints(_grid);
    }

    private void HandleItemClicked(Item item)
    {
        if (!item.TryActivate())
            return;

        if (GameManager.Instance.MoveManager.TryConsumeMove())
        {
            if (!ActionTracker.Instance.HasPendingActions())
            {
                isInputLocked = true;
                StartCoroutine(DoPhysicsThenHints());
            }
        }
    }

    private void OnAllActionsDone()
    {
        StartCoroutine(DoPhysicsThenHints());
    }

    private void HandleItemDestroyed(Item item)
    {
        int x = item.GridX;
        int y = item.GridY;

        if (_grid[x, y] == item)
            _grid[x, y] = null;
        if (item.Asset.type == ItemType.Obstacle) 
        {
            GoalManager.Instance.DecreaseGoal(item.Key);
        }
        GameManager.Instance.TileSpawner.Despawn(item.Key, item.gameObject);
    }

    private IEnumerator DoPhysicsThenHints()
    {
        yield return StartCoroutine(GameManager.Instance.PhysicsService.RefillAndApplyGravity());
        GameManager.Instance.HintService.ApplyHints(_grid);
    }
    
    public void DamageObstaclesAroundGroup(List<Cube> group)
    {
        HashSet<Obstacle> damaged = new();

        Vector2Int[] dirs = new[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var cube in group)
        {
            foreach (var dir in dirs)
            {
                int nx = cube.GridX + dir.x;
                int ny = cube.GridY + dir.y;

                if (IsInsideGrid(nx, ny))
                {
                    var item = _grid[nx, ny];
                    if (item is Obstacle obs && !damaged.Contains(obs))
                    {
                        if (obs.Asset.takesDamageFromCube)
                        {
                            obs.TakeDamage();
                            damaged.Add(obs);
                        }
                    }
                }
            }
        }
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }
}