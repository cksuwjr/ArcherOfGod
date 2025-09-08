using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>,IManager
{
    private SoundObject BGMAudioObject;
    private float current, percent;

    public AudioClip BGM;

    private Pool soundPool;

    public void Init()
    {
        transform.GetChild(0).TryGetComponent<Pool>(out soundPool);


        if(BGM)
            ChangeBGM(BGM);
    }

    public void ChangeBGM(AudioClip audioClip)
    {
        if (!BGMAudioObject)
        {
            BGMAudioObject = PlaySound(audioClip, true);
            BGMAudioObject.AudioSource.loop = true;
            BGMAudioObject.AudioSource.pitch = 0.8f;
            BGMAudioObject.name = "BGM Object";
        }
        else
            StartCoroutine("ChangeBGMClip", audioClip);
    }

    public SoundObject PlaySound(AudioClip audioClip, bool loop = false)
    {
        if (audioClip == null) return null;

        if (soundPool.GetPoolObject().TryGetComponent<SoundObject>(out SoundObject soundObject))
        {
            soundObject.Init(audioClip, loop);
            return soundObject;
        }
        return null;
    }

    IEnumerator ChangeBGMClip(AudioClip newClip)
    {
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        BGMAudioObject.AudioSource.clip = newClip;
        BGMAudioObject.AudioSource.Play();
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }
    }
}