using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;

namespace OrchestraArmy.Room.Rooms
{
    public class BossRoom : Room, IListener<BossDeathEvent>, IListener<InstrumentPickupEvent>
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
        
        public void OnEvent(BossDeathEvent invokedEvent)
        {
                       
        }

        public void OnEvent(InstrumentPickupEvent invokedEvent)
        {
            
        }
    }
}