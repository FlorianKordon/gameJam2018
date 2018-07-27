using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCollider : MonoBehaviour
{
    // The GameObject needs a BoxCollider-Component width 
    // greater Height and half of the GameObject-Width separatly.
    // See also the Prototype-Scene.

    private SceneController _sc;

    private void Start()
    {
        _sc = FindObjectOfType<SceneController>();
    }

    void OnTriggerEnter()
    {
        _sc.FadeAndLoadScene("Level1");
    }
}
