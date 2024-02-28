using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject pickupModel;
    [SerializeField] private AudioClip pickupAudio;

    public void OnPickedUp()
    {
        var audioSource = GetComponent<AudioSource>();
        AudioUtility.PlayAudioCue(audioSource, pickupAudio);
        pickupModel.SetActive(false);
        Destroy(gameObject, pickupAudio.length);
    }
}
