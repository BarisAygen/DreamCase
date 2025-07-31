using System;
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

    private void Start()
    {
        panel.SetActive(false);
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
        if (!win)
        {
            panel.SetActive(true);
        }
        resultText.text = win ? "YOU WON!" : "YOU LOST";
        nextLevelButton.gameObject.SetActive(win);
        retryButton.gameObject.SetActive(!win);
    }

    public void OnGameOverClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}