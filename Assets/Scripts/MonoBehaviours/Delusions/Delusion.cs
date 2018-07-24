using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Delusion : MonoBehaviour
{
    public abstract void DelusionContent();
    public abstract void DelusionCloseDown();

    public int EncounterDelay { get; set; }
    public int Duration { get; set; }

    public int maxDelay = 60;
    public int minDelay = 45;
    public int delayDecrease = 2;

    public int maxDuration = 10;
    public int minDuration = 5;

    public bool isCurrentlyActive;

    private void Awake()
    {
        gameObject.tag = "Delusion";
    }

    public IEnumerator StartDelusion(int encounterDelay)
    {
        // Wait for the scheduled delay time
        yield return new WaitForSeconds(encounterDelay);

        // After waiting for the scheduled delay time, execute delusion content
        DelusionContent();

        // Generate new random value for the duration
        System.Random rnd = new System.Random();
        Duration = rnd.Next(minDuration, maxDuration);
        // For stopping the delusion, wait for the specified duration
        yield return new WaitForSeconds(Duration);
        StopDelusion();
    }

    public void StopDelusion()
    {
        // Call clean up code for dilusion
        DelusionCloseDown();

        // Recursivly start delusion again with a little decease in the delay.
        StartCoroutine(StartDelusion(EncounterDelay - delayDecrease));
    }
}
