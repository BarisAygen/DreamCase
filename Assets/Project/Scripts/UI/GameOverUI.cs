using System;
using DG.Tweening;
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
    [SerializeField] private GameObject retryButtonBackground;
    [SerializeField] private GameObject nextLevelButtonBackground;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        panel.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        nextLevelButtonBackground.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        retryButtonBackground.gameObject.SetActive(false);
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
        panel.transform.localScale = Vector3.zero;

        panel.transform
            .DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack) 
            .OnComplete(() =>
            {
                panel.transform.DOShakeScale(
                    0.3f,             
                    0.1f,             
                    10,               
                    90f,              
                    false,
                    ShakeRandomnessMode.Harmonic 
                );
            });

        resultText.text = win ? "YOU WON!" : "YOU LOST";
        nextLevelButton.gameObject.SetActive(win);
        nextLevelButtonBackground.gameObject.SetActive(win);
        retryButton.gameObject.SetActive(!win);
        retryButtonBackground.gameObject.SetActive(!win);
    }

    public void OnGameOverButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}