using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection
{
    CLOCKWISE,
    COUNTERCLOCKWISE
}


public class RotateablePlatform : Platform
{
    // Platform utility
    public bool IsActivated { get; set; }
    public float flippingThresholdAccelerometer = 1f;
    public float flippingThresholdGyroX = 8f;
    public float flippingThresholdGyroY = 4f;
    public float rotationDuration = 1f;
    public float rotationSpeed = 5f;
    public int rotationValue = 90;
    public Vector3 rotationAxis = new Vector3(0, 1, 0);

    // Platform 
    private bool isRotating = false;
    private Vector3 baseTilt;
    //private Quaternion baseRotation;
    private int snapInAngle = 10;
    private RotationDirection rotationDirection = RotationDirection.CLOCKWISE;

    //
    ////////////////
    // Accelerometer/Gyro support

    // Calibration
    public bool gyroEnabled = true;
    public bool accelerometerEnabled = false;


    private void Awake()
    {
        IsRotatable = true;
        IsActivated = false;

        gameObject.tag = "Rotateable";
    }

    private IEnumerator Start()
    {
        // If the system does not support the accelerometer, end update routine
        if (!SystemInfo.supportsGyroscope)
        {
            gyroEnabled = false;
            yield break;
        }
        gyroEnabled = true;
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        Debug.Log(Input.gyro.attitude);
        // If platform is activated and is not currently rotating, start rotate routine
        if (IsActivated && !isRotating && (accelerometerEnabled || gyroEnabled))
        {
            if (gyroEnabled && CheckGyroMobileFlipGesture())
            {
                StartCoroutine(RotatePlatform(rotationValue, rotationAxis, rotationDirection));
            }
            else if (accelerometerEnabled && CheckAccelerometerMobileFlipGesture())
                StartCoroutine(RotatePlatform(rotationValue, rotationAxis, RotationDirection.CLOCKWISE));
        }

        if (IsActivated && Input.GetKeyDown(KeyCode.Z) && !isRotating)
        {
            Debug.Log("Starting Coroutine");
            StartCoroutine(RotatePlatform(rotationValue, rotationAxis, RotationDirection.CLOCKWISE));
        }
    }

    public void OnInteraction()
    {
        Debug.Log("OnInteraction triggered!");

        // When the platform currently is activated, an additional click should deactivate it;
        // If the platform currently is not activated, we want to activate it for rotation.
        IsActivated = !IsActivated;
        Debug.Log("IsActivated: " + IsActivated);

        if (IsActivated)
        {
            if (accelerometerEnabled)
                baseTilt = Input.acceleration;
        }
    }

    private bool CheckAccelerometerMobileFlipGesture()
    {
        Vector3 currentTiltDifference = baseTilt - Input.acceleration;
        //Debug.Log("X: " + currentTiltDifference.x + "Y: " + currentTiltDifference.y + "Z: " + currentTiltDifference.z);

        // If flipping is detected
        if (Mathf.Abs(currentTiltDifference.y) > flippingThresholdAccelerometer && Mathf.Abs(currentTiltDifference.z) > flippingThresholdAccelerometer)
        {
            // set isRotating to true, so that for the duraction any further flipping is disabled
            //isRotating = true;
            Debug.Log("Flipping detected");
            return true;
        }
        else
            return false;
    }

    private bool CheckGyroMobileFlipGesture()
    {
        Vector3 currentTiltDifference = Input.gyro.rotationRate;
        //Debug.Log(currentTiltDifference);

        // If flipping is detected, return true
        if (Vector3.Equals(rotationAxis, new Vector3(0, 1, 0)))
        {
            if (currentTiltDifference.y < flippingThresholdGyroY)
                rotationDirection = RotationDirection.CLOCKWISE;
            else
                rotationDirection = RotationDirection.COUNTERCLOCKWISE;

            return Mathf.Abs(currentTiltDifference.y) >= flippingThresholdGyroY;
        }
        else
            if (currentTiltDifference.x < flippingThresholdGyroX)
            rotationDirection = RotationDirection.CLOCKWISE;
        else
            rotationDirection = RotationDirection.COUNTERCLOCKWISE;
        return Mathf.Abs(currentTiltDifference.x) >= flippingThresholdGyroX;
    }

    private IEnumerator RotatePlatform(float rotationValue, Vector3 rotationAxis, RotationDirection rotationDirection)
    {
        isRotating = true;

        float currentRotation = Vector3.Dot(transform.localEulerAngles, rotationAxis);

        // Adjust positivity/negativity of rotation according to specified rotationdirection
        switch (rotationDirection)
        {
            case RotationDirection.CLOCKWISE:
                rotationValue = Mathf.Abs(rotationValue);
                snapInAngle = Mathf.Abs(snapInAngle);
                break;
            case RotationDirection.COUNTERCLOCKWISE:
                if (rotationValue > 0)
                    rotationValue = -rotationValue;
                if (snapInAngle > 0)
                    snapInAngle = -snapInAngle;
                break;
        }

        // Compute the new targetRotation by rotating about given rotationValue around given rotation axis
        // Add additional small value to lateron create snap in effect
        Quaternion targetRotation = Quaternion.AngleAxis(currentRotation + rotationValue + snapInAngle, rotationAxis);

        // Overshoot target rotation by snapInAngle
        while (Quaternion.Angle(transform.rotation, targetRotation) >= 5)
        {
            transform.rotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Come back to original target rotation
        targetRotation = Quaternion.AngleAxis(currentRotation + rotationValue, rotationAxis);
        while (Quaternion.Angle(transform.rotation, targetRotation) >= 1E-5)
        {
            transform.rotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localRotation = (targetRotation);
        Debug.Log("Rotation finished");
        isRotating = false;
    }
}
