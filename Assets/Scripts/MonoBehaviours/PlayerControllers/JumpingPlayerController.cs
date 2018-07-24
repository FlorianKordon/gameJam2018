using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayerController : MonoBehaviour
{

	public float speed;
	public float jumpSpeed;
	public float gravity;
	public float tapThreshold;

	private Vector3 _moveDirection;
	// private bool _jump;
	private float _tiltZ;

	CircularBuffer<Vector3> AccHistory = new CircularBuffer<Vector3>(2);

	// Use this for initialization
	void Start()
	{
		speed = 6.0F;
		jumpSpeed = 8.0F;
		gravity = 20.0F;
		tapThreshold = 0.7F;
		_moveDirection = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{		
		Acceleration();
		Controller();
	}

	void Acceleration()
	{
		AccHistory.Add(Input.acceleration);
		Vector3 lastValue = AccHistory.Peek();
		Vector3 tiltDifference = Input.acceleration - lastValue;
		_tiltZ = Mathf.Abs(tiltDifference.z);
	}

	void Controller()
	{
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded)
		{
			_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			_moveDirection = transform.TransformDirection(_moveDirection);
			_moveDirection *= speed;
			if (_tiltZ > tapThreshold)
				_moveDirection.y = jumpSpeed;
		}
		_moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(_moveDirection * Time.deltaTime);
	}
}
