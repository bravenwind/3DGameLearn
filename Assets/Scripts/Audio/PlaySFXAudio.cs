using UnityEngine;

public class PlaySFXAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource fxAudioSource;

    [Header("Actions")]
    public AudioClip walk1Audio_Wood; 
    public AudioClip walk2Audio_Wood;
    public string walktag_Wood;

    public AudioClip walk1Audio_Rock;
    public AudioClip walk2Audio_Rock;
    public string walktag_Rock;

    public AudioClip walk1Audio_Metal;
    public AudioClip walk2Audio_Metal;
    public string walktag_Metal;

    public AudioClip walk1Audio_Glass;
    public AudioClip walk2Audio_Glass;
    public string walktag_Glass;

    [SerializeField]
    private AudioClip walk1Audio_Current;

    [SerializeField]
    private AudioClip walk2Audio_Current;

    [SerializeField]
    private bool isGrounded = true;

    public void PlayWalk1()
    {
        if (isGrounded)
        {
            fxAudioSource.PlayOneShot(walk1Audio_Current);
        }
    }

    public void PlayWalk2()
    {
        if (isGrounded)
        {
            fxAudioSource.PlayOneShot(walk2Audio_Current);
        }
    }

    public void SetCurrentWalkAudio(string tag)
    {
        if (tag == walktag_Wood)
        {
            walk1Audio_Current = walk1Audio_Wood;
            walk2Audio_Current = walk2Audio_Wood;
        }
        else if (tag == walktag_Rock)
        {
            walk1Audio_Current = walk1Audio_Rock;
            walk2Audio_Current = walk2Audio_Rock;
        }
        else if (tag == walktag_Metal)
        {
            walk1Audio_Current = walk1Audio_Metal;
            walk2Audio_Current = walk2Audio_Metal;
        }
        else if (tag == walktag_Glass)
        {
            walk1Audio_Current = walk1Audio_Glass;
            walk2Audio_Current = walk2Audio_Glass;
        }
        else
        {
            // 예외 상황 기본값
            walk1Audio_Current = walk1Audio_Wood;
            walk2Audio_Current = walk2Audio_Wood;
        }
    }

    public void SetIsGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}