using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 10.0F;
    public float jumpSpeed = 10.0F;
    public float gravity = 22.0F;
    public float tapThreshold = 0.5F;
    public VirtualJoystick joystick;

    public bool InvertedControls { get; set; }
    public bool InputsDelayed { get; set; }
    public bool InputsDisabled { get; set; }

    private Vector3 jump;
    private Vector3 moveDirection = Vector3.zero;
    private float _tiltZ;

    // INPUT CONTROLS
    private Vector3 _moveDirectionMultiplier = Vector3.zero;
    private float _currentJumpHeight;
    private CharacterController _charController;
    private CircularBuffer<Vector3> _accerleratorHistory = new CircularBuffer<Vector3>(2);
    private CircularBuffer<Vector3> _moveDirectionHistory = new CircularBuffer<Vector3>(40);

    private float _baseSpeed;
    private float _baseJumpSpeed;

    // GLOBAL CONTROLLERS
    private GameLogicController _glc;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        _glc = FindObjectOfType<GameLogicController>();
        _currentJumpHeight = 0;

        _baseSpeed = speed;
        _baseJumpSpeed = jumpSpeed;

        // Add event listeners
        InputsDisabled = InputsDelayed = InvertedControls = false;
        _glc.InputsInvertedEvent += OnInputsInverted;
        _glc.InputsDisabledEvent += OnInputsDisabled;
        _glc.InputsDelayedEvent += OnInputsDelayed;
    }
    //InputsDelayed

    private void Update()
    {
        if (InputsDisabled)
            return;

        // Handle history tracking of accelerometer tilt
        _accerleratorHistory.Add(Input.acceleration);
        Vector3 lastValue = _accerleratorHistory.Peek();
        Vector3 tiltDifference = Input.acceleration - lastValue;
        _tiltZ = Mathf.Abs(tiltDifference.z);

        // Retrieve user joystick input
        moveDirection = joystick.InputDirection;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        moveDirection = Quaternion.Euler(_moveDirectionMultiplier) * moveDirection;

        if (InputsDelayed)
        {
            // Handly moveDirectionHistory
            _moveDirectionHistory.Add(moveDirection);
            moveDirection = _moveDirectionHistory.Peek();

            speed = speed * 0.995f;
            jumpSpeed = jumpSpeed * 0.995f;
        }

        // Check if character is grounded and if inputs are not disabled
        if (_charController.isGrounded)
        {
            CheckForJump();
        }
        else
        {
            //Reduce Speed in Air 1f = no modification
            moveDirection = moveDirection * 1f;
            moveDirection.y = _currentJumpHeight;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        _currentJumpHeight = moveDirection.y;
        _charController.Move(moveDirection * Time.deltaTime);
    }

    private void CheckForJump()
    {
        if (Input.GetButton("Jump"))
            moveDirection.y = jumpSpeed;
        else if (_tiltZ > tapThreshold)
            moveDirection.y = jumpSpeed;
    }

    private void OnInputsInverted(bool inverted)
    {
        if (inverted)
            _moveDirectionMultiplier = new Vector3(0, new System.Random().Next(90, 270), 0);
        else
            _moveDirectionMultiplier = Vector3.zero;
        InvertedControls = inverted;
    }

    private void OnInputsDisabled(bool disabled)
    {
        InputsDisabled = disabled;
    }

    private void OnInputsDelayed(bool delayed)
    {
        if (!delayed)
        {
            speed = _baseSpeed;
            jumpSpeed = _baseJumpSpeed;
        }
        _moveDirectionHistory = new CircularBuffer<Vector3>(40);
        InputsDelayed = delayed;
    }

    private void OnDisable()
    {
        _glc.InputsInvertedEvent -= OnInputsInverted;
        _glc.InputsDisabledEvent -= OnInputsDisabled;
        _glc.InputsDelayedEvent -= OnInputsDelayed;
    }
}

