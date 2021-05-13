using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ttt : MonoBehaviour
{
    public NavMeshSurface Surface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("m"))
        {
            Surface.BuildNavMesh();
        }
    }
}
