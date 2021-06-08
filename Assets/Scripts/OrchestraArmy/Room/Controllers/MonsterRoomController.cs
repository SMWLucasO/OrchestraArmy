using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Room;

namespace OrchestraArmy.Room.Controllers
{
    public class MonsterRoomController : RoomController, IListener<RoomClearedOfEnemiesEvent>
    {
        public override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Bind<RoomClearedOfEnemiesEvent>(this);
        }
        
        public override void UnregisterEvents()
        {
            base.RegisterEvents();
            EventManager.Unbind<RoomClearedOfEnemiesEvent>(this);
        }

        public void OnEvent(RoomClearedOfEnemiesEvent invokedEvent) 
            => RoomManager.Instance.RoomsCleared += 1;
    }
}