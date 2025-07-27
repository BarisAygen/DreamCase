using UnityEditor;
using UnityEngine;

public static class LevelEditorMenu
{
    [MenuItem("SetLevelTool/Set Last Level to 1", false, 1)]
    private static void SetLevel1() => SetLevel(1);

    [MenuItem("SetLevelTool/Set Last Level to 2", false, 2)]
    private static void SetLevel2() => SetLevel(2);

    [MenuItem("SetLevelTool/Set Last Level to 3", false, 3)]
    private static void SetLevel3() => SetLevel(3);

    [MenuItem("SetLevelTool/Set Last Level to 4", false, 4)]
    private static void SetLevel4() => SetLevel(4);

    [MenuItem("SetLevelTool/Set Last Level to 5", false, 5)]
    private static void SetLevel5() => SetLevel(5);

    [MenuItem("SetLevelTool/Set Last Level to 6", false, 6)]
    private static void SetLevel6() => SetLevel(6);

    [MenuItem("SetLevelTool/Set Last Level to 7", false, 7)]
    private static void SetLevel7() => SetLevel(7);

    [MenuItem("SetLevelTool/Set Last Level to 8", false, 8)]
    private static void SetLevel8() => SetLevel(8);

    [MenuItem("SetLevelTool/Set Last Level to 9", false, 9)]
    private static void SetLevel9() => SetLevel(9);

    [MenuItem("SetLevelTool/Set Last Level to 10", false, 10)]
    private static void SetLevel10() => SetLevel(10);

    private static void SetLevel(int level)
    {
        PlayerPrefs.SetInt("LastLevel", level);
        PlayerPrefs.Save();
        Debug.Log($"[SetLevelTool] LastLevel set to {level}");
    }
}