using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource efxSource;                   
    public AudioSource musicSource;                 

    public float lowPitchRange = .95f;              
    public float highPitchRange = 1.05f;            

    /// <summary>
	/// Play single sound audioclip without random pitch.
	/// </summary>
    public void PlaySingleEFX(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

	/// <summary>
	/// Play single sound audioclip without random pitch.
	/// </summary>
    public void PlaySingleEFXDelayed(AudioClip clip, float delay)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    /// <summary> 
	/// Choose audio clip at random and change the pitch by a small margin
	/// </summary>
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    /// <summary> 
	/// Choose audio clip and play it
	/// </summary>
    public void PlayMusic(AudioClip clip, bool shouldLoop)
    {
        musicSource.clip = clip;
        musicSource.loop = shouldLoop;
        musicSource.Play();
    }

	
}