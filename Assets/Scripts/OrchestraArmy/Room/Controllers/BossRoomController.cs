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

            int bossSpawnIndex = LevelManager.Instance.Level - 1;

            if (bossSpawnIndex >= RoomManager.Instance.RoomPrefabData.Bosses.Count)
                return;

            // Get the right boss, since we want to spawn it.
            GameObject bossToSpawn = RoomManager.Instance.RoomPrefabData.Bosses[bossSpawnIndex];

            // Calculate the center of the room.
            float roomMidX = Room.RoomWidth / 2;
            float roomMidY = Room.RoomHeight / 2;
            
            // Add the boss.
            Objects.Add(
                GameObject.Instantiate(
                        bossToSpawn, 
                        new Vector3(roomMidX,0, roomMidY) - new Vector3(Room.OffsetOfRoom.x, 0, Room.OffsetOfRoom.y),
                        Quaternion.identity
                    )
                );
        }

        public void OnEvent(BossDeathEvent invokedEvent)
        {
            int instrumentSpawnIndex = LevelManager.Instance.Level - 1;
            
            if (instrumentSpawnIndex >= RoomManager.Instance.RoomPrefabData.InstrumentDrops.Count)
                return;
            
            GameObject dropToSpawn = RoomManager.Instance.RoomPrefabData.InstrumentDrops[instrumentSpawnIndex];
            
            // Add the drop.
            Objects.Add(
                GameObject.Instantiate(
                        dropToSpawn,
                        invokedEvent.PositionOfDeath,
                        Quaternion.identity
                    )
                );
        }

        public void OnEvent(InstrumentPickupEvent invokedEvent)
        {
            Debug.Log("CALL");
            // add instrument to weapons
            // spawn portal
        }
    }
}