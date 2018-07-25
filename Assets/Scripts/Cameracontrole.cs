using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControle : MonoBehaviour {

    public GameObject player;
    private Vector3 camdistance;
    private int x = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");

        transform.position = player.transform.position + new Vector3 (-19.5f,16.5f,-20f);

        camdistance =   player.transform.position - transform.position;
        Debug.Log("Camdistance" + camdistance );
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(new Vector3(transform.position.x, transform.position.y, transform.position.z) );


        //transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)+ camdistance;

        transform.position = new Vector3(player.transform.position.x - camdistance.x, player.transform.position.y + 16.5f, player.transform.position.z - camdistance.z);
        //transform.position = new Vector3(player.transform.position.x - camdistance.x, transform.position.y, player.transform.position.z - camdistance.z);
        //rigid.velocity = player.velocity;
    }
}
