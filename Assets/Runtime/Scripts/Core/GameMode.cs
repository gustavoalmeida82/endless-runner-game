using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameMode : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private MainHUD mainHUD;
    [SerializeField] private MusicPlayer musicPlayer;
    
    [Header("Gameplay")]
    [SerializeField] private float startPlayerSpeed = 10;
    [SerializeField] private float maxPlayerSpeed = 17;
    [SerializeField] private float timeToMaxSpeedSeconds = 300;
    [SerializeField] private float reloadGameDelay = 3;
    
    [Header("Score")]
    [SerializeField] private float baseScoreMultiplier = 1;

    [SerializeField] private int startGameCountdown = 5;

    private float _score;
    private float _startGameTime;
    private bool _isGameRunning;
    
    public int CherriesPicked { get; private set; }

    public float Score => Mathf.RoundToInt(_score);

    private void Awake()
    {
        SetWaitForStartGameState();
    }

    private void Update()
    {
        if (!_isGameRunning) return;
        
        var timePercent = (Time.time - _startGameTime) / timeToMaxSpeedSeconds;
        player.ForwardSpeed = Mathf.Lerp(startPlayerSpeed, maxPlayerSpeed, timePercent);
        
        var extraScoreMultiplier = 1 + timePercent;
        _score += baseScoreMultiplier * extraScoreMultiplier * player.ForwardSpeed * Time.deltaTime;
    }

    private void SetWaitForStartGameState()
    {
        player.enabled = false;
        _isGameRunning = false;
        mainHUD.ShowStartGameOverlay();
        musicPlayer.PlayStartMenuMusic();
    }

    public void OnGameOver()
    {
        _isGameRunning = false;
        StartCoroutine(ReloadGameCoroutine());
    }
    
    private IEnumerator ReloadGameCoroutine()
    {
        yield return new WaitForSeconds(1);
        musicPlayer.PlayGameOverMusic();
        yield return new WaitForSeconds(reloadGameDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCor());
    }

    public void OnCherryPickedUp()
    {
        CherriesPicked++;
    }

    private IEnumerator StartGameCor()
    {
        musicPlayer.PlayMainTrackMusic();
        yield return StartCoroutine(mainHUD.PlayStartGameCountdown(startGameCountdown));
        yield return StartCoroutine(playerAnimationController.StartGame());

        player.enabled = true;
        player.ForwardSpeed = startPlayerSpeed;
        _startGameTime = Time.time;
        _isGameRunning = true;
    }
}
