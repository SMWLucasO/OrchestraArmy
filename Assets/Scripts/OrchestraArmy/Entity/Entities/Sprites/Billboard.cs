using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Class for making sure the sprite is parallel to the camera
*/
public class Billboard : MonoBehaviour
{
    void LateUpdate() 
    {
        transform.forward = Camera.main.transform.forward;
    }
}