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
    [SerializeField] private SpriteRenderer gridBackground;
    [SerializeField] private float tileSize = 1.4f;
    [SerializeField] private float gridPadding = 0.04f;

    private int _width, _height;
    private Item[,] _grid;

    public Vector2 GridOffset => gridBackground.transform.localPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEventManager.OnItemClicked += HandleItemClicked;
        GameEventManager.OnItemDestroyed += HandleItemDestroyed;
    }

    private void OnDisable()
    {
        GameEventManager.OnItemClicked -= HandleItemClicked;
        GameEventManager.OnItemDestroyed -= HandleItemDestroyed;
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
        if (!GameManager.Instance.MoveManager.TryConsumeMove())
            return;

        GameEventManager.MoveUsed();

        int x = item.GridX;
        int y = item.GridY;
        Vector2 clickPos = item.transform.position;

        if (item.Key == "hro") ClearRow(y);
        else if (item.Key == "vro") ClearColumn(x);
        else if (item is Cube cube)
        {
            var group = GameManager.Instance.MatchService.FindConnectedGroup(_grid, x, y, cube.Key);
            if (group.Count < 2) return;

            GameManager.Instance.MatchService.RemoveGroup(_grid, group);

            if (group.Count >= 4)
            {
                string rocketKey = Random.value < 0.5f ? "hro" : "vro";
                var rocket = GameManager.Instance.TileSpawner.Spawn(rocketKey, x, y, clickPos, gridParent);
                rocket.SetGridPosition(x, y);
                _grid[x, y] = rocket;
            }
        }

        StartCoroutine(DoPhysicsThenHints());
    }

    private void HandleItemDestroyed(Item item)
    {
        int x = item.GridX;
        int y = item.GridY;

        if (_grid[x, y] == item)
            _grid[x, y] = null;

        GameManager.Instance.TileSpawner.Despawn(item.Key, item.gameObject);
    }

    private IEnumerator DoPhysicsThenHints()
    {
        yield return StartCoroutine(GameManager.Instance.PhysicsService.ApplyGravity());
        yield return StartCoroutine(GameManager.Instance.PhysicsService.Refill());
        GameManager.Instance.HintService.ApplyHints(_grid);
    }

    public void ClearRow(int row)
    {
        for (int x = 0; x < _width; x++)
            _grid[x, row]?.DestroySelf();
    }

    public void ClearColumn(int col)
    {
        for (int y = 0; y < _height; y++)
            _grid[col, y]?.DestroySelf();
    }
}