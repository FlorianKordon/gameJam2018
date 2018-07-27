using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Delusion : MonoBehaviour
{
    public abstract void DelusionForecast();
    public abstract void DelusionContent();
    public abstract void DelusionCloseDown();

    public int EncounterDelay { get; set; }
    public int Duration { get; set; }
    public int VibrationForecastTime { get; set; }

    public int maxDelay = 60;
    public int minDelay = 45;
    public int minDelayDecrease = 2;
    public int maxDelayDecrease = 5;

    public int maxDuration = 10;
    public int minDuration = 5;

    public bool isCurrentlyActive;

    public SoundController _sc;
    public AudioSource _as;

    private void Awake()
    {
        gameObject.tag = "Delusion";

        _sc = FindObjectOfType<SoundController>();
    }

    public IEnumerator StartDelusion(int encounterDelay)
    {
        // Generate new random value for the duration and handheld vibration forecast time
        System.Random rnd = new System.Random();
        Duration = rnd.Next(minDuration, maxDuration);
        VibrationForecastTime = rnd.Next(1, 3);

        if (encounterDelay > VibrationForecastTime)
        {
            // Wait for the scheduled delay time
            yield return new WaitForSeconds(encounterDelay - VibrationForecastTime);
            DelusionForecast();

            // After waiting for the scheduled delay time, execute delusion content
            yield return new WaitForSeconds(VibrationForecastTime);
        }
        else if (encounterDelay > 0)
        {
            yield return new WaitForSeconds(VibrationForecastTime);
        }

        // Play delusion content
        DelusionContent();

        // FadeIn delusion audio cue and fade out ambient
        _sc.FadeInAudioSource(_as);

        isCurrentlyActive = true;

        // For stopping the delusion, wait for the specified duration
        yield return new WaitForSeconds(Duration);
        StopDelusion();
        isCurrentlyActive = false;
    }

    public void StopDelusion()
    {
        // Call clean up code for dilusion
        DelusionCloseDown();
        // Fade out delusion audio cue
        _sc.FadeOutAudioSource(_as);

        // Generate new random value for delay decrease
        System.Random rnd = new System.Random();
        int currentDelayDecrease = rnd.Next(minDelayDecrease, maxDelayDecrease);

        // Recursivly start delusion again with a little decease in the delay.
        StartCoroutine(StartDelusion(EncounterDelay - currentDelayDecrease));
    }
}
