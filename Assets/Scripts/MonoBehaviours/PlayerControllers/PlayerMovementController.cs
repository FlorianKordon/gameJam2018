using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public bool Alive { get; set; }
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

    private Animator _animator;
    private int _animatorSpeedHashParam;

    // INPUT CONTROLS
    private Vector3 _moveDirectionMultiplier = Vector3.zero;
    private float _currentJumpHeight;
    private CharacterController _charController;
    private CircularBuffer<Vector3> _accerleratorHistory = new CircularBuffer<Vector3>(2);
    private CircularBuffer<Vector3> _moveDirectionHistory = new CircularBuffer<Vector3>(40);

    private float _baseSpeed;
    private float _baseJumpSpeed;
    private const float _fallingSpeedThreshold = -30f;
    private Vector3 _lastMoveDirection = Vector3.zero;

    // GLOBAL CONTROLLERS
    private GameLogicController _glc;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        _glc = FindObjectOfType<GameLogicController>();

        _animator = GetComponent<Animator>();
        _animatorSpeedHashParam = Animator.StringToHash("Speed");

        _currentJumpHeight = 0;

        _baseSpeed = speed;
        _baseJumpSpeed = jumpSpeed;

        // Add event listeners
        InputsDisabled = InputsDelayed = InvertedControls = false;
        _glc.InputsInvertedEvent += OnInputsInverted;
        _glc.InputsDisabledEvent += OnInputsDisabled;
        _glc.InputsDelayedEvent += OnInputsDelayed;

        Alive = true;
    }
    //InputsDelayed

    private void Update()
    {
        if (InputsDisabled || !Alive)
            return;

        CheckForFalling();

        // Handle history tracking of accelerometer tilt
        _accerleratorHistory.Add(Input.acceleration);
        Vector3 lastValue = _accerleratorHistory.Peek();
        Vector3 tiltDifference = Input.acceleration - lastValue;
        _tiltZ = Mathf.Abs(tiltDifference.z);

        // Retrieve user joystick input
        moveDirection = joystick.InputDirection;
        moveDirection *= speed;
        moveDirection = Quaternion.Euler(_moveDirectionMultiplier) * moveDirection;

        if (moveDirection.magnitude != 0)
            _lastMoveDirection = moveDirection;

        transform.rotation = Quaternion.LookRotation(_lastMoveDirection);

        // Notify animator about current speed
        _animator.SetFloat(_animatorSpeedHashParam, moveDirection.magnitude);

        if (InputsDelayed)
        {
            // Handly moveDirectionHistory
            _moveDirectionHistory.Add(moveDirection);
            moveDirection = _moveDirectionHistory.Peek();

            if (moveDirection.magnitude != 0)
                _lastMoveDirection = moveDirection;

            transform.rotation = Quaternion.LookRotation(_lastMoveDirection);

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
        //transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    private void CheckForFalling()
    {
        if (Alive && (_charController.velocity.y < _fallingSpeedThreshold))
        {
            Alive = false;
            _glc.NotifyPlayerDeath();
        }
    }

    private void CheckForJump()
    {
        if (Input.GetButton("Jump") || _tiltZ > tapThreshold)
        {
            moveDirection.y = jumpSpeed;
            _animator.SetTrigger("Jump");
        }
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
        Debug.Log("Event recieved: " + disabled);
        InputsDisabled = disabled;

        // set current speed to 0 in animator
        _animator.SetFloat(_animatorSpeedHashParam, 0);
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

