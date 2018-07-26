using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCollider : MonoBehaviour
{
	// The GameObject needs a BoxCollider-Component width 
	// greater Height and half of the GameObject-Width separatly.
	// See also the Prototype-Scene.

	void OnTriggerEnter()
	{
		Debug.Log("YOU WIN!");
		// Do GlobalGameState Stuff
	}
}
