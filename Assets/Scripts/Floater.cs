using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Transform EndPositionTransform;
    public float speed;

    private Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.localPosition;
        StartCoroutine(MovementDir());
    }

    IEnumerator MovementDir()
    {
        //yield return new WaitForSeconds(1);
        while (Vector3.Distance(EndPositionTransform.localPosition, transform.localPosition) >= 0.5f)
        {
            Debug.Log("Endposition: " + EndPositionTransform.localPosition);
            Debug.Log("Cube Position: " + transform.localPosition);
            transform.localPosition = Vector3.Slerp(transform.localPosition, EndPositionTransform.localPosition, 0.01f);
            yield return null;
        }
        //yield return new WaitForSeconds(1);

        while (Vector3.Distance(startPosition, transform.localPosition) >= 0.5f)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, startPosition, 0.01f);
            yield return null;
        }
        StartCoroutine(MovementDir());
    }
}
