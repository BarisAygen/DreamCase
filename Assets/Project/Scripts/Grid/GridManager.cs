using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [Header("Grid References")]
    [SerializeField] private SpriteRenderer gridBackground;
    [SerializeField] private Transform gridParent;

    private int _width, _height;
    private float _tileSize = 1.4f;

    public async Task InitGrid(LevelData data)
    {
        _width = data.grid_width;
        _height = data.grid_height;

        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);

        float spacingTileSize = _tileSize + 0.04f;
        Vector2 boardSize = new Vector2(_width, _height) * spacingTileSize;
        gridBackground.gameObject.SetActive(true);
        gridBackground.drawMode = SpriteDrawMode.Sliced;
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

        await GameManager.Instance.TileSpawner.Spawn(key, pos, gridParent);
    }
}