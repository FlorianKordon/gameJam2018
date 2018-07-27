using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For AudioFadeScript see https://forum.unity.com/threads/fade-out-audio-source.335031/
public static class AudioFadeScript
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool stop)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        if (stop)
        {
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, bool restart)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;

        if (restart)
            audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }
}

public class SoundController : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioSource nihilismDelusionSource;
    public AudioSource controlDelusionSource;
    public AudioSource hauntedDelusionSource;
    public AudioClip platformRotationSound;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    private bool isPlayingAmbient = true;

    private void Awake()
    {
        nihilismDelusionSource.volume = 0f;
        controlDelusionSource.volume = 0f;
        hauntedDelusionSource.volume = 0f;

        musicSource.loop = nihilismDelusionSource.loop = controlDelusionSource.loop = hauntedDelusionSource.loop = true;
    }

    public void FadeInAmbient()
    {
        if (!isPlayingAmbient)
        {
            isPlayingAmbient = true;
            StartCoroutine(AudioFadeScript.FadeIn(musicSource, 1f, false));
        }
    }

    public void FadeOutAmbient()
    {
        if (isPlayingAmbient)
        {
            isPlayingAmbient = false;
            StartCoroutine(AudioFadeScript.FadeOut(musicSource, 1f, false));
        }
    }

    public void FadeInAudioSource(AudioSource audioSource)
    {
        StartCoroutine(AudioFadeScript.FadeIn(audioSource, 0.5f, true));
    }

    public void FadeOutAudioSource(AudioSource audioSource)
    {
        StartCoroutine(AudioFadeScript.FadeOut(audioSource, 1f, true));
    }

    public void PlaySingleEFX(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlaySingleEFXDelayed(AudioClip clip, float delay)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayMusic(AudioClip clip, bool shouldLoop)
    {
        musicSource.clip = clip;
        musicSource.loop = shouldLoop;
        musicSource.Play();
    }
}