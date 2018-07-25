using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControle : MonoBehaviour
{
    public GameObject player;

    private Vector3 camdistance;
    private int x = 0;

    void Awake()
    {
        transform.position = player.transform.position + new Vector3(-19.5f, 16.5f, -20f);
        camdistance = player.transform.position - transform.position;
        Debug.Log("Camdistance" + camdistance);
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - camdistance.x, transform.position.y, player.transform.position.z - camdistance.z);
    }
}
