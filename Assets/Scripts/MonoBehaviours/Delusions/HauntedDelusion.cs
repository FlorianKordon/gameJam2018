using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntedDelusion : Delusion
{
    public PersecutorAgent persecutor;

    private void Start()
    {
        _as = _sc.hauntedDelusionSource;
    }

    public override void DelusionForecast()
    {
        //Debug.Log("Vibration Forecast");
        Handheld.Vibrate();
    }

    public override void DelusionContent()
    {
        // Debug.Log("Start Hunting");
        persecutor.SpawnAndHaunt();
    }

    public override void DelusionCloseDown()
    {
        //Debug.Log("Stop Hunting");
        persecutor.StopHaunting();
    }
}
