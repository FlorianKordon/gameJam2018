using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatformController : MonoBehaviour
{
    public Transform from;
    public Transform to;
    public Transform target;
    public Transform resetPosition;
    public Transform sceneCamera;
    public Transform playerCharacter;
    public float viewingTime = 3;

    public float interpolationSpeed;
    public bool viewing;

    private Quaternion _rotation;
    private Transform _currentView;

    private CameraOffset _sceneCameraOffset;

    private void Start()
    {
        _sceneCameraOffset = sceneCamera.GetComponent<CameraOffset>();
    }

    private void Update()
    {
        if (viewing)
        {
            // Deactivate camera offset
            _sceneCameraOffset.enabled = false;

            _currentView = to;
            _rotation = Quaternion.LookRotation(target.transform.position - _currentView.position, Vector3.up);
            StartCoroutine(Waiting());
        }
    }

    private void LateUpdate()
    {
        if (_currentView == null)
            return;

        // Animates the position between a and b looking to target
        sceneCamera.position = Vector3.Slerp(sceneCamera.position, _currentView.position, Time.deltaTime * interpolationSpeed);
        Quaternion currentRotation = Quaternion.Slerp(sceneCamera.localRotation, _rotation, Time.deltaTime * interpolationSpeed);

        sceneCamera.localRotation = currentRotation;
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(viewingTime);
        _currentView = from;
        _rotation = from.transform.rotation;

        playerCharacter.position = resetPosition.transform.position;

        viewing = false;
        // Reactivate camera offset
        _sceneCameraOffset.enabled = true;
    }
}
