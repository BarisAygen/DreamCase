using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelDataLoader
{
    private static string LEVEL_FOLDER_PATH =>
        Path.Combine(Application.dataPath, "CaseStudyAssets2025/Levels");

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
            if (key == "bo" || key == "s" || key == "v")
                goalCounts[key] = goalCounts.TryGetValue(key, out var c) ? c + 1 : 1;
        }
        var goalList = new List<LevelGoal>();
        foreach (var kvp in goalCounts)
            goalList.Add(new LevelGoal(kvp.Key, kvp.Value));
        return goalList;
    }
}