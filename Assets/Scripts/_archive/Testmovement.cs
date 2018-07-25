﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmovement : MonoBehaviour {

    public VirtualJoystick joystick;
    public float speed = 200.0f;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rigid.AddForce(joystick.InputDirection * speed * Time.deltaTime);
    }
}