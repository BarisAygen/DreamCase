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
            Debug.LogError($"Level file not found: {fullPath}");
            return null;
        }

        string json = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<LevelData>(json);
    }
}