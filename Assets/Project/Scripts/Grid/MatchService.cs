using System.Collections.Generic;
using UnityEngine;

public class MatchService : MonoBehaviour
{
    /// Finds all connected cubes of the given key, starting at (startX,startY).
    public List<Cube> FindConnectedGroup(Item[,] grid, int startX, int startY, string key)
    {
        int width = grid.GetLength(0), height = grid.GetLength(1);
        var result  = new List<Cube>();
        var visited = new bool[width, height];

        void DFS(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) return;
            if (visited[x, y]) return;
            if (!(grid[x, y] is Cube c) || c.Key != key) return;

            visited[x, y] = true;
            result.Add(c);

            DFS(x+1, y);
            DFS(x-1, y);
            DFS(x, y+1);
            DFS(x, y-1);
        }

        DFS(startX, startY);
        return result;
    }

    /// Removes all cubes in the group from the grid and despawns them.
    public void RemoveGroup(Item[,] grid, IEnumerable<Cube> group)
    {
        foreach (var c in group)
        {
            grid[c.GridX, c.GridY] = null;
            GameManager.Instance.TileSpawner.Despawn(c.Key, c.gameObject);
        }
    }
}