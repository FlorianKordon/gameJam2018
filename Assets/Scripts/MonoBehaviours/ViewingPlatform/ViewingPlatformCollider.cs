using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatformCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        GetComponentInParent<ViewingPlatformController>().viewing = true;
    }
}
