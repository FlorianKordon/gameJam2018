using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [SerializeField]
    private float size = 1f;
    private int xx;
    private int zz;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        if (size >= 0.4f)
        {

            int xCount = Mathf.RoundToInt(position.x / size);
            int yCount = Mathf.RoundToInt(position.y / size);
            int zCount = Mathf.RoundToInt(position.z / size);

            Vector3 result = new Vector3(xCount * size, yCount * size, zCount * size);

            result += transform.position;

            return result;
        }

        else return Vector3.zero;
    }

    private void OnDrawGizmos()
    {  
        if (size >= 0.4f){
        Gizmos.color = Color.yellow;
        for (float x = xx; x < 40; x += size)
        {
            for (float z = zz; z < 40; z += size)
            {
               
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
            }
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        xx = (int)transform.position.x ;
        zz = (int)transform.position.z ;
        //DrawClickablePoints();
    }
}
