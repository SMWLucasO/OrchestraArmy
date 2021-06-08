using System;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Room.Rooms;
using UnityEngine;

namespace OrchestraArmy.Room
{
    public class LevelManager : MonoBehaviour, IListener<PlayerDeathEvent>
    {
        
        /// <summary>
        /// The single instance of the levelmanager.
        /// </summary>
        public static LevelManager Instance { get; set; }

        /// <summary>
        /// The Level on which the rooms are located.
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// The room manager for the level.
        /// </summary>
        public RoomManager RoomManager { get; set; }

        private float _timeOfDeath = 0;
        
        private int _deathState = 0;

        public GameObject DeathScreen;
        
        private void Start()
        {
            Instance = this;
            RoomManager = GetComponent<RoomManager>();
            
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        /// <summary>
        /// Go to the next level.
        /// </summary>
        public void MoveToNextLevel()
        {
            RoomManager.Instance.DestroyRooms();
            this.Level += 1;

            RoomManager.CurrentRoomIndex = new Vector2Int(10, 10);

            // generate the starting room for the player.
            RoomManager.Instance.GenerateRoom(new Vector2(10, 10), RoomType.StartingRoom);
            
            // set to one such that we can get to other rooms.
            RoomManager.RoomsCleared = 1;

            // Position the player properly.
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null)
                player.transform.position = RoomManager.CurrentRoom.GetPlayerSpawnPosition(DoorDirection.Left);

        }

        /// <summary>
        /// Go to the previous level.
        /// </summary>
        public void MoveToPreviousLevel()
        {

            RoomManager.Instance.DestroyRooms();

            // we do not want the guitar to be locked, it should only be available in the final level.
            if (this.Level > 1)
            {
                this.Level -= 1;
                RoomManager.CollectedInstrumentCount -= 1;

            }
            
            RoomManager.CurrentRoomIndex = new Vector2Int(10, 10);

            // generate the starting room for the player.
            RoomManager.Instance.GenerateRoom(new Vector2(10, 10), RoomType.StartingRoom);

            // set to one such that we can get to other rooms.
            RoomManager.RoomsCleared = 1;
            
            // Position the player properly.
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null)
                player.transform.position = RoomManager.CurrentRoom.GetPlayerSpawnPosition(DoorDirection.Left);
        }

        private void Update()
        {
            //death animation and hidden un-/loading
            switch (_deathState)
            {
                case (1):
                    DeathScreen.SetActive(true);        //activate death screen
                    RoomManager.RoomsCleared = 0;       // reset roomsCleared
                    _timeOfDeath = Time.time;
                    _deathState++;
                    break;
                
                case (2):                               //slow functions hidden by death screen
                    _deathState++;
                    break;
                
                case (3):
                    if (Time.time - _timeOfDeath >= 2)  //extends the time of the deathscreen on fast computers
                    {
                        MoveToPreviousLevel();
                        DeathScreen.SetActive(false); //deactivate death screen
                        _deathState = 0; //deactivate death 'loop'
                    }
                    break;
            }
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
            => _deathState = 1;
        
    }
}