using System;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Room;
using UnityEngine;

namespace OrchestraArmy.Room.DoorCollisions
{
    public abstract class DoorColision : MonoBehaviour,IListener<RoomClearedOfEnemiesEvent>
    {
        private void Awake()
        {
            EventManager.Bind<RoomClearedOfEnemiesEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.Unbind<RoomClearedOfEnemiesEvent>(this);
        }

        public void OnEvent(RoomClearedOfEnemiesEvent invokedEvent)
        {
            transform.Find("Closed").gameObject.SetActive(false);
        }
    }
}