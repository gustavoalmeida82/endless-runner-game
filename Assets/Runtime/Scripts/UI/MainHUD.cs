using System.Collections;
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

    private void ShowCountdownOverlay()
    {
        countdownOverlay.SetActive(true);
        startGameOverlay.SetActive(false);
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

    public IEnumerator StartCountDown()
    {
        ShowCountdownOverlay();
        
        if (countdownSeconds == 0) yield break;

        var timeToStart = Time.time + countdownSeconds;
        yield return null;
        
        while (Time.time <= timeToStart)
        {
            var remainingTime = timeToStart - Time.time;
            var remainingTimeInt = Mathf.FloorToInt(remainingTime);
            countdownText.text = (remainingTimeInt + 1).ToString();
            var percent = remainingTime - remainingTimeInt;
            countdownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);
            yield return null;
        }
        
        ShowHudOverlay();
    }
}
