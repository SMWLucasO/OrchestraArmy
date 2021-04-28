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

    void OnCollisionEnter()
    {
        print("left");
        
    }

    void OnMouseOver()
    {
        print("left");
        EventManager.Invoke(new LevelDoorLeftEvent());
        
    }
}
