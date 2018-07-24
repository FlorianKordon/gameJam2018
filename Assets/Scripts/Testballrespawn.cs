using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Testballrespawn : MonoBehaviour {
    private Vector3 respawn;
	// Use this for initialization
	void Start () {
        respawn = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y <= -3)
        {
            transform.position = respawn;
        }
    }
}
