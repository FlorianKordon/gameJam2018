using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDelusion : Delusion
{
    private GameLogicController _glc;

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
    }

    public override void DelusionForecast()
    {
        Debug.Log("Vibration Forecast");
        Handheld.Vibrate();
    }

    public override void DelusionContent()
    {
        Debug.Log("NotifyInvertedInputs with bool true");
        _glc.NotifyInvertedInputs(true);
    }

    public override void DelusionCloseDown()
    {
        Debug.Log("NotifyInvertedInputs with bool false");
        _glc.NotifyInvertedInputs(false);
    }
}
