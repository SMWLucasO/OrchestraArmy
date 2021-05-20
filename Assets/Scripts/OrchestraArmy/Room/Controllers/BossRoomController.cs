using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;

namespace OrchestraArmy.Room.Controllers
{
    public class BossRoomController : RoomController, IListener<RoomClearedOfEnemiesEvent>, IListener<BossDeathEvent>,
        IListener<InstrumentPickupEvent>
    {
        public override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Bind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Bind<BossDeathEvent>(this);
        }

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
            EventManager.Unbind<RoomClearedOfEnemiesEvent>(this);
            EventManager.Unbind<BossDeathEvent>(this);
        }

        public void OnEvent(RoomClearedOfEnemiesEvent invokedEvent)
        {
            // spawn boss
            switch (LevelManager.Instance.Level)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            
            // Create boss at the center of the room or something.
            // base this on level.
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