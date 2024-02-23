using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip rollSound;
    
    private AudioSource _audioSource;

    private AudioSource AudioSource => _audioSource == null
        ? _audioSource = GetComponent<AudioSource>()
        : _audioSource;

    public void PlayJumpSound()
    {
        Play(jumpSound);
    }

    public void PlayRollSound()
    {
        Play(rollSound);
    }

    private void Play(AudioClip clip)
    {
        AudioUtility.PlayAudioCue(AudioSource, clip);
    }
}
