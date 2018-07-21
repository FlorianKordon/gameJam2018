using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.

    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

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