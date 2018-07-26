using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatformCollider : MonoBehaviour
{

	public GameObject camera; 

	void OnTriggerEnter(Collider collision)
	{
		camera.GetComponent<ViewingPlatform>().viewing = true;
	}
}
