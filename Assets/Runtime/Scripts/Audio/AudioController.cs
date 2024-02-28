using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private GameSaver gameSaver;
    [SerializeField] private AudioMixer mixer;

    private const string MainVolumeParameters = "MainVolume";
    private const string MusicVolumeParameters = "MusicVolume";
    private const string SfxVolumeParameters = "SfxVolume";

    private const int MinVolumeDb = -60;
    private const int MaxVolumeDb = 0;

    public float MainVolume
    {
        get => GetMixerVolumeParameter(MainVolumeParameters);
        set => SetMixerVolumeParameter(MainVolumeParameters, value);
    }    
    
    public float MusicVolume
    {
        get => GetMixerVolumeParameter(MusicVolumeParameters);
        set => SetMixerVolumeParameter(MusicVolumeParameters, value);
    }    
    
    public float SfxVolume
    {
        get => GetMixerVolumeParameter(SfxVolumeParameters);
        set => SetMixerVolumeParameter(SfxVolumeParameters, value);
    }

    private void Start()
    {
        LoadAudioPreferences();
    }

    private void LoadAudioPreferences()
    {
        gameSaver.LoadAudioPreferences();
        MainVolume = gameSaver.AudioPreferences.MainVolume;
        MusicVolume = gameSaver.AudioPreferences.MusicVolume;
        SfxVolume = gameSaver.AudioPreferences.SfxVolume;
    }

    public void SaveAudioPreferences()
    {
        var data = new AudioPreferences
        {
            MainVolume = MainVolume,
            MusicVolume = MusicVolume,
            SfxVolume = SfxVolume
        };
        
        gameSaver.SaveAudioPreferences(data);
    }

    private void SetMixerVolumeParameter(string key, float volumePercent)
    {
        var volumeValue = Mathf.Lerp(MinVolumeDb, MaxVolumeDb, volumePercent);
        mixer.SetFloat(key, volumeValue);
    }

    private float GetMixerVolumeParameter(string key)
    {
        if (mixer.GetFloat(key, out var value))
        {
            return Mathf.InverseLerp(MinVolumeDb, MaxVolumeDb, value);
        }

        return 1;
    }
}
