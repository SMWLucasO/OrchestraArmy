namespace OrchestraArmy.Room.Controllers.Factories
{
    public class DefaultRoomControllerFactory : IRoomControllerFactory
    {
        public RoomController MakeBossRoomController()
            => new BossRoomController();

        public RoomController MakeStartingRoomController()
            => new StartingRoomController();

        public RoomController MakeMonsterRoomController()
            => new MonsterRoomController();
    }
}