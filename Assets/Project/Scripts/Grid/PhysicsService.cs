using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TileSpawner))]
public class PhysicsService : MonoBehaviour
{
    [Header("Fall Settings")]
    [SerializeField] private float fallDuration = 0.25f;
    [SerializeField] private int spawnHeightOffset = 2;
    [SerializeField] private float bounceHeight = 0.1f;
    [SerializeField] private float bounceDuration = 0.07f;

    private TileSpawner _spawner;
    private float _tileSize, _padding;
    private int _width, _height;
    private Transform _gridParent;
    private Item[,] _grid;
    private Vector2 _gridOffset;

    private void Awake()
    {
        _spawner = GetComponent<TileSpawner>();
    }

    public void Initialize(Item[,] grid, int width, int height, float tileSize, float padding, Transform parent, Vector2 offset)
    {
        _grid = grid;
        _width = width;
        _height = height;
        _tileSize = tileSize;
        _padding = padding;
        _gridParent = parent;
        _gridOffset = offset;
    }

    public IEnumerator RefillAndApplyGravity()
    {
        yield return new WaitForSeconds(0.05f);

        List<(Item item, Vector2 targetPos, bool isNew)> moveList = new();

        for (int x = 0; x < _width; x++)
        {
            int emptyY = -1;

            for (int y = 0; y < _height; y++)
            {
                var item = _grid[x, y];

                if (_grid[x, y] is null)
                {
                    if (emptyY == -1) emptyY = y;
                }
                else if (item.CanFall && emptyY != -1)
                {
                    Vector2 targetPos = WorldPosition(x, emptyY);
                    moveList.Add((item, targetPos, false));

                    _grid[x, emptyY] = item;
                    _grid[x, y] = null;
                    item.SetGridPosition(x, emptyY);

                    emptyY++;
                }
            }
            
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] is not null) continue;

                string key = GetRandomColorKey();
                Vector2 spawnPos = WorldPosition(x, _height + spawnHeightOffset);
                Vector2 targetPos = WorldPosition(x, y);

                var item = _spawner.Spawn(key, x, y, spawnPos, _gridParent);
                if (item is null) continue;

                _grid[x, y] = item;
                item.SetGridPosition(x, y);

                moveList.Add((item, targetPos, true));
            }
        }

        List<Coroutine> coroutines = new();
        foreach (var (item, targetPos, isNew) in moveList)
        {
            coroutines.Add(StartCoroutine(FallToPositionSmooth(item, targetPos, fallDuration, isNew)));
        }

        foreach (var c in coroutines)
            yield return c;

        yield return new WaitForSeconds(0.05f);
    }

    private IEnumerator FallToPositionSmooth(Item item, Vector2 targetPos, float duration, bool isNew)
    {
        Vector2 startPos = item.transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float easedT = EaseInQuad(t);
            item.transform.position = Vector2.Lerp(startPos, targetPos, easedT);
            yield return null;
        }

        item.transform.position = targetPos;

        if (isNew)
            yield return StartCoroutine(SquashStretch(item, bounceHeight, bounceDuration));
    }

    private IEnumerator SquashStretch(Item item, float height, float duration)
    {
        Vector2 original = item.transform.position;
        Vector2 up = original + Vector2.up * height;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2);
            item.transform.position = Vector2.Lerp(original, up, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2);
            item.transform.position = Vector2.Lerp(up, original, t);
            yield return null;
        }

        item.transform.position = original;
    }

    private string GetRandomColorKey()
    {
        string[] colors = { "r", "g", "b", "y" };
        return colors[Random.Range(0, colors.Length)];
    }

    private float EaseInQuad(float t)
    {
        return t * t;
    }

    private Vector2 WorldPosition(int x, int y)
    {
        float step = _tileSize + _padding;
        Vector2 size = new Vector2(_width, _height) * step;
        Vector2 origin = _gridOffset - size * 0.5f;
        return origin + new Vector2(x + 0.5f, y + 0.5f) * step;
    }
}