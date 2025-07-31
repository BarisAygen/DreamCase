using System.Collections.Generic;
using UnityEngine;

public class MatchService : MonoBehaviour
{
    public List<Cube> FindConnectedGroup(Item[,] grid, int startX, int startY, string key)
    {
        return GridSearchUtil.FindConnectedGroup(grid, startX, startY, key);
    }

    public void RemoveGroup(Item[,] grid, IEnumerable<Cube> group)
    {
        foreach (var c in group)
        {
            grid[c.GridX, c.GridY] = null;
            c.DestroySelf();
        }
    }
}