using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cameracontrole : MonoBehaviour {

    public GameObject player;
    private Vector3 camdistance;
    private int x = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        camdistance =   player.transform.position - transform.position;
        Debug.Log("Camdistance" + camdistance );
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(new Vector3(transform.position.x, transform.position.y, transform.position.z) );


        //transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)+ camdistance;

        transform.position = new Vector3(player.transform.position.x - camdistance.x, transform.position.y, player.transform.position.z - camdistance.z);
        //rigid.velocity = player.velocity;
    }
}
