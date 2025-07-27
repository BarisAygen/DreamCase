using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public GridManager gridManager;
    public int levelToLoad = 1;

    void Start()
    {
        LevelData data = LevelDataLoader.LoadLevelData(levelToLoad);
        gridManager.InitGrid(data);
    }
}
