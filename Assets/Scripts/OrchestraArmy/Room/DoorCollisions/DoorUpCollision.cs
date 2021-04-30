using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
    public class DoorUpCollision : MonoBehaviour
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
            print("up");
            EventManager.Invoke(new RoomDoorDownEvent());
        }
        void OnCollisionEnter()
        {
            EventManager.Invoke(new RoomDoorUpEvent());
            print("up");
        }
    }
}
