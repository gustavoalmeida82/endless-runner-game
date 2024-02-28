using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOverlay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameMode gameMode;
    [SerializeField] private MainHUD mainHUD;
    [SerializeField] private AudioController audioController;

    [Header("UI Elements")] 
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI deleteText;

    private void OnEnable()
    {
        UpdateUI();
        deleteButton.interactable = true;
        deleteText.text = "Delete Data";
    }

    private void OnDisable()
    {
        audioController.SaveAudioPreferences();
    }
        
    private void UpdateUI()
    {
        masterSlider.value = audioController.MainVolume;
        musicSlider.value = audioController.MusicVolume;
        sfxSlider.value = audioController.SfxVolume;
    }

    public void OnCloseButtonClicked()
    {
        //TODO: Assuming we only open from StartGameOverlay. Need go back funcionality.
        mainHUD.ShowStartGameOverlay();
    }

    public void OnMainVolumeChange(float value)
    {
        audioController.MainVolume = value;
    }    
    
    public void OnMusicVolumeChange(float value)
    {
        audioController.MusicVolume = value;
    }    
    
    public void OnSfxVolumeChange(float value)
    {
        audioController.SfxVolume = value;
    }
    
    public void OnDeleteDataButtonClicked()
    {
        gameMode.DeleteData();
        deleteButton.interactable = false;
        deleteText.text = "Deleted";
    }
}
