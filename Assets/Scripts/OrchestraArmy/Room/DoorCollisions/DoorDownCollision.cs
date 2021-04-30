using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

public class DoorDownCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EventManager.Invoke(new RoomDoorDownEvent());
        print("down");
    }
    void OnCollisionEnter()
    {
        EventManager.Invoke(new RoomDoorDownEvent());
        print("down");
    }
}
