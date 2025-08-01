using System.Collections.Generic;

public static class GridSearchUtil
{
    public static List<Cube> FindConnectedGroup(Item[,] grid, int startX, int startY, string key, bool[,] visited = null)
    {
        int w = grid.GetLength(0), h = grid.GetLength(1);
        visited ??= new bool[w, h];

        var result = new List<Cube>();

        void DFS(int x, int y)
        {
            if (x < 0 || y < 0 || x >= w || y >= h) return;
            if (visited[x, y]) return;
            if (grid[x, y] is not Cube c || c.Key != key) return;

            visited[x, y] = true;
            result.Add(c);

            DFS(x + 1, y); DFS(x - 1, y);
            DFS(x, y + 1); DFS(x, y - 1);
        }

        DFS(startX, startY);
        return result;
    }
    
    public static List<Rocket> FindConnectedRockets(Item[,] grid, int startX, int startY)
    {
        int w = grid.GetLength(0), h = grid.GetLength(1);
        var visited = new bool[w, h];
        var result = new List<Rocket>();

        void DFS(int x, int y)
        {
            if (x < 0 || y < 0 || x >= w || y >= h) return;
            if (visited[x, y]) return;

            if (grid[x, y] is Rocket r)
            {
                visited[x, y] = true;
                result.Add(r);

                DFS(x + 1, y);
                DFS(x - 1, y);
                DFS(x, y + 1);
                DFS(x, y - 1);
            }
        }

        DFS(startX, startY);
        return result;
    }
}