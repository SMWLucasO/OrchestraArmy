using OrchestraArmy.Room.Controllers.Factories;
using OrchestraArmy.Room.Rooms;

namespace OrchestraArmy.Room.Factories
{
    public class DefaultRoomFactory : IRoomFactory
    {
        public IRoomControllerFactory RoomControllerFactory { get; set; }
            = new DefaultRoomControllerFactory();

        public Room MakeBossRoom() 
            => new BossRoom() { RoomController = RoomControllerFactory.MakeBossRoomController() };

        public Room MakeStartingRoom()
            => new StartingRoom() { RoomController = RoomControllerFactory.MakeStartingRoomController() };

        public Room MakeMonsterRoom()
            => new MonsterRoom() { RoomController = RoomControllerFactory.MakeMonsterRoomController() };
    }
}