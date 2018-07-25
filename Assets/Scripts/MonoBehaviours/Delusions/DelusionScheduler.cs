﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DelusionScheduler : MonoBehaviour
{
    public List<Delusion> delusions = new List<Delusion>();

    private void Start()
    {
        PopulateDelusionTimings(0);
        ScheduleDelusionStarts();
    }

    private void PopulateDelusionTimings(int delayReduction)
    {
        System.Random rnd = new System.Random();
        foreach (Delusion del in delusions)
        {
            // set encounter delay to random value in specified range
            del.EncounterDelay = rnd.Next(del.minDelay - delayReduction, del.maxDelay - delayReduction);
        }
    }
    private void ScheduleDelusionStarts()
    {
        foreach (Delusion del in delusions)
        {
            StartCoroutine(del.StartDelusion(del.EncounterDelay));
        }
    }
}