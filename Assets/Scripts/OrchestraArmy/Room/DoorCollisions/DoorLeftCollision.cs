using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

public class DoorLeftCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EventManager.Invoke(new RoomDoorDownEvent());
        print("left");
    }
    void OnCollisionEnter()
    {
        EventManager.Invoke(new RoomDoorLeftEvent());
        print("left");
        
    }
}
