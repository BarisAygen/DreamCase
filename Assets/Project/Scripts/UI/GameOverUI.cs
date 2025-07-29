using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        panel.SetActive(false);

        nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnEnable()
    {
        GameEventManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEventManager.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver(bool win)
    {
        panel.SetActive(true);
        resultText.text = win ? "YOU WIN!" : "YOU LOSE";
        nextLevelButton.gameObject.SetActive(win);
        retryButton.gameObject.SetActive(!win);
    }

    private void OnNextLevelClicked()
    {
        int lastLevel = PlayerPrefs.GetInt("LastLevel", 1);
        PlayerPrefs.SetInt("LastLevel", lastLevel + 1);
        SceneManager.LoadScene("LevelScene");
    }

    private void OnRetryClicked()
    {
        SceneManager.LoadScene("LevelScene");
    }

    private void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}