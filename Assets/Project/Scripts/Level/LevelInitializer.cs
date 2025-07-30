using System.Collections;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private const string LastLevelKey = "LastLevel";

    private void Start()
    {
        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        int level = PlayerPrefs.GetInt(LastLevelKey, 1);

        LevelData data = LevelDataLoader.LoadLevelData(level);
        if (data == null)
        {
            yield break;
        }

        var gridTask = GridManager.Instance.InitGrid(data);

        while (!gridTask.IsCompleted)
            yield return null;

        LevelUI.Instance.SetupLevel(data);
    }
}