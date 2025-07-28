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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Spawns all items, then wires up physics, move counter, and hints.
    public async Task InitGrid(LevelData data)
    {
        // store dimensions and clear old children
        _width = data.grid_width;
        _height = data.grid_height;
        _grid = new Item[_width, _height];
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // resize background to fit grid
        float step = tileSize + gridPadding;
        Vector2 boardSize = new Vector2(_width, _height) * step;
        gridBackground.size = boardSize;
        gridBackground.transform.localPosition = new Vector3(0f, -2.8f, 0f);

        // compute origin and spawn tasks
        Vector2 origin = (Vector2)gridBackground.transform.localPosition - boardSize * 0.5f;
        var spawnTasks = new List<Task>();
        for (int i = 0; i < data.grid.Count; i++)
        {
            int index = i;
            spawnTasks.Add(SpawnCell(data.grid[index], index, origin));
        }
        await Task.WhenAll(spawnTasks);

        // initialize physics, moves, and hints
        GameManager.Instance.PhysicsService.Initialize(_grid, _width, _height, tileSize, gridPadding, gridParent);
        GameManager.Instance.MoveManager.Initialize(data.move_count);
        GameManager.Instance.HintService.ApplyHints(_grid);
    }

    /// Spawns a single cell at the given grid index.
    private async Task SpawnCell(string key, int index, Vector2 origin)
    {
        if (key == "rand")
        {
            var colors = new[] { "r", "g", "b", "y" };
            key = colors[Random.Range(0, colors.Length)];
        }

        int x = index % _width;
        int y = index / _width;
        Vector2 position = origin + new Vector2(x + 0.5f, y + 0.5f) * (tileSize + gridPadding);

        Item item = await GameManager.Instance.TileSpawner.Spawn(key, x, y, position, gridParent);
        _grid[x, y] = item;
    }

    /// Called by Cube.OnClicked: processes moves, rockets, matches,
    public void OnCubeClicked(Cube clicked)
    {
        // consume one move; abort if none left
        if (!GameManager.Instance.MoveManager.TryConsumeMove())
            return;

        int x = clicked.GridX;
        int y = clicked.GridY;
        Vector2 clickPos = clicked.transform.position;

        // handle rocket taps
        if (clicked.Key == "hro" || clicked.Key == "vro")
        {
            if (clicked.Key == "hro") ClearRow(y);
            else ClearColumn(x);

            StartCoroutine(DoPhysicsThenHints());
            return;
        }

        // normal cube match
        var group = GameManager.Instance.MatchService.FindConnectedGroup(_grid, x, y, clicked.Key);
        if (group.Count < 2)
            return;

        GameManager.Instance.MatchService.RemoveGroup(_grid, group);

        // spawn rocket if group >= 4
        if (group.Count >= 4)
        {
            string rocketKey = (Random.value < 0.5f) ? "hro" : "vro";
            _ = GameManager.Instance.TileSpawner
                .Spawn(rocketKey, x, y, clickPos, gridParent)
                .ContinueWith(t =>
                {
                    var rocket = t.Result;
                    rocket.SetGridPosition(x, y);
                    _grid[x, y] = rocket;
                });
        }

        // then gravity, refill, hints
        StartCoroutine(DoPhysicsThenHints());
    }

    /// Runs gravity and refill coroutines in sequence, then reapplies hints.
    private IEnumerator DoPhysicsThenHints()
    {
        yield return StartCoroutine(GameManager.Instance.PhysicsService.ApplyGravity());
        yield return StartCoroutine(GameManager.Instance.PhysicsService.Refill());
        Cube[,] cubeGrid = new Cube[_width, _height];
        for (int x = 0; x < _width; x++)
        for (int y = 0; y < _height; y++)
            cubeGrid[x, y] = _grid[x, y] as Cube;

        GameManager.Instance.HintService.ApplyHints(cubeGrid);
    }

    public void ClearRow(int row)
    {
        for (int x = 0; x < _width; x++)
            ClearItemAt(x, row);
    }

    public void ClearColumn(int col)
    {
        for (int y = 0; y < _height; y++)
            ClearItemAt(col, y);
    }

    public void ClearItemAt(int x, int y)
    {
        var item = _grid[x, y];
        if (item is null) return;
        _grid[x, y] = null;
        GameManager.Instance.TileSpawner.Despawn(item.Key, item.gameObject);
    }
}