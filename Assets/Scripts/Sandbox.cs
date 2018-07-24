using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox : MonoBehaviour
{    
    private GameLogicController _glc;
    // Use this for initialization
    void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _glc.InputsInvertedEvent += (bool inverted) => Debug.Log(inverted);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _glc.NotifyInvertedInputs(true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _glc.NotifyInvertedInputs(false);
        }
    }
}
