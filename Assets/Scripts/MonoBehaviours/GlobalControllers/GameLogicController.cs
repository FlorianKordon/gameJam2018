using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate bool InputsInvertedDelegate(bool inverted);

public class GameLogicController : MonoBehaviour
{
    public event System.Action<bool> DelusionActivityEvent;

    public event System.Action<bool> InputsInvertedEvent;

    public event System.Action<bool> InputsDelayedEvent;

    public event System.Action<bool> InputsDisabledEvent;

    public event System.Action PlayerDiedEvent;

    public bool DelusionActive
    {
        get
        {
            return _delusionActive;
        }
        set
        {
            // If the value to set is different from the current value, fire the event that a change has happened.
            // (We only want to inform if the overall value has changed)
            if (value != _delusionActive && DelusionActivityEvent != null)
                DelusionActivityEvent(value);

            _delusionActive = value;
        }
    }

    private bool _delusionActive;

    private bool _inputsInverted;
    private bool _inputsDelayed;
    private bool _inputsDisabled;

    public void NotifyInvertedInputs(bool inverted)
    {
        _inputsInverted = inverted;
        DelusionActive = _inputsInverted || _inputsDelayed || _inputsDisabled;
        if (InputsInvertedEvent != null)
        {
            _inputsInverted = inverted;
            InputsInvertedEvent(inverted);
        }
    }

    public void NotifyDelayedInputs(bool delayed)
    {
        _inputsDelayed = delayed;
        DelusionActive = _inputsInverted || _inputsDelayed || _inputsDisabled;
        if (InputsDelayedEvent != null)
        {
            _inputsDelayed = delayed;
            InputsDelayedEvent(delayed);
        }
    }

    public void NotifyDisabledInputs(bool disabled)
    {
        _inputsDisabled = disabled;
        DelusionActive = _inputsInverted || _inputsDelayed || _inputsDisabled;
        if (InputsDisabledEvent != null)
        {
            InputsDisabledEvent(disabled);
        }
    }

    public void NotifyPlayerDeath()
    {
        if (PlayerDiedEvent != null)
            PlayerDiedEvent();
    }
}
