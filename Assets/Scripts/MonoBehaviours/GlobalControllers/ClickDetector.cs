using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100)) // or whatever range, if applicable
            {
                GameObject go = hit.transform.gameObject;
                if (hit.transform.gameObject.tag == "Rotateable")
                {
                    go.GetComponent<RotateablePlatform>().OnInteraction();
                }
            }
            else
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Rotateable"))
                {
                    item.GetComponent<RotateablePlatform>().IsActivated = false;
                    item.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}