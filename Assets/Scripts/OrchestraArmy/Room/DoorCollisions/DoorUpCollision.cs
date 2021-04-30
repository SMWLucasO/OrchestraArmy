using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

public class DoorUpCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        print("up");
        EventManager.Invoke(new RoomDoorDownEvent());
    }
    void OnCollisionEnter()
    {
        EventManager.Invoke(new RoomDoorUpEvent());
        print("up");
    }
}
