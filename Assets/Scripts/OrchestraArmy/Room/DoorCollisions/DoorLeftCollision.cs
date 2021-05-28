using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
    public class DoorLeftCollision : DoorCollision
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                EventManager.Invoke(new RoomDoorLeftEvent());
        }
    }
}