using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gridBackground;
    [SerializeField] private Transform gridParent;

    private int _width, _height;
    private float _tileSize = 1.42f;

    public async Task InitGrid(LevelData data)
    {
        _width = data.grid_width;
        _height = data.grid_height;

        // Clean up existing grid
        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);

        // Setup background
        Vector2 boardSize = new Vector2(_width, _height) * _tileSize;
        gridBackground.gameObject.SetActive(true);
        gridBackground.drawMode = SpriteDrawMode.Sliced;
        gridBackground.size = boardSize;
        gridBackground.transform.position = new Vector3(0f, -2.8f, 0f);
        Vector2 origin = (Vector2)gridBackground.transform.position - boardSize * 0.5f;

        // Parallel tile spawns
        List<Task> spawnTasks = new();

        for (int i = 0; i < data.grid.Count; i++)
        {
            int index = i; // closure fix
            spawnTasks.Add(SpawnTile(data.grid[index], index, origin));
        }

        await Task.WhenAll(spawnTasks);
    }

    private async Task SpawnTile(string key, int index, Vector2 origin)
    {
        if (key == "rand")
        {
            var keys = new[] { "r", "g", "b", "y" };
            key = keys[Random.Range(0, keys.Length)];
        }

        int x = index % _width;
        int y = index / _width;
        Vector2 pos = origin + new Vector2(x + 0.5f, y + 0.5f) * _tileSize;

        await GameManager.Instance.TileSpawner.Spawn(key, pos, gridParent);
    }
}