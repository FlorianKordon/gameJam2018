using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpSpeed;
    public float gravity;
    private Vector3 jump;
    private Vector3 moveDirection = Vector3.zero;
    public VirtualJoystick joystick;
    private float _tiltZ;

    public float tapThreshold;
    private float yy;
    // Use this for initialization

	CircularBuffer<Vector3> AccHistory = new CircularBuffer<Vector3>(2);

    void Start()
    {
        speed = 10.0F;
        jumpSpeed = 10.0F;
        gravity = 22.0F;
        yy = 0;
        tapThreshold = 0.5F;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();


	{
		AccHistory.Add(Input.acceleration);
		Vector3 lastValue = AccHistory.Peek();
		Vector3 tiltDifference = Input.acceleration - lastValue;
		_tiltZ = Mathf.Abs(tiltDifference.z);
	}


        moveDirection = joystick.InputDirection;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if (controller.isGrounded)
        {
            //if (Input.GetButton("Jump"))
            //    moveDirection.y = jumpSpeed;
            //else 
            
            if (_tiltZ > tapThreshold)
                moveDirection.y = jumpSpeed;
        }
        else
        {
            //Reduce Speed in Air 1f = no modification
            moveDirection = moveDirection * 1f;
            moveDirection.y = yy;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        yy = moveDirection.y;
        //Debug.Log("Movedirection y = " + moveDirection.y);
        controller.Move(moveDirection * Time.deltaTime);
    }



}

