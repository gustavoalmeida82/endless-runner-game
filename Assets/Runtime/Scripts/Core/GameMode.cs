using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private MainHUD mainHUD;
    
    [Header("Reload Parameters")]
    [SerializeField] private float reloadGameDelay = 3;

    private void Awake()
    {
        player.enabled = false;
        mainHUD.ShowStartGameOverlay();
    }

    public void OnGameOver()
    {
        StartCoroutine(ReloadGameCoroutine());
    }
    
    private IEnumerator ReloadGameCoroutine()
    {
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

    public void StartCountdown()
    {
        mainHUD.ShowCountdownOverlay();
        mainHUD.StartCountDown();
    }

    public void StartGame()
    {
        mainHUD.ShowHudOverlay();
        playerAnimationController.StartGame();
    }
}
