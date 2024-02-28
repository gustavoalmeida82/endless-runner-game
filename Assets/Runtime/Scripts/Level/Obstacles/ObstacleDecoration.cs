using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObstacleDecoration : MonoBehaviour
{
    [SerializeField] private AudioClip collisionAudio;
    [SerializeField] private Animation collisionAnimation;
    
    private AudioSource _audioSource;
    private AudioSource AudioSource => _audioSource == null 
        ? _audioSource = GetComponent<AudioSource>() 
        : _audioSource;

    public void PlayCollisionFeedback()
    {
        AudioUtility.PlayAudioCue(AudioSource, collisionAudio);
        if (collisionAnimation != null)
        {
            collisionAnimation.Play();
        }
    }
}
