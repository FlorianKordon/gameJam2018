using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloat : MonoBehaviour
{

    private Vector3 startpos;

    //public Transform endpos;
    public Vector3 endpos;


    public float speed;
    public float acceleration;

    private bool inversemovement = true;
    public bool dcb = false;

    // Use this for initialization
    void Start()
    {

        // Instantiate(emptyGameObjectPrefab, transform.position + , Quaternion.identity);

        startpos = transform.localPosition;
        //objToSpawn = new GameObject("Cool GameObject made from Code");

        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        while ((endpos - transform.localPosition).magnitude >= 1f)
        {
            //transform.localPosition = transform.localPosition + (endpos - transform.localPosition) * 0.01f * speed;
            transform.localPosition = Vector3.Slerp(transform.localPosition, endpos, 0.01f * speed);
        }

        yield return new WaitForSeconds(3);

        Debug.Log(startpos - transform.localPosition );

        while ((startpos - transform.localPosition ).magnitude >= 1f)
        {
            //transform.localPosition = transform.localPosition + (startpos - transform.localPosition) * 0.01f * speed;
            transform.localPosition = Vector3.Slerp(transform.localPosition, startpos, 0.01f * speed);
            yield return null;
        }

        yield return new WaitForSeconds(3);
        StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        /* 
        if (inversemovement)
        {
            //Move to endpos
            transform.localPosition = transform.localPosition + (endpos - transform.localPosition) * 0.01f * speed;

            if ((endpos - transform.localPosition).magnitude < 1f)
                Debug.Log("Wechsel startet");

            if ((endpos - transform.localPosition).magnitude < 1f)
            {

                Invoke("Dirchange", 3);
                dcb = !dcb;
                StartCoroutine(DirectionChangeBlock());
            }
        }
        else
        {
            //Move to startpos
            transform.localPosition = transform.localPosition + (startpos - transform.localPosition) * 0.01f * speed;

            if ((startpos - transform.localPosition).magnitude < 1f)
                Invoke("Dirchange", 3);
            dcb = !dcb;
            StartCoroutine(DirectionChangeBlock());
        }*/

    }

    private void DirChange()
    {
        inversemovement = !inversemovement;
    }



    private void DirChangeBlocker()
    {

    }
}
