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
    
    private void Start()
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        int maxLevel = LevelDataLoader.GetMaxLevel();

        levelText.text = level > maxLevel ? "Finished" : $"Level {level}"; 
    }
    
    public void OnLevelButtonClicked() 
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        int maxLevel = LevelDataLoader.GetMaxLevel();

        if (level <= maxLevel)
        {
            SceneManager.LoadScene("LevelScene");
        }
    }
}