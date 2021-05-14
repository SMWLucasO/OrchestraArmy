using System;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Player;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Room
{
    public class RoomController : MonoBehaviour, IListener<RoomDoorDownEvent>, IListener<RoomDoorUpEvent>,
        IListener<RoomDoorLeftEvent>, IListener<RoomDoorRightEvent>, IListener<PlayerDeathEvent>
    {
        private enum DoorDirection
        {
            Left,
            Right,
            Up,
            Down
        };
        public GameObject DeathScreen;

        /// <summary>
        /// Objects for the map surroundings
        /// </summary>
        public GameObject FillObj,
            RockObj,
            RubbleObj,
            WallObj,
            FloorObj,
            DoorRightObj,
            DoorLeftObj,
            DoorUpObj,
            DoorDownObj;

        /// <summary>
        /// List to save the different types of enemies
        /// </summary>
        public List<GameObject> Enemies;

        private Room[,] _rooms;

        /// <summary>
        /// 0 through n. 0 means only guitar, n means all. n = 3 for now.
        /// </summary>
        [Range(0, 3)] 
        public int CollectedInstruments = 3;

        private List<GameObject> _instantiated;
        private Vector2 _currentRoom;

        private int _level = 1;
        private int _roomsCleared = 0;
        private byte _deathState = 0;   //0=show deathscreen, 1=reload level, 3=wait/disable screen (used in update())
        private int _enemiesFib1 = 0, _enemiesFib2 = 1, _enemiesNow;
        private float _timeOfDeath = 0 ;
        
        // Start is called before the first frame update
        void Start()
        {
            Setup();

            // bind events
            EventManager.Bind<RoomDoorUpEvent>(this);
            EventManager.Bind<RoomDoorDownEvent>(this);
            EventManager.Bind<RoomDoorLeftEvent>(this);
            EventManager.Bind<RoomDoorRightEvent>(this);   // door actions (go though door)
            EventManager.Bind<PlayerDeathEvent>(this);      // death actions (player died)

        }
        
        void Setup()
        {
            _instantiated = new List<GameObject>(); //room object list
            _rooms = new Room[20, 20];  //make a grid
            _currentRoom = new Vector2(10, 10); // Start at the halfway point of the room grid
            CreateRoom(_currentRoom);   //make a room
            SpawnRoom();                //spawn the room
        }

        /// <summary>
        /// Create room
        /// </summary>
        /// <param name="position"></param>
        private void CreateRoom(Vector2 position)
        {
            // Get amount of enemies based on rooms cleared in this level

            // Calculation for chance boss room (after 5 rooms +20% per room)
            if (Random.value < 0.1f * (_roomsCleared - 5 + Math.Abs(_roomsCleared - 5)))
            {    //calculation for chance boss room (after 5 rooms +20% per room)
                print("boss room");
                _rooms[(int)position.x, (int)position.y] = new Room(GetNumberOfEnemies(), boss:true); // create boss room
            } else if (_roomsCleared==0) {
                print("spawnRoom");
                _rooms[(int)position.x, (int)position.y] = new Room(0, spawn:true); // create spawn room
            }
            else
            {
                _rooms[(int)position.x, (int)position.y] = new Room(GetNumberOfEnemies()); // create new room
            }
        }

        /// <summary>
        /// Get number of enemies
        /// </summary>
        private int GetNumberOfEnemies() 
        {
            if (_enemiesNow < 8)
            {
                _enemiesNow = _enemiesFib1 + _enemiesFib2;
                _enemiesFib1 = _enemiesFib2;
                _enemiesFib2 = _enemiesNow;
            }
            else
            {
                _enemiesNow += 4;
            }

            return _enemiesNow;
        }

        /// <summary>
        /// Go into a room corresponding to the given direction
        /// </summary>
        /// <param name="direction"></param>
        private void ChangeCurrentRoom(DoorDirection direction)
        {
            // Clear previous field
            DestroyCurrentRoom();

            // Move to another position on the room grid
            switch (direction)
            {
                case DoorDirection.Left:
                    _currentRoom.x -= 1;
                    break;

                case DoorDirection.Right:
                    _currentRoom.x += 1;
                    break;

                case DoorDirection.Up:
                    _currentRoom.y -= 1;
                    break;

                case DoorDirection.Down:
                    _currentRoom.y += 1;
                    break;

                default:
                    _currentRoom += new Vector2(0, -1);
                    break;
            }

            // Check if array is in bounds, if not return to bounds
            // the 3d shape of the map is supposed to be a torroid
            if (_currentRoom.x > 19)
            {
                _currentRoom.x = 0; // 19;
            }

            if (_currentRoom.x < 0)
            {
                _currentRoom.x = 19; // 0;
            }

            if (_currentRoom.y < 0)
            {
                _currentRoom.y = 19; // 0;
            }

            if (_currentRoom.y > 19)
            {
                _currentRoom.y = 0; // 19;
            }

            print(_currentRoom);

            // Create room if it does not exist
            if (_rooms[(int) _currentRoom.x, (int) _currentRoom.y] == null)
            {
                _roomsCleared++;
                CreateRoom(_currentRoom);
            }

            SpawnRoom();
        }

        /// <summary>
        /// Check if tile directly borders an empty tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private bool BorderOnFill(int x, int y)
        {
            Room room = _rooms[(int) _currentRoom.x, (int) _currentRoom.y];
            bool outp = false;
            try
            {
                outp = outp || room.Grid[x - 1, y] == Room.GridSpace.Empty;
            }

            catch
            {
                return true;
            }

            try
            {
                outp = outp || room.Grid[x + 1, y] == Room.GridSpace.Empty;
            }

            catch
            {
                return true;
            }

            try
            {
                outp = outp || room.Grid[x, y - 1] == Room.GridSpace.Empty;
            }

            catch
            {
                return true;
            }

            try
            {
                outp = outp || room.Grid[x, y + 1] == Room.GridSpace.Empty;
            }

            catch
            {
                return true;
            }

            return outp;
        }


        /// <summary>
        /// Spawn room into game
        /// </summary>
        private void SpawnRoom()
        {
            Room room = _rooms[(int) _currentRoom.x, (int) _currentRoom.y];

            for (int x = 0; x < room.RoomWidth; x++)
            {
                for (int y = 0; y < room.RoomHeight; y++)
                {
                    switch (room.Grid[x, y])
                    {
                        case Room.GridSpace.Empty:
                            Spawn(x, y, FillObj);
                            break;

                        case Room.GridSpace.Floor:
                            Spawn(x, y, FloorObj);
                            if (Random.value < 0.1f)
                            {
                                Spawn(x, y, RubbleObj.transform.GetChild(Random.Range(0, 3)).gameObject);
                            }

                            break;
                        case Room.GridSpace.Wall:
                            Spawn(x, y,
                                BorderOnFill(x, y)
                                    ? WallObj
                                    : RockObj.transform.GetChild(Random.Range(0, 4)).gameObject);
                            break;

                        case Room.GridSpace.DoorU:
                            Spawn(x, y, DoorUpObj);
                            break;

                        case Room.GridSpace.DoorD:
                            Spawn(x, y, DoorDownObj);
                            break;

                        case Room.GridSpace.DoorL:
                            Spawn(x, y, DoorLeftObj);
                            break;

                        case Room.GridSpace.DoorR:
                            Spawn(x, y, DoorRightObj);
                            break;
                    }
                }
            }

            // Spawn enemies
            int newestEnemy = Math.Min(CollectedInstruments, Enemies.Count - 1);
            int enemyTypes = newestEnemy + 1;

            int newestEnemyPercentage = (int) (100.0 / enemyTypes + 10.0);
            int newestEnemyAmount =
                (int) ((float) room.EnemySpawnLocations.Count / 100.0 * (float) newestEnemyPercentage);
            
            // Make sure there is always at least one new enemy in the field
            newestEnemyAmount =
                Math.Max(newestEnemyAmount, 1); 

            foreach (Vector2 _enemy in room.EnemySpawnLocations)
            {
                if (newestEnemyAmount > 0)
                {
                    Spawn(_enemy.x, _enemy.y, Enemies[newestEnemy]);
                    newestEnemyAmount--;
                }
                else
                {
                    // Check how many types of enemies may spawn and randomly get enemy to spawn from older ones
                    Spawn(_enemy.x, _enemy.y, Enemies[Random.Range(0, newestEnemy)]);
                }
            }


            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                player.transform.position = new Vector3(75 - room.OffsetOfRoom.x, player.transform.position.y,
                    75 - room.OffsetOfRoom.y);
        }


        /// <summary>
        /// Destroy a room to make space for a new one
        /// </summary>
        private void DestroyCurrentRoom()
        {
            foreach (GameObject g in _instantiated)
            {
                Destroy(g);
            }
        }

        /// <summary>
        /// Spawn object into game
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="toSpawn"></param>
        private void Spawn(float x, float y, GameObject toSpawn)
        {
            Room room = _rooms[(int) _currentRoom.x, (int) _currentRoom.y];

            // Find the position to spawn
            Vector3 spawnPos = new Vector3(x, 0, y) - new Vector3(room.OffsetOfRoom.x, 0, room.OffsetOfRoom.y);
            // Spawn object
            // create object and add it to the list
            _instantiated.Add(Instantiate(toSpawn, spawnPos,
                Quaternion.identity)); 
        }


        /// <summary>
        /// Event for when you touch the down door
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(RoomDoorDownEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Down);
        }

        /// <summary>
        /// Event for when you touch the up door
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(RoomDoorUpEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Up);
        }

        /// <summary>
        /// Event for when you touch the left door
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(RoomDoorLeftEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Left);
        }

        /// <summary>
        /// Event for when you touch the right door
        /// </summary>
        /// <param name="invokedEvent"></param>
        public void OnEvent(RoomDoorRightEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Right);
        }

        private void Update()
        {
            //death animation and hidden un-/loading
            switch (_deathState)
            {
                case (1):
                    DeathScreen.SetActive(true);        //activate death screen
                    _roomsCleared = 0;                  // reset roomsCleared
                    _timeOfDeath = Time.time;
                    _deathState++;
                    break;
                
                case (2):                               //slow functions hidden by death screen
                    _level= (_level>1)?_level-1:1;      //go one level (not room) back (if possible)
                    DestroyCurrentRoom();               //destroy the room
                    Setup();                            //reset the map, respawn the start/spawn room and reset location on map
                    _deathState++;
                    break;
                
                case (3):
                    if (Time.time - _timeOfDeath >= 2)  //extends the time of the deathscreen on fast computers
                    {
                        DeathScreen.SetActive(false); //deactivate death screen
                        _deathState = 0; //deactivate death 'loop'
                    }
                    break;
            }
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
         {
              _deathState = 1;
         }

    }
}