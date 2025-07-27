using System.Collections;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        LevelData data = LevelDataLoader.LoadLevelData(level);
        if (data == null)
        {
            Debug.LogError($"Level {level} data not found.");
            yield break;
        }

        var gridTask = GameManager.Instance.GridManager.InitGrid(data);
        while (!gridTask.IsCompleted) yield return null;
    }
}