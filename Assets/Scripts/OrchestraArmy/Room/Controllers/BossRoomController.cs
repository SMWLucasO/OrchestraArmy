using OrchestraArmy.Entity.Entities.Enemies.Bosses;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;
using UnityEngine;

namespace OrchestraArmy.Room.Controllers
{
    public class BossRoomController : RoomController, IListener<RoomClearedOfEnemiesEvent>, IListener<BossDeathEvent>,
        IListener<InstrumentPickupEvent>
    {
        
        public override void RegisterEvents()
        {
            
            Debug.Log("Boss room!");
            
            base.RegisterEvents();
            EventManager.Bind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Bind<InstrumentPickupEvent>(this);
            EventManager.Bind<BossDeathEvent>(this);
        }

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
            EventManager.Unbind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Unbind<InstrumentPickupEvent>(this);
            EventManager.Unbind<BossDeathEvent>(this);
        }

        public void OnEvent(RoomClearedOfEnemiesEvent invokedEvent)
        {

            int BossSpawnIndex = LevelManager.Instance.Level - 1;

            if (BossSpawnIndex >= RoomManager.Instance.RoomPrefabData.Bosses.Count)
                return;

            // Get the right boss, since we want to spawn it.
            GameObject BossToSpawn = RoomManager.Instance.RoomPrefabData.Bosses[BossSpawnIndex];

            // Calculate the center of the room.
            float roomMidX = Room.RoomWidth / 2;
            float roomMidY = Room.RoomHeight / 2;
            
            // Add the boss.
            Objects.Add(
                GameObject.Instantiate(
                        BossToSpawn, 
                        new Vector3( roomMidX,0, roomMidY) - new Vector3(Room.OffsetOfRoom.x, 0, Room.OffsetOfRoom.y),
                        Quaternion.identity
                    )
                );
            
            Debug.Log("Called here");
        }

        public void OnEvent(BossDeathEvent invokedEvent)
        {
            // Spawn instrument to pick up
        }

        public void OnEvent(InstrumentPickupEvent invokedEvent)
        {
            // add instrument to weapons
            // spawn portal
        }
    }
}