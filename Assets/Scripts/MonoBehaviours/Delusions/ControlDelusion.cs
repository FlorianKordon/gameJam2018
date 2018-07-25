using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDelusion : Delusion
{
    private GameLogicController _glc;
    private CameraShake _cs;

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _cs = FindObjectOfType<CameraShake>();
    }

    public override void DelusionForecast()
    {
        Debug.Log("Vibration Forecast");
        Handheld.Vibrate();
    }

    public override void DelusionContent()
    {
        //Debug.Log("NotifyInvertedInputs with bool true");
        // Notify all event listeners in the game logic controller 
        // to utilize inverted controls.
        _glc.NotifyInvertedInputs(true);

        // Start camera shakes
        StartCoroutine(_cs.Shake(Duration, 0.05f));
    }

    public override void DelusionCloseDown()
    {
        // Debug.Log("NotifyInvertedInputs with bool false");
        _glc.NotifyInvertedInputs(false);
    }
}
