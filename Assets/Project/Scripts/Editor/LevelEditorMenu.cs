using UnityEditor;
using UnityEngine;

public class SetLevelToolWindow : EditorWindow
{
    private int levelToSet = 1;

    [MenuItem("SetLevelTool/Set Level Manually")]
    public static void ShowWindow()
    {
        GetWindow<SetLevelToolWindow>("Set Level");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Last Level", EditorStyles.boldLabel);

        levelToSet = EditorGUILayout.IntField("Level", levelToSet);

        if (GUILayout.Button("Set Last Level"))
        {
            PlayerPrefs.SetInt("LastLevel", levelToSet);
            PlayerPrefs.Save();
            Debug.Log($"[SetLevelTool] LastLevel set to {levelToSet}");
        }
    }
}