using UnityEngine;

public class PlaySFXAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource fxAudioSource;

    [Header("Actions")]
    public AudioClip walk1Audio; // 감정연결음
    public AudioClip walk2Audio;

    // 2. 대쉬 소리 (type 1 또는 2)
    public void PlayWalk1()
    {
        fxAudioSource.PlayOneShot(walk1Audio);
    }

    public void PlayWalk2()
    {
        fxAudioSource.PlayOneShot(walk2Audio);
    }
}