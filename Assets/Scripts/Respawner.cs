using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Respawner : MonoBehaviour {
    private Vector3 respawn;
	// Use this for initialization
	void Start () {
        respawn = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y <= respawn.y -10)
        {
            transform.position = respawn;
        }
    }
}
