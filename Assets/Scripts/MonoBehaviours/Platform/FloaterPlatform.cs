using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterPlatform : Platform
{
    public Vector3 endpos;
    public float speed;
    public float floatWaitTime = 3f;

    private Vector3 startpos;

    // Use this for initialization
    void Start()
    {
        // Instantiate(emptyGameObjectPrefab, transform.position + , Quaternion.identity);
        speed = 1;
        startpos = transform.localPosition;

        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        while ((endpos - transform.localPosition).magnitude >= 0.01f)
        {
            //transform.localPosition = transform.localPosition + (endpos - transform.localPosition) * 0.01f * speed;
            transform.localPosition = Vector3.Slerp(transform.localPosition, endpos, 0.01f * speed);
            yield return null;
        }

        yield return new WaitForSeconds(floatWaitTime);

        while ((startpos - transform.localPosition).magnitude >= 0.01f)
        {
            //transform.localPosition = transform.localPosition + (startpos - transform.localPosition) * 0.01f * speed;
            transform.localPosition = Vector3.Slerp(transform.localPosition, startpos, 0.01f * speed);
            yield return null;
        }

        yield return new WaitForSeconds(floatWaitTime);
        StartCoroutine(Movement());
    }
}