using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Active { get; set; }
    public int ActivationNumber { get; set; }

    public static Checkpoint currentActive;

    private void Awake()
    {
        Active = true;
        ActivationNumber = -1;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (Active && collision.gameObject.GetComponent<PlayerMovementController>() != null)
        {
            Active = false;
            currentActive = this;
        }
    }
}
