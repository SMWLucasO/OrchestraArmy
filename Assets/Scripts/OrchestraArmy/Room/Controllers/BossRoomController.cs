using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;
using OrchestraArmy.Event.Events.Room;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Room.Controllers
{
    public class BossRoomController : RoomController, IListener<RoomClearedOfEnemiesEvent>, IListener<BossDeathEvent>,
        IListener<FinalBossDeathEvent>, IListener<RoomDoorNextLevelEvent>
    {
        
        public override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Bind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Bind<FinalBossDeathEvent>(this);
            EventManager.Bind<BossDeathEvent>(this);
            EventManager.Bind<RoomDoorNextLevelEvent>(this);
        }

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
            EventManager.Unbind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Unbind<BossDeathEvent>(this);
            EventManager.Unbind<FinalBossDeathEvent>(this);
            EventManager.Unbind<RoomDoorNextLevelEvent>(this);
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
                    new Vector3(roomMidX, 0, roomMidY) - new Vector3(Room.OffsetOfRoom.x, 0, Room.OffsetOfRoom.y),
                    Quaternion.identity
                )
            );
        }

        public void OnEvent(BossDeathEvent invokedEvent)
        {
            int instrumentSpawnIndex = LevelManager.Instance.Level - 1;
            
            if (instrumentSpawnIndex >= RoomManager.Instance.RoomPrefabData.InstrumentDrops.Count)
                return;
            // Spawn the drop for this level.
            GameObject dropToSpawn = RoomManager.Instance.RoomPrefabData.InstrumentDrops[instrumentSpawnIndex];
            
            // Add the drop.
            Objects.Add(
                GameObject.Instantiate(
                    dropToSpawn,
                    invokedEvent.PositionOfDeath + new Vector3(0, 0.5f, 0),
                    Quaternion.identity
                )
            );
        }

        /// <summary>
        /// Event for moving to the next level.
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(RoomDoorNextLevelEvent invokedEvent) 
            => LevelManager.Instance.MoveToNextLevel();

        /// <summary>
        /// Event for after killing the conductor.
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(FinalBossDeathEvent invokedEvent)
        {
            // The Conductor was defeated.
            SceneManager.LoadScene(0);
        }
    }
}
