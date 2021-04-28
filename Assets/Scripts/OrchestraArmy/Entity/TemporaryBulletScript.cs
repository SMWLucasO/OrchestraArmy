using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//!!!!!!!!!!!!!!!!!!!!!
//Temporary file to test aiming, will not be part of the final product.
//!!!!!!!!!!!!!!!!!!!!!
public class TemporaryBulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }
}
