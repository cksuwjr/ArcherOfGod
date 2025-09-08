using UnityEngine;

public class SoundObject : PoolObject
{
    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    private void Awake()
    {
        TryGetComponent<AudioSource>(out audioSource);
    }

    public void Init(AudioClip clip, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.Play();
        if(!loop)
            Invoke("ReturnToPool", audioSource.clip.length);
    }
}