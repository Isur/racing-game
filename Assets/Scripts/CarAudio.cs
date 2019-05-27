using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioClip EngineSound;

    private AudioSource EngineAudioSource { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        EngineAudioSource = gameObject.AddComponent<AudioSource>();
        EngineAudioSource.clip = EngineSound;
        EngineAudioSource.loop = true;
        EngineAudioSource.playOnAwake = true;
        EngineAudioSource.Play();
    }

    void Update()
    {

    }

    public void SetEngineSoundPitch(float value)
    {
        EngineAudioSource.pitch = value;
    }
}
