using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MainHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameMode gameMode;
    
    [Header("Overlays")] 
    [SerializeField] private GameObject hudOverlay;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject startGameOverlay;
    [SerializeField] private GameObject countdownOverlay;
    
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI travelledDistanceText;
    
    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int countdownSeconds;

    private UIAudioController _uiAudioController;

    private void Awake()
    {
        _uiAudioController = GetComponent<UIAudioController>();
    }

    private void LateUpdate()
    {
        scoreText.text = $"Score : {player.Score}";
        travelledDistanceText.text = $"{Mathf.RoundToInt(player.TravelledDistance)}m";
    }

    private void ShowPauseOverlay()
    {
        pauseOverlay.SetActive(true);
        hudOverlay.SetActive(false);
        startGameOverlay.SetActive(false);
        countdownOverlay.SetActive(false);
    }

    public void ShowHudOverlay()
    {
        hudOverlay.SetActive(true);
        pauseOverlay.SetActive(false);
        startGameOverlay.SetActive(false);
        countdownOverlay.SetActive(false);
    }

    public void ShowStartGameOverlay()
    {
        startGameOverlay.SetActive(true);
        hudOverlay.SetActive(false);
        pauseOverlay.SetActive(false);
        countdownOverlay.SetActive(false);
    }

    public void ShowCountdownOverlay()
    {
        countdownOverlay.SetActive(true);
        startGameOverlay.SetActive(false);
        hudOverlay.SetActive(false);
        pauseOverlay.SetActive(false);
    }
    
    public void OnResumeButtonClicked()
    {
        ShowHudOverlay();
        _uiAudioController.PlayButtonSound();
        gameMode.ResumeGame();
    }

    public void OnPauseButtonClicked()
    {
        ShowPauseOverlay();
        _uiAudioController.PlayButtonSound();
        gameMode.PauseGame();
    }

    public void OnStartGameButtonClicked()
    {
        _uiAudioController.PlayButtonSound();
        gameMode.StartCountdown();
    }

    public void StartCountDown()
    {
        DOVirtual.Int(countdownSeconds, 0, countdownSeconds, UpdateCountdown)
            .SetEase(Ease.Linear)
            .OnComplete(CountdownComplete);
    }

    private void UpdateCountdown(int currentCountdown)
    {
        if (currentCountdown > 0)
        {
            countdownText.text = $"{currentCountdown}";
            //_uiAudioController.PlayCountdownSound();
        }
        else
        {
            countdownText.text = "Go!";
            //_uiAudioController.PlayCountdownEndSound();
        }
    }

    private void CountdownComplete()
    {
        gameMode.StartGame();
    }
}