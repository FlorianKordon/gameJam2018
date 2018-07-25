using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate bool InputsInvertedDelegate(bool inverted);

public class GameLogicController : MonoBehaviour
{
    public event System.Action<bool> InputsInvertedEvent;

    public event System.Action PlayerDiedEvent;

    public void NotifyInvertedInputs(bool inverted)
    {
        InputsInvertedEvent(inverted);
    }

    public void NotifyPlayerDeath()
    {
        PlayerDiedEvent();
    }
}
