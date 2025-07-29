using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelDataLoader
{
    private static string LEVEL_FOLDER_PATH =>
        Path.Combine(Application.dataPath, "CaseStudyAssets2025/Levels");

    public static int GetMaxLevel()
    {
        var files = Directory.GetFiles(LEVEL_FOLDER_PATH, "level_*.json");
        return files.Length;
    }
    
    public static LevelData LoadLevelData(int levelNumber)
    {
        string fileName = $"level_{levelNumber:00}.json";
        string fullPath = Path.Combine(LEVEL_FOLDER_PATH, fileName);

        if (!File.Exists(fullPath))
        {
            return null;
        }

        string json = File.ReadAllText(fullPath);

        var data = JsonUtility.FromJson<LevelData>(json);

        data.goals = CalculateGoalsFromGrid(data.grid);

        return data;
    }

    private static List<LevelGoal> CalculateGoalsFromGrid(List<string> grid)
    {
        var goalCounts = new Dictionary<string, int>();

        foreach (string key in grid)
        {
            var asset = GameManager.Instance.TileSpawner.GetAssetByKey(key);
            if (asset is null || asset.type != ItemType.Obstacle) continue;

            goalCounts[key] = goalCounts.TryGetValue(key, out int count) ? count + 1 : 1;
        }

        var goalList = new List<LevelGoal>();
        foreach (var kvp in goalCounts)
            goalList.Add(new LevelGoal(kvp.Key, kvp.Value));
        return goalList;
    }
}