using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyAfterParticles : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public ParticleSystem Particles => _particleSystem ?? (_particleSystem = GetComponent<ParticleSystem>());

    // Update is called once per frame
    void Update()
    {
        if (Particles.particleCount <= 0)
            Destroy(gameObject);
    }
}
