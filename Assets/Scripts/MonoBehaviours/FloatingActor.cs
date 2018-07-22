using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingActor : MonoBehaviour
{

    public float heightSpeed = 0.5f;
    public float rotSpeed = 0.1f;
    private float runningTime;

    private void Start()
    {
        runningTime = Time.time;
    }

    private void Update()
    {
        // Floating in y-direction
        float deltaTime = Time.deltaTime;
        Vector3 newPosition = transform.position;
        float deltaFactor = Mathf.Sin(runningTime + deltaTime) - Mathf.Sin(runningTime);
        newPosition.y += deltaFactor * heightSpeed;
        transform.position = newPosition;
        // Update running time.
        runningTime += deltaTime;

        // Rotation around y-axis
        transform.Rotate(new Vector3(0, rotSpeed, 0), Space.World);
    }
}
