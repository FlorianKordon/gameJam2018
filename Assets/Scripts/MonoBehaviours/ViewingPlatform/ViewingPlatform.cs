using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatform : MonoBehaviour
{
	public Transform from;
	public Transform to;
	public GameObject target;
	public GameObject playerCharacter;
	public GameObject resetPosition;

	public float interpolationSpeed;
	public bool viewing;

	private Quaternion _rotation;
	private Transform _currentView;
	


	// Use this for initialization
	void Start()
	{
		playerCharacter.GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		if (viewing)
		{
			_currentView = to;
			_rotation = Quaternion.LookRotation(target.transform.position - _currentView.position, Vector3.up);
			StartCoroutine(Waiting());

			viewing = false;
		}
	}

	void LateUpdate()
	{
		if (_currentView == null)
			return;

		// Animates the position between a and b looking to target
		transform.position = Vector3.Slerp(transform.position, _currentView.position, Time.deltaTime * interpolationSpeed);
		Quaternion currentRotation = Quaternion.Slerp(transform.localRotation, _rotation, Time.deltaTime * interpolationSpeed);

		transform.localRotation = currentRotation;
	}

	IEnumerator Waiting()
	{
		yield return new WaitForSeconds(2);
		_currentView = from;
		_rotation = from.transform.rotation;

		playerCharacter.transform.position = resetPosition.transform.position;
	}
}
