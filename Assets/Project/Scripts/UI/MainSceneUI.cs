using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField, Tooltip("The button that starts the level")]
    private Button levelButton;

    [SerializeField, Tooltip("Text that shows the current level number")]
    private TextMeshProUGUI levelText;

    private const int MaxLevel = 10; // Currently max # of levels in game

    private void Start()
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        if (levelText == null || levelButton == null)
        {
            Debug.LogError("UI references not assigned.");
            return;
        }

        levelText.text = level > MaxLevel ? "Finished" : $"Level {level}"; // Modify level button accordingly
    }
    
    public void OnLevelButtonClicked() // Change the scene and load the level
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        if (level <= MaxLevel)
        {
            SceneManager.LoadScene("LevelScene");
        }
    }
}