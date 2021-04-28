using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;
using UnityEngine;

public class DoorLeftCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        EventManager.Invoke(new LevelDoorDownEvent());
        print("left");
    }
    void OnCollisionEnter()
    {
        EventManager.Invoke(new LevelDoorLeftEvent());
        print("left");
        
    }
}
