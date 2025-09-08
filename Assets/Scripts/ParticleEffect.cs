using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : PoolObject
{
    private ParticleSystem particle;

    private void Awake()
    {
        TryGetComponent<ParticleSystem>(out particle);
    }

    public void Init()
    {
        particle.Play();
        Invoke("Dissappear", 1);
    }

    private void Dissappear()
    {
        ReturnToPool();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
