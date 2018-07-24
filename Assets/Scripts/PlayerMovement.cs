using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float jumpSpeed;
    public float gravity;
    private Vector3 _moveDirection;
    public VirtualJoystick joystick;
    // Use this for initialization
    void Start () {
        _moveDirection = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        Controller();
	}

    void Controller()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            //_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = joystick.InputDirection;
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= speed;
            //if (_tiltZ > tapThreshold)
            //    _moveDirection.y = jumpSpeed;
        }
        _moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(_moveDirection * Time.deltaTime);
    }
}

