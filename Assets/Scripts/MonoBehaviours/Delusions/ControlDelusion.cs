using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Delusion
{
    private GameLogicController _glc;

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
    }
    public override void DelusionContent()
    {
        _glc.NotifyInvertedInputs(true);
    }

    public override void DelusionCloseDown()
    {
        _glc.NotifyInvertedInputs(false);
    }
}
