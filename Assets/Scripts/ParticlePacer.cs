using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePacer : MonoBehaviour
{
    private ParticleSystem particles;
    private float randomTime;
    IEnumerator EmitParticles()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomTime);

            for (int i = 0; i < 4; i++)
            {
                particles.Emit(1);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        randomTime = Random.Range(3.0f, 10.0f);

        particles = GetComponent<ParticleSystem>();
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        StartCoroutine(EmitParticles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
