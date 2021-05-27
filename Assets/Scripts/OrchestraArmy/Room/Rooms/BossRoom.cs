using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;
using UnityEngine;

namespace OrchestraArmy.Room.Rooms
{
    public class BossRoom : Room
    {
        public override void SetupSettings()
        {
            RoomGenerationData.WalkerDirectionChangeChance = .99f;
            RoomGenerationData.WalkerSpawnChance = 0.8f;
            RoomGenerationData.WalkerDestructionChance = 0.8f;
            RoomGenerationData.MaxAliveWalkers = 20;
            RoomGenerationData.PercentToFill = 0.1f;
        }

        protected override void CreateDoors()
        {
            // do nothing. We do not want to spawn doors at this point.
        }

        public override Vector3 GetPlayerSpawnPosition(DoorDirection doorDirection) =>
            new Vector3(RoomSizeWorldUnits.x / 2 - OffsetOfRoom.x, 0.5f,
                RoomSizeWorldUnits.y / 2 - OffsetOfRoom.y);
    }
}