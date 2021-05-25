namespace OrchestraArmy.Room.Controllers.Factories
{
    public interface IRoomControllerFactory
    {
        /// <summary>
        /// Generate the boss room controller.
        /// </summary>
        /// <returns></returns>
        public RoomController MakeBossRoomController();

        /// <summary>
        /// Generate the starting room controller.
        /// </summary>
        /// <returns></returns>
        public RoomController MakeStartingRoomController();

        /// <summary>
        /// Generate the monster room controller.
        /// </summary>
        /// <returns></returns>
        public RoomController MakeMonsterRoomController();
    }
}