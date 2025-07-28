// PhysicsService.cs
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TileSpawner))]
public class PhysicsService : MonoBehaviour
{
    private TileSpawner _spawner;
    private float _tileSize, _padding;
    private int _width, _height;
    private Transform _gridParent;
    private Item[,] _grid;

    private void Awake()
    {
        _spawner = GetComponent<TileSpawner>();
    }

    /// Configures the physics service with the active grid and layout parameters.
    public void Initialize(Item[,] grid, int width, int height,
                           float tileSize, float padding, Transform parent)
    {
        _grid = grid;
        _width = width;
        _height = height;
        _tileSize = tileSize;
        _padding = padding;
        _gridParent = parent;
    }

    /// Slides all cubes down into empty spaces.
    public IEnumerator ApplyGravity()
    {
        float step = _tileSize + _padding;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 1; y < _height; y++)
            {
                if (!(_grid[x, y] is Cube)) continue;
                int targetY = y;
                while (targetY > 0 && _grid[x, targetY - 1] == null)
                    targetY--;

                if (targetY != y)
                {
                    var item = _grid[x, y];
                    _grid[x, targetY] = item;
                    _grid[x, y] = null;

                    // update world position and grid coords
                    item.transform.position = WorldPosition(x, targetY);
                    item.SetGridPosition(x, targetY);
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    /// Fills empty cells by spawning new cubes from above.
    public IEnumerator Refill()
    {
        var colors = new[] { "r", "g", "b", "y" };
        float step = _tileSize + _padding;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] != null) continue;

                string key = colors[Random.Range(0, colors.Length)];
                Vector2 spawnPos = WorldPosition(x, _height); // above top row
                Vector2 targetPos = WorldPosition(x, y);

                var task = _spawner.Spawn(key, x, y, spawnPos, _gridParent);
                yield return new WaitUntil(() => task.IsCompleted);

                var item = task.Result;
                item.transform.position = targetPos;
                _grid[x, y] = item;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    /// Converts grid coordinates to world position for cell centers.
    private Vector2 WorldPosition(int x, int y)
    {
        float step = _tileSize + _padding;
        Vector2 size   = new Vector2(_width, _height) * step;
        Vector2 origin = (Vector2)GameManager.Instance.GridManager.GridOffset - size * 0.5f;
        return origin + new Vector2(x + 0.5f, y + 0.5f) * step;
    }
}