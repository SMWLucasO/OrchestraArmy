using System;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Room
{
    public class RoomController : MonoBehaviour, IListener<RoomDoorDownEvent>, IListener<RoomDoorUpEvent>, IListener<RoomDoorLeftEvent>, IListener<RoomDoorRightEvent>
    {
        private enum DoorDirection { Left, Right, Up, Down };
        public GameObject FillObj, RockObj, RubbleObj, WallObj, FloorObj, DoorRightObj, DoorLeftObj, DoorUpObj, DoorDownObj;
        private List<GameObject> _instantiated; // list to save all current room objects in game

        public List<GameObject> Enemies;
        private Room[,] _rooms;
        private int _collectedInstruments = 3; // 0 through n. 0 means only guitar, n means all. n = 3 for now.
        private Vector2 _currentRoom;

        private int _roomsCleared = 0;

        private int _enemiesFib1 = 0, _enemiesFib2 = 1, _enemiesNow;

        // Start is called before the first frame update
        void Start()
        {
            _instantiated = new List<GameObject>();
            _rooms = new Room[20, 20]; // can move 10 rooms in each way
            _currentRoom = new Vector2(10, 10); // start at the halfway point of the room grid
            CreateRoom(_currentRoom);  //make a grid
            SpawnRoom();

            // bind events
            EventManager.Bind<RoomDoorUpEvent>(this);
            EventManager.Bind<RoomDoorDownEvent>(this);
            EventManager.Bind<RoomDoorLeftEvent>(this);
            EventManager.Bind<RoomDoorRightEvent>(this);   // door actions (go through door)

        }

        void CreateRoom(Vector2 position)
        {
            //get amount of enemies based on rooms cleared in this level
            int _numberOfEnemies = GetNumberOfEnemies();

            if (Random.value < 0.1f * (_roomsCleared - 5 + Math.Abs(_roomsCleared - 5)))
            {    //calculation for chance boss room (after 5 rooms +20% per room)
                print("boss room");
                _rooms[(int)position.x, (int)position.y] = new Room(_numberOfEnemies, true); // create boss room, numberOfEnemies is irrelevant in this case
            }
            else
            {
                _rooms[(int)position.x, (int)position.y] = new Room(_numberOfEnemies); // create new room
            }
        }

        int GetNumberOfEnemies() // uses fibonacci to get amount of enemies
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

        void ChangeCurrentRoom(DoorDirection direction)
        {
            // clear previous field
            DestroyCurrentRoom();

            // move to another position on the room grid
            switch (direction)
            {
                case DoorDirection.Left:
                    _currentRoom += new Vector2(-1, 0);
                    break;

                case DoorDirection.Right:
                    _currentRoom += new Vector2(1, 0);
                    break;

                case DoorDirection.Up:
                    _currentRoom += new Vector2(0, -1);
                    break;

                case DoorDirection.Down:
                    _currentRoom += new Vector2(0, 1);
                    break;

                default:
                    _currentRoom += new Vector2(0, -1);
                    break;
            }

            // check if array is in bounds, if not return to bounds      //the 3d shape of the map is suppost to be a torroid
            if (_currentRoom.x > 19)
            {
                _currentRoom.x = 0;//19;
            }

            if (_currentRoom.x < 0)
            {
                _currentRoom.x = 19;//0;
            }

            if (_currentRoom.y < 0)
            {
                _currentRoom.y = 19;//0;
            }

            if (_currentRoom.y > 19)
            {
                _currentRoom.y = 0;//19;
            }

            print(_currentRoom);

            // create room if it does not exist
            if (_rooms[(int)_currentRoom.x, (int)_currentRoom.y] == null)
            {
                CreateRoom(_currentRoom);
                _roomsCleared++;
            }

            SpawnRoom();
        }

        bool borderOnFill(int x, int y)
        {       //check if tile directly borders an empty tile
            Room room = _rooms[(int)_currentRoom.x, (int)_currentRoom.y];
            bool outp = false;
            try
            {
                outp = outp || room.Grid[x - 1, y] == Room.GridSpace.Empty;
            }

            catch { return true; }
            try
            {
                outp = outp || room.Grid[x + 1, y] == Room.GridSpace.Empty;
            }

            catch { return true; }
            try
            {
                outp = outp || room.Grid[x, y - 1] == Room.GridSpace.Empty;
            }

            catch { return true; }
            try
            {
                outp = outp || room.Grid[x, y + 1] == Room.GridSpace.Empty;
            }

            catch { return true; }

            return outp;
        }


        void SpawnRoom()
        {
            Room room = _rooms[(int)_currentRoom.x, (int)_currentRoom.y];

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
                            Spawn(x, y, borderOnFill(x, y) ? WallObj : RockObj.transform.GetChild(Random.Range(0, 4)).gameObject);
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

            //spawn enemies
            print(room.EnemySpawnLocations.Count + " enemies");
            int _newestEnemy = Math.Min(_collectedInstruments, Enemies.Count - 1);
            int _enemyTypes = _newestEnemy + 1;

            int _newestEnemyPercentage = (int)(100.0 / _enemyTypes + 10.0);
            print("percentage: "+_newestEnemyPercentage);
            int _newestEnemyAmount = (int)((float)room.EnemySpawnLocations.Count / 100.0 * (float)_newestEnemyPercentage);
            _newestEnemyAmount = Math.Max(_newestEnemyAmount, 1); // make sure there is always at least one new enemy in the field
            print(_newestEnemyAmount + " " + Enemies[_newestEnemy].name);

            foreach (Vector2 _enemy in room.EnemySpawnLocations)
            {
                if (_newestEnemyAmount > 0)
                {
                    Spawn(_enemy.x, _enemy.y, Enemies[_newestEnemy]);
                    _newestEnemyAmount--;
                }
                else
                {
                    //check how many types of enemies may spawn and randomly get enemy to spawn from older ones
                    Spawn(_enemy.x, _enemy.y, Enemies[Random.Range(0, _newestEnemy)]);
                }
            }


            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                player.transform.position = new Vector3(75 - room.OffsetOfRoom.x, player.transform.position.y, 75 - room.OffsetOfRoom.y);
        }


        void DestroyCurrentRoom() // destroy a room to make space for a new one
        {
            foreach (GameObject g in _instantiated)
            {
                Destroy(g);
            }
        }

        void Spawn(float x, float y, GameObject toSpawn)
        {
            Room room = _rooms[(int)_currentRoom.x, (int)_currentRoom.y];

            //find the position to spawn
            Vector3 spawnPos = new Vector3(x, 0, y) - new Vector3(room.OffsetOfRoom.x, 0, room.OffsetOfRoom.y);
            //spawn object
            _instantiated.Add(Instantiate(toSpawn, spawnPos, Quaternion.identity)); // create object and add it to the list
        }



        public void OnEvent(RoomDoorDownEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Down);
        }

        public void OnEvent(RoomDoorUpEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Up);
        }

        public void OnEvent(RoomDoorLeftEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Left);
        }

        public void OnEvent(RoomDoorRightEvent invokedEvent)
        {
            ChangeCurrentRoom(DoorDirection.Right);
        }
    }
}
