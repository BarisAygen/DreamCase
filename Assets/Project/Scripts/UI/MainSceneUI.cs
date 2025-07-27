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
        levelText.text = level > 10 ? "Finished" : $"Level {level}";

        levelButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LevelScene");
        });
    }
}