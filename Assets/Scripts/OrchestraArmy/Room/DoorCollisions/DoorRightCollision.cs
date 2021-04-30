using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

public class DoorRightCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EventManager.Invoke(new RoomDoorDownEvent());
        print("right");
    }
    void OnCollisionEnter()
    {
        EventManager.Invoke(new RoomDoorRightEvent());
        print("right");
    }
}
