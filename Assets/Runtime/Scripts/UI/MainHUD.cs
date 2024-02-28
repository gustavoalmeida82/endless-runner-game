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
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI travelledDistanceText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI cherryCountText;

    private UIAudioController _uiAudioController;

    private void Awake()
    {
        ShowHudOverlay();
        _uiAudioController = GetComponent<UIAudioController>();
    }

    private void LateUpdate()
    {
        scoreText.text = $"Score : {gameMode.Score}";
        travelledDistanceText.text = $"{Mathf.RoundToInt(player.TravelledDistance)}m";
        cherryCountText.text = $"{gameMode.CherriesPicked}";
    }

    private void ShowPauseOverlay()
    {
        pauseOverlay.SetActive(true);
        hudOverlay.SetActive(false);
        startGameOverlay.SetActive(false);
    }

    public void ShowHudOverlay()
    {
        hudOverlay.SetActive(true);
        pauseOverlay.SetActive(false);
        startGameOverlay.SetActive(false);
    }

    public void ShowStartGameOverlay()
    {
        startGameOverlay.SetActive(true);
        hudOverlay.SetActive(false);
        pauseOverlay.SetActive(false);
    }
    
    public void OnResumeButtonClicked()
    {
        ShowHudOverlay();
        gameMode.ResumeGame();
    }

    public void OnPauseButtonClicked()
    {
        ShowPauseOverlay();
        gameMode.PauseGame();
    }

    public void OnStartGameButtonClicked()
    {
        gameMode.StartGame();
    }

    public IEnumerator PlayStartGameCountdown(int countdownSeconds)
    {
        ShowHudOverlay();
        countdownText.gameObject.SetActive(false);

        if (countdownSeconds == 0)
        {
            yield break;
        }

        var timeToStart = Time.time + countdownSeconds;
        yield return null;
        
        countdownText.gameObject.SetActive(true);
        var previousRemainingTimeInt = countdownSeconds;
        
        while (Time.time <= timeToStart)
        {
            var remainingTime = timeToStart - Time.time;
            var remainingTimeInt = Mathf.FloorToInt(remainingTime);
            countdownText.text = (remainingTimeInt + 1).ToString();

            if (previousRemainingTimeInt != remainingTimeInt)
            {
                _uiAudioController.PlayCountdownSound();
            }

            previousRemainingTimeInt = remainingTimeInt;
            
            var percent = remainingTime - remainingTimeInt;
            countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);
            yield return null;
        }
        
        _uiAudioController.PlayCountdownEndSound();
        
        countdownText.gameObject.SetActive(false);
    }
}
