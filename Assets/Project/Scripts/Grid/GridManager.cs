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
    [SerializeField] private GameObject cubePrefab;

    private int _width, _height;
    private float _tileSize = 1.4f;
    private Cube[,] _grid;

    private void Awake()
    {
        Instance = this;
    }

    public async Task InitGrid(LevelData data)
    {
        _width = data.grid_width;
        _height = data.grid_height;
        _grid = new Cube[_width, _height];

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        float spacingTileSize = _tileSize + 0.04f;
        Vector2 boardSize = new Vector2(_width, _height) * spacingTileSize;
        gridBackground.gameObject.SetActive(true);
        gridBackground.size = boardSize;
        gridBackground.transform.position = new Vector3(0f, -2.8f, 0f);
        Vector2 origin = (Vector2)gridBackground.transform.position - boardSize * 0.5f;

        List<Task> spawnTasks = new();

        for (int i = 0; i < data.grid.Count; i++)
        {
            int index = i;
            spawnTasks.Add(SpawnTile(data.grid[index], index, origin));
        }

        await Task.WhenAll(spawnTasks);
    }

    private async Task SpawnTile(string key, int index, Vector2 origin)
    {
        if (key == "rand")
        {
            var colorKeys = new[] { "r", "g", "b", "y" };
            key = colorKeys[Random.Range(0, colorKeys.Length)];
        }

        int x = index % _width;
        int y = index / _width;

        float spacingTileSize = _tileSize + 0.04f;
        Vector2 pos = origin + new Vector2(x + 0.5f, y + 0.5f) * spacingTileSize;

        GameObject obj = await GameManager.Instance.TileSpawner.Spawn(key, x, y, pos, gridParent);
        Cube cube = obj.GetComponent<Cube>();
        _grid[x, y] = cube;
    }

    public void OnCubeClicked(Cube clickedCube)
    {
        var group = GetConnectedGroup(clickedCube.GridX, clickedCube.GridY, clickedCube.Key);

        if (group.Count < 2)
            return;

        foreach (var cube in group)
        {
            _grid[cube.GridX, cube.GridY] = null;
            GameManager.Instance.TileSpawner.Despawn(cube.Key, cube.gameObject);
        }
    }

    private List<Cube> GetConnectedGroup(int x, int y, string key)
    {
        List<Cube> result = new();
        bool[,] visited = new bool[_width, _height];

        void DFS(int cx, int cy)
        {
            if (cx < 0 || cy < 0 || cx >= _width || cy >= _height) return;
            if (visited[cx, cy]) return;

            Cube current = _grid[cx, cy];
            if (current == null || current.Key != key) return;

            visited[cx, cy] = true;
            result.Add(current);

            DFS(cx + 1, cy);
            DFS(cx - 1, cy);
            DFS(cx, cy + 1);
            DFS(cx, cy - 1);
        }

        DFS(x, y);
        return result;
    }

    public Cube GetCubeAt(int x, int y) => _grid[x, y];
}