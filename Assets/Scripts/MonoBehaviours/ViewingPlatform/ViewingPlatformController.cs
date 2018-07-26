using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatformController : MonoBehaviour
{
    public Transform to;
    public Transform target;
    public Transform resetPosition;
    public Transform playerCharacter;
    public Camera sceneCamera;
    public float viewingTime = 5;
    public float interpolationSpeed = 2;
    public bool orthographicLerpMode = true;
    public float zoomOutFactor = 2;

    public bool viewing;

    private Quaternion _rotation;
    private Transform _currentTarget;

    private Quaternion fromRotation;
    private CameraOffset _sceneCameraOffset;

    private int moveDirection = 1;
    private float baseOrthographicSize;
    private float movementTimePassed = 0;

    private Vector3 movementStartPosition;

    // Global Controllers
    private GameLogicController _glc;

    private void Awake()
    {
        _glc = FindObjectOfType<GameLogicController>();
    }

    private void Start()
    {
        _sceneCameraOffset = sceneCamera.GetComponent<CameraOffset>();
        baseOrthographicSize = sceneCamera.orthographicSize;
    }

    private void Update()
    {
        if (viewing)
        {
            // Inform all listeners about player input disabled 
            _glc.NotifyCameraIsMoving(true);

            // Get starting position for camera lerping
            movementStartPosition = sceneCamera.transform.position;
            // Determine movement direction
            moveDirection = 1;

            // Temporarly deactivate camera offset
            _sceneCameraOffset.enabled = false;
            fromRotation = sceneCamera.transform.localRotation;

            if (!orthographicLerpMode)
            {
                _rotation = Quaternion.LookRotation(target.transform.position - _currentTarget.position, Vector3.up);
                _currentTarget = to;
            }
            else
            {
                _currentTarget = to;
                _currentTarget.position = target.position;
            }

            StartCoroutine(Waiting());
            viewing = false;
        }
    }

    private void LateUpdate()
    {
        if (_currentTarget == null)
            return;

        //Debug.Log(movementStartPosition);
        // Animates the position between a and b looking to target
        movementTimePassed = movementTimePassed + Time.deltaTime / (viewingTime / 2);

        sceneCamera.transform.position = Vector3.Slerp(movementStartPosition, _currentTarget.position, movementTimePassed);
        if (moveDirection == 1)
            sceneCamera.orthographicSize = Mathf.SmoothStep(baseOrthographicSize, baseOrthographicSize * zoomOutFactor, movementTimePassed);
        else if (moveDirection == -1)
            sceneCamera.orthographicSize = Mathf.SmoothStep(baseOrthographicSize * zoomOutFactor, baseOrthographicSize, movementTimePassed);

        if (!orthographicLerpMode)
            sceneCamera.transform.localRotation = Quaternion.Slerp(sceneCamera.transform.localRotation, _rotation, Time.deltaTime * interpolationSpeed);
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(viewingTime);
        playerCharacter.position = resetPosition.transform.position;
        _currentTarget.position = playerCharacter.position + _sceneCameraOffset.offset;
        _rotation = fromRotation;
        moveDirection = -1;
        movementTimePassed = 0;
        movementStartPosition = sceneCamera.transform.position;

        yield return new WaitForSeconds(viewingTime);
        _currentTarget = null;
        _sceneCameraOffset.enabled = true;
        sceneCamera.orthographicSize = baseOrthographicSize;
        movementStartPosition = sceneCamera.transform.position;
        movementTimePassed = 0;

        // Inform all listeners about player input enabled 
        _glc.NotifyCameraIsMoving(false);
    }
}
