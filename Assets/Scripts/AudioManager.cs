using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip handSound;
    [SerializeField] private AudioClip cutSound;
    [SerializeField] private AudioClip flashlightSound;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip plantHurtSound;

    public static AudioClip ButtonSound => Instance.buttonSound;
    public static AudioClip HandSound => Instance.handSound;
    public static AudioClip CutSound => Instance.cutSound;
    public static AudioClip FlashlightSound => Instance.flashlightSound;
    public static AudioClip StepSound => Instance.stepSound;
    public static AudioClip PlantHurtSound => Instance.plantHurtSound;

    private void Start()
    {
        musicSource.clip = backgroundSound;
        musicSource.Play();
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            transform.position = Camera.main.transform.position;
        }
    }

    public static void PlayButtonSound()
    {
        AudioSource source = Instance.effectSource;
        source.pitch = Random.Range(0.75f, 1f);
        source.PlayOneShot(ButtonSound);
    }

    public static void PlaySound(AudioClip clip, float lowRange = 1, float highRange = 1, float volume = 1, float delay = -1)
    {
        AudioSource source = Instance.effectSource;

        if (delay > 0)
        {
            Instance.StartCoroutine(Instance.IEPlaySound(clip, lowRange, highRange, volume, delay));
        }    
        else
        {
            source.pitch = Random.Range(lowRange, highRange);
            source.volume = volume;
            source.PlayOneShot(clip);
        }
    }

    private IEnumerator IEPlaySound(AudioClip clip, float lowRange, float highRange, float volume, float delay)
    {
        float elapsedTime = 0;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectSource.pitch = Random.Range(lowRange, highRange);
        effectSource.volume = volume;
        effectSource.PlayOneShot(clip);
    }
}
