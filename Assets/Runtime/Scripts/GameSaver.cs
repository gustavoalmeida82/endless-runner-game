using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveGameData
{
    public int LastScore { get; set; }
    public int HighestScore { get; set; }
    public int TotalCherriesCollected { get; set; }
}

public class AudioPreferences
{
    public float MainVolume { get; set; } = 1;
    public float MusicVolume { get; set; } = 1;
    public float SfxVolume { get; set; } = 1;
}

public class GameSaver : MonoBehaviour
{
    private string SaveGameFilePath => $"{Application.persistentDataPath}/saveGame.json";
    private string AudioPreferencesFilePath => $"{Application.persistentDataPath}/preferences.json";
    
    public SaveGameData CurrentSave { get; private set; }
    public AudioPreferences AudioPreferences { get; private set; }

    private bool IsLoaded => CurrentSave != null && AudioPreferences != null;

    public void LoadGame()
    {
        if (IsLoaded) return;

        CurrentSave = LoadGameDataFromFile(SaveGameFilePath) ?? new SaveGameData();
    }

    public void SaveGame(SaveGameData saveData)
    {
        CurrentSave = saveData;
        SaveGameDataToFile(SaveGameFilePath, saveData);
    }
    
    private void SaveGameDataToFile(string filePath, SaveGameData data)
    {
        using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using StreamWriter writer = new StreamWriter(stream);
        using JsonWriter jsonWriter = new JsonTextWriter(writer);
        
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, data);
    }
    
    //TODO: Use generics
    private SaveGameData LoadGameDataFromFile(string filePath)
    {
        using FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
        using StreamReader reader = new StreamReader(stream);
        using JsonReader jsonReader = new JsonTextReader(reader);

        JsonSerializer serializer = new JsonSerializer();
        return serializer.Deserialize<SaveGameData>(jsonReader);
    }

    public void LoadAudioPreferences()
    {
        if (IsLoaded) return;

        AudioPreferences = LoadAudioPreferencesFromFile(AudioPreferencesFilePath) ?? new AudioPreferences();
    }
    
    //TODO: Use generics
    private AudioPreferences LoadAudioPreferencesFromFile(string filePath)
    {
        using FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
        using StreamReader reader = new StreamReader(stream);
        using JsonReader jsonReader = new JsonTextReader(reader);

        JsonSerializer serializer = new JsonSerializer();
        return serializer.Deserialize<AudioPreferences>(jsonReader);
    }

    public void SaveAudioPreferences(AudioPreferences data)
    {
        AudioPreferences = data;
        SaveAudioPreferencesToFile(AudioPreferencesFilePath, data);
    }

    private void SaveAudioPreferencesToFile(string filePath, AudioPreferences data)
    {
        using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using StreamWriter writer = new StreamWriter(stream);
        using JsonWriter jsonWriter = new JsonTextWriter(writer);

        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, data);
    }

    public void DeleteAllData()
    {
        File.Delete(SaveGameFilePath);
        File.Delete(AudioPreferencesFilePath);
        CurrentSave = null;
        AudioPreferences = null;
        LoadGame();
        LoadAudioPreferences();
    }
}
