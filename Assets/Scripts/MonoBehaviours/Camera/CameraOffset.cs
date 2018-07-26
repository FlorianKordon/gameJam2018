using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public Transform playerCharacter;
    public Vector3 offset = new Vector3(-19.5f, 16.5f, -20f);

    private Vector3 _camDistance;
    private Quaternion _camRotation;
    private int x = 0;

    private void Awake()
    {
        transform.position = playerCharacter.position + offset;
        _camDistance = playerCharacter.position - transform.position;
        _camRotation = transform.rotation;
    }

    private void Update()
    {
        transform.position = playerCharacter.position + offset;
        //transform.position = new Vector3(playerCharacter.position.x - _camDistance.x, transform.position.y, playerCharacter.position.z - _camDistance.z);
        transform.rotation = _camRotation;
    }
}
