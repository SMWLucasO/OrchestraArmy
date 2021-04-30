using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
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
            EventManager.Invoke(new RoomDoorDownEvent());
            print("left");
        }
        void OnCollisionEnter()
        {
            EventManager.Invoke(new RoomDoorLeftEvent());
            print("left");
        
        }
    }
}


