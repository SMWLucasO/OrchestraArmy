using OrchestraArmy.Room.Controllers.Factories;
using OrchestraArmy.Room.Rooms;

namespace OrchestraArmy.Room.Factories
{
    public interface IRoomFactory
    {

        /// <summary>
        /// The factory which will be used to generate the room's controller.
        /// </summary>
        public IRoomControllerFactory RoomControllerFactory { get; set; }
        
        /// <summary>
        /// Generate the boss room.
        /// </summary>
        /// <returns>BossRoom object</returns>
        public Room MakeBossRoom();

        /// <summary>
        /// Generate the starting room.
        /// </summary>
        /// <returns>StartingRoom object</returns>
        public Room MakeStartingRoom();

        /// <summary>
        /// Generate the monster room.
        /// </summary>
        /// <returns>MonsterRoom object</returns>
        public Room MakeMonsterRoom();

    }
}