using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
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
}