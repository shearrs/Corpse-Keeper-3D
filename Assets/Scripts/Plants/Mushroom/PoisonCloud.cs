using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    public static float Lifespan = 5f;

    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        Invoke(nameof(Die), Lifespan);
    }

    private void Die()
    {
        StartCoroutine(IEFade());
    }

    private IEnumerator IEFade()
    {
        particles.Stop();

        while (particles.particleCount > 0)
            yield return null;

        Destroy(gameObject);
    }
}