using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class UIAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip countdownSound;
    [SerializeField] private AudioClip countdownEndSound;
    
    private AudioSource _audioSource;

    private AudioSource AudioSource => _audioSource == null
        ? _audioSource = GetComponent<AudioSource>()
        : _audioSource;

    public void PlayButtonSound()
    {
        Play(buttonSound);
    }

    public void PlayCountdownSound()
    {
        Play(countdownSound);
    }
    
    public void PlayCountdownEndSound()
    {
        Play(countdownEndSound);
    }

    private void Play(AudioClip clip)
    {
        AudioUtility.PlayAudioCue(AudioSource, clip);
    }
}
