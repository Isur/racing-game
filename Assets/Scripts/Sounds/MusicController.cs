using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] MusicClips;
    [Range(0f, 1f)]
    public float Volume = 0.15f;

    private AudioSource MusicAudioSource { get; set; }

    void Start()
    {
        SetupAudioSource();
        SelectMusic();
    }

    private void SetupAudioSource()
    {
        MusicAudioSource = gameObject.AddComponent<AudioSource>();
        var mas = MusicAudioSource;
        mas.volume = Volume;
        mas.loop = true;
    }

    private void SelectMusic()
    {
        var index = Random.Range(0, MusicClips.Length);
        var musicClip = MusicClips[index];
        MusicAudioSource.clip = musicClip;
        MusicAudioSource.Play();
    }
}
