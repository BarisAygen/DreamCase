using UnityEngine;

public class HintService : MonoBehaviour
{
    public void ApplyHints(Item[,] grid)
    {
        int w = grid.GetLength(0), h = grid.GetLength(1);
        var visited = new bool[w, h];

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
            if (grid[x, y] is Cube c)
                c.SetHintState(false);

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
        {
            if (visited[x, y]) continue;
            if (grid[x, y] is not Cube cube) continue;

            var group = GridSearchUtil.FindConnectedGroup(grid, x, y, cube.Key, visited);

            if (group.Count >= 4)
                foreach (var g in group)
                    g.SetHintState(true);
        }
    }
}