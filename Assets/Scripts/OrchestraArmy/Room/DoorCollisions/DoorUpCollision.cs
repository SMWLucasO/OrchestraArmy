using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
    public class DoorUpCollision : DoorColision
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                EventManager.Invoke(new RoomDoorUpEvent());
        }
    }
}