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
    private Quaternion baseRotation;
    private int snapInAngle = 10;
    private RotationDirection rotationDirection = RotationDirection.CLOCKWISE;

    //
    ////////////////
    // Accelerometer/Gyro support

    // Calibration
    public bool gyroEnabled = true;
    public bool accelerometerEnabled = false;
    public bool useYAngleCalibration = true;
    public bool useXAngleCalibration = false;
    public bool useMicrophoneInput = true;

    // State holding
    private float initialYAngle = 0f;
    private float initialXAngle = 0f;
    private float appliedGyroYAngle = 0f;
    private float appliedGyroXAngle = 0f;
    private float calibrationYAngle = 0f;
    private float calibrationXAngle = 0f;
    private float smoothingFactor = 0.1f;
    private float tempSmoothingFactor = 0.0f;
    private const float setupWaitingTime = 0.1f;
    private Transform rawGyroRotation;

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
        // If gyro is supported, we assume that there is a mic too
        // Initialize gyroscope
        InitializeGyro();

        // Wait until gyro is active, then calibrate to reset starting rotation.
        yield return new WaitForSeconds(setupWaitingTime);

        // As we don't need to wait for the calibration, we can omit the yield return here
        StartCoroutine(CalibrateGyroAngels());
    }

    private void Update()
    {
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

            if (gyroEnabled)
                baseRotation = rawGyroRotation.rotation;
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
        Debug.Log(currentTiltDifference);

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
        while (Quaternion.Angle(transform.rotation, targetRotation) >= 1E-16)
        {
            transform.rotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localRotation = (targetRotation);
        Debug.Log("Rotation finished");
        isRotating = false;
    }

    ///////////////////////////////////////////////////
    // Gyro, calibration adapted from https://gist.github.com/kormyen/a1e3c144a30fc26393f14f09989f03e1 
    // CURRENTLY NOT USED
    private void InitializeGyro()
    {
        gyroEnabled = true;
        Input.gyro.enabled = true;

        // In order to recalibrate our camera position, we need a reference to the initial x and y values.
        initialYAngle = transform.eulerAngles.y;
        initialXAngle = transform.eulerAngles.x;

        // Initialize our transform object where we do the rotation calculations on
        rawGyroRotation = new GameObject("GyroRaw").transform;
        rawGyroRotation.position = transform.position;
        rawGyroRotation.rotation = transform.rotation;
    }

    private IEnumerator CalibrateGyroAngels()
    {
        if (!useYAngleCalibration && !useXAngleCalibration)
            yield break;

        tempSmoothingFactor = smoothingFactor;

        // As we want to directly set our calibrated value without any slerping going on, we set our factor to 1
        smoothingFactor = 1;

        // Offsets the y angle in case it wasn't 0 at edit time.
        if (useYAngleCalibration)
            calibrationYAngle = appliedGyroYAngle - initialYAngle;

        // Offsets the x angle in case it wasn't 0 at edit time.
        if (useXAngleCalibration)
            calibrationXAngle = appliedGyroXAngle - initialXAngle;

        // Wait for one frame, then continue
        yield return null;

        // After the frame, we want to reset our smoothing factor
        smoothingFactor = tempSmoothingFactor;
    }

    private void ApplyGyroRotation()
    {
        rawGyroRotation.rotation = Input.gyro.attitude;
        // Swap "handedness" of quaternion from gyro, as the gyro is right-handed, Unity is left-handed.
        rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self);
        // Rotate to make sense as a camera pointing out the back of the device.
        rawGyroRotation.Rotate(90f, 180f, 0f, Space.World);
        // Save the angle around y axis for use in calibration.
        appliedGyroYAngle = rawGyroRotation.eulerAngles.y;
        appliedGyroXAngle = rawGyroRotation.eulerAngles.x;
    }

    private void ApplyCalibration()
    {
        rawGyroRotation.Rotate(0f, -calibrationYAngle, 0f, Space.World);
        rawGyroRotation.Rotate(-calibrationXAngle, 0f, 0f, Space.World);
    }
    // CURRENTLY NOT USED
    ///////////////////////////////////////////////////
}
