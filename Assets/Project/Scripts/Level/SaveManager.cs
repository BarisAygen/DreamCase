using UnityEngine;

public static class SaveManager
{
    private const string LastLevelKey = "LastLevel";

    public static int LastLevel
    {
        get => PlayerPrefs.GetInt(LastLevelKey, 1);
        set => PlayerPrefs.SetInt(LastLevelKey, value);
    }

    public static void Save() => PlayerPrefs.Save();
}