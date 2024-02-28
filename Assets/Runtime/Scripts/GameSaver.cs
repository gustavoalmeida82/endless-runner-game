using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData
{
    public int LastScore { get; set; }
    public int HighestScore { get; set; }
    public int TotalCherriesCollected { get; set; }
}

public class AudioPreferences
{
    public float MainVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SfxVolume { get; set; }
}

public class GameSaver : MonoBehaviour
{
    private const string LastScoreKey = "LastScore";
    private const string HighestScoreKey = "HighestScore";
    private const string TotalCherriesCollectedKey = "TotalCherriesCollected";

    private const string MainVolumeKey = "MainVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    
    public SaveGameData CurrentSave { get; private set; }
    public AudioPreferences AudioPreferences { get; private set; }

    private bool IsLoaded => CurrentSave != null && AudioPreferences != null;

    public void LoadGame()
    {
        if (IsLoaded) return;
        
        CurrentSave = new SaveGameData
        {
            LastScore = PlayerPrefs.GetInt(LastScoreKey, 0),
            HighestScore = PlayerPrefs.GetInt(HighestScoreKey, 0),
            TotalCherriesCollected = PlayerPrefs.GetInt(TotalCherriesCollectedKey, 0)
        };
    }

    public void SaveGame(SaveGameData saveData)
    {
        CurrentSave = saveData;
        
        PlayerPrefs.SetInt(LastScoreKey, CurrentSave.LastScore);
        PlayerPrefs.SetInt(HighestScoreKey, CurrentSave.HighestScore);
        PlayerPrefs.SetInt(TotalCherriesCollectedKey, CurrentSave.TotalCherriesCollected);
        PlayerPrefs.Save();
    }

    public void LoadAudioPreferences()
    {
        if (IsLoaded) return;
        
        AudioPreferences = new AudioPreferences
        {
            MainVolume = PlayerPrefs.GetFloat(MainVolumeKey),
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey),
            SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey),
        };
    }

    public void SaveAudioPreferences(AudioPreferences data)
    {
        AudioPreferences = data;
        
        PlayerPrefs.SetFloat(MainVolumeKey, data.MainVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, data.MusicVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, data.SfxVolume);
        PlayerPrefs.Save();
    }

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        CurrentSave = null;
        AudioPreferences = null;
        LoadGame();
        LoadAudioPreferences();
    }
}
