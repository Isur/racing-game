using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip WinClip;
    public AudioClip LooseClip;
    public AudioClip ClickClip;
    [Range(0f, 1f)]
    public float Volume = 1f;

    private AudioSource WinAudioSource { get; set; }
    private AudioSource LooseAudioSource { get; set; }
    private AudioSource ClickAudioSource { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        WinAudioSource = gameObject.AddComponent<AudioSource>();
        WinAudioSource.clip = WinClip;
        WinAudioSource.volume = Volume;
        LooseAudioSource = gameObject.AddComponent<AudioSource>();
        LooseAudioSource.clip = LooseClip;
        LooseAudioSource.volume = Volume;
        ClickAudioSource = gameObject.AddComponent<AudioSource>();
        ClickAudioSource.clip = ClickClip;
        ClickAudioSource.volume = Volume;
    }

    public void PlayWin()
    {
        WinAudioSource.Play();
    }

    public void PlayLoose()
    {
        LooseAudioSource.Play();
    }

    public void PlayClick()
    {
        ClickAudioSource.Play();
    }
}
