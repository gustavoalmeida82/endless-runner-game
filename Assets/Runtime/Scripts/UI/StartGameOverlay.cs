using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameOverlay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameMode gameMode;
    [SerializeField] private MainHUD mainHUD;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI highestScoreText;
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI cherriesCollectedText;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        highestScoreText.text = $"Highest Score\n{gameMode.CurrentSave.HighestScore}";
        lastScoreText.text = $"Last Score\n{gameMode.CurrentSave.LastScore}";
        cherriesCollectedText.text = $"{gameMode.CurrentSave.TotalCherriesCollected}";
    }

    public void OnCloseButtonClicked()
    {
        gameMode.QuitGame();
    }

    public void OnSettingsButtonClicked()
    {
        mainHUD.ShowSettingsOverlay();
    }
}
