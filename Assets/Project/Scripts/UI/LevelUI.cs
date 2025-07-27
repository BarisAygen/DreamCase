using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI movesText;

    private void Start()
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        LevelData data = LevelDataLoader.LoadLevelData(level);

        if (data != null)
        {
            movesText.text = $"{data.move_count}";
        }
        else
        {
            movesText.text = "?";
            Debug.LogError($"Level {level} data not found.");
        }
    }
}