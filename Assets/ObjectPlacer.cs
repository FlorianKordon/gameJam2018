using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inspired by https://www.youtube.com/watch?v=VBZFYGWvm4A
public class ObjectPlacer : MonoBehaviour
{

    private Grid grid;
    public GameObject o;

    // Use this for initialization
    void Start()
    {
        //Instantiate(o, new Vector3 (0,4,0), o.transform.rotation);
    }

    // Update is called once per frame

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Test1");
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Test2");
                PlaceObjectNear(hitInfo.point);
            }
        }
    }

    private void PlaceObjectNear(Vector3 clickpoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickpoint);
        Instantiate(o, finalPosition + new Vector3(0, 4, 0), o.transform.rotation);

        //GameObject.CreatePrimitive(PrimitveType.Cube).transform.position = finalPosition;
    }
}
