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

    /// Slides all fallable items down into empty spaces one cell at a time with animation.
    public IEnumerator ApplyGravity()
    {
        yield return new WaitForSeconds(0.1f); // Rocket oluşumu tamamlanmadan düşme başlamasın

        float step = _tileSize + _padding;
        bool moved;

        do
        {
            moved = false;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 1; y < _height; y++)
                {
                    var item = _grid[x, y];
                    if (item == null || !item.CanFall) continue;
                    if (_grid[x, y - 1] != null) continue;

                    _grid[x, y - 1] = item;
                    _grid[x, y] = null;

                    yield return StartCoroutine(FallToPosition(item, WorldPosition(x, y - 1)));
                    item.SetGridPosition(x, y - 1);

                    moved = true;
                }
            }

        } while (moved);
    }

    /// Fills empty cells by spawning new cubes from above and makes them fall one by one.
    public IEnumerator Refill()
    {
        var colors = new[] { "r", "g", "b", "y" };

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] != null) continue;

                string key = colors[Random.Range(0, colors.Length)];
                Vector2 spawnPos = WorldPosition(x, _height); // way above the top
                Vector2 targetPos = WorldPosition(x, y);

                var task = _spawner.Spawn(key, x, y, spawnPos, _gridParent);
                yield return new WaitUntil(() => task.IsCompleted);

                var item = task.Result;
                _grid[x, y] = item;
                item.SetGridPosition(x, y);

                yield return StartCoroutine(FallToPosition(item, targetPos));
            }
        }

        yield return new WaitForSeconds(0.05f);
    }

    /// Smoothly animates item falling to a specific position.
    private IEnumerator FallToPosition(Item item, Vector2 targetPos, float speed = 15f)
    {
        while (Vector2.Distance(item.transform.position, targetPos) > 0.01f)
        {
            item.transform.position = Vector2.MoveTowards(
                item.transform.position,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        item.transform.position = targetPos;
    }

    /// Converts grid coordinates to world position for cell centers.
    private Vector2 WorldPosition(int x, int y)
    {
        float step = _tileSize + _padding;
        Vector2 size = new Vector2(_width, _height) * step;
        Vector2 origin = (Vector2)GameManager.Instance.GridManager.GridOffset - size * 0.5f;
        return origin + new Vector2(x + 0.5f, y + 0.5f) * step;
    }
}