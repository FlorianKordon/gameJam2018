using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate bool InputsInvertedDelegate(bool inverted);

public class GameLogicController : MonoBehaviour
{
    public event InputsInvertedDelegate InputsInvertedEvent;

    public void NotifyInvertedInputs(bool inverted)
    {
        InputsInvertedEvent(inverted);
    }
}
