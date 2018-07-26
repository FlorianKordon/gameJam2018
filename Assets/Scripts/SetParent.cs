using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        transform.parent = GameObject.Find("prototyp_labyrinth_v03").transform;
    //object1 is now the child of object2


    }

    // Update is called once per frame
    void Update()
    {

    }
}
