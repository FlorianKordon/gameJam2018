using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateableCollisionCheck : MonoBehaviour
{
    public bool debugRaycast = true;
    public int raycastDistance = 5;

    private Vector3 raycastDirection = Vector3.down;
    private RaycastHit raycastHit;

    private void Update()
    {
        if (debugRaycast)
            Debug.DrawRay(transform.position, raycastDistance * raycastDirection, Color.yellow);

        if (Physics.Raycast(transform.position, raycastDirection, out raycastHit, raycastDistance))
        {
            // If hit, check for platform locking
            GameObject hitObject = raycastHit.collider.gameObject;
            if (hitObject.tag == "Rotateable")
            {
                //Debug.Log("Hit Rotateable");
               // hitObject.GetComponent<RotateablePlatform>().IsLocked = true;
            }
            else if (raycastHit.collider.transform.parent.tag == "Rotateable")
            {
               // Debug.Log("Hit Rotateable");
               // hitObject.GetComponentInParent<RotateablePlatform>().IsLocked = true;
            }
            else if (hitObject.tag == "Floating")
            {
				transform.parent = hitObject.transform;
            }
        }
        else
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("Rotateable"))
            {
                //RotateablePlatform rotPlatform = go.GetComponent<RotateablePlatform>();
                //if (rotPlatform == null)
                  //  rotPlatform = GetComponentInParent<RotateablePlatform>();
                //rotPlatform.IsLocked = false;
            }

			transform.parent = null;
        }
    }

    // SOMEHOW NOT WORKING
    /* 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        RotateablePlatform rotPlatform = other.gameObject.GetComponent<RotateablePlatform>();

        if (rotPlatform == null)
            rotPlatform = other.gameObject.GetComponentInParent<RotateablePlatform>();

        if (rotPlatform != null)
        {

            rotPlatform.IsLocked = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit");
        RotateablePlatform rotPlatform = other.gameObject.GetComponent<RotateablePlatform>();

        if (rotPlatform == null)
            rotPlatform = other.gameObject.GetComponentInParent<RotateablePlatform>();

        if (rotPlatform != null)
        {
            rotPlatform.IsLocked = false;
        }
    }


    private RotateablePlatform _rotPlatform;

    private void Start()
    {
        _rotPlatform = GetComponent<RotateablePlatform>();
        if (_rotPlatform == null)
            _rotPlatform = GetComponentInParent<RotateablePlatform>();
    }
	*/

}


