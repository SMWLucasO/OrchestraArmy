using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
    public class DoorDownCollision : MonoBehaviour
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
            print("down");
        }
        void OnCollisionEnter()
        {
            EventManager.Invoke(new RoomDoorDownEvent());
            print("down");
        }

        void OnMouseOver()
        {
        
        }
    }
}
