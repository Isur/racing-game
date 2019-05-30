using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioClip EngineSound;
    public AudioClip CrashSound;

    private AudioSource EngineAudioSource { get; set; }
    private AudioSource CrashAudioSource { get; set; }

    void Start()
    {
        SetupEngineAudioSource();
        SetupCrashAudioSource();
    }

    private void SetupEngineAudioSource()
    {

        EngineAudioSource = gameObject.AddComponent<AudioSource>();
        var eas = EngineAudioSource;
        eas.clip = EngineSound;
        eas.loop = true;
        eas.spatialBlend = 1f;
        eas.playOnAwake = true;
        eas.maxDistance = 50;
        eas.pitch = 0f;
        eas.Play();
    }

    private void SetupCrashAudioSource()
    {
        CrashAudioSource = gameObject.AddComponent<AudioSource>();
        var cas = CrashAudioSource;
        cas.clip = CrashSound;
        cas.maxDistance = 50;
        cas.spatialBlend = 1f;
    }

    void Update()
    {

    }

    public void SetEngineSoundPitch(float value)
    {
        EngineAudioSource.pitch = value;
    }

    public void PlayCrash()
    {
        CrashAudioSource.Play();
    }
}
