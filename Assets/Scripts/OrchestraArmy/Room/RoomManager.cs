using System;
using System.Collections;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Room.Data;
using OrchestraArmy.Room.Factories;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Room
{
    public class RoomManager : MonoBehaviour, IListener<RoomDoorDownEvent>, IListener<RoomDoorUpEvent>,
        IListener<RoomDoorLeftEvent>, IListener<RoomDoorRightEvent>, IListener<PlayerDeathEvent>
    {
        
        /// <summary>
        /// The single instance of the RoomManager.
        /// </summary>
        public static RoomManager Instance { get; set; }

        /// <summary>
        /// The count of currently collected instruments, starting at 0.
        /// The count ends at 3.
        /// </summary>
        [field: SerializeField]
        public int CollectedInstrumentCount { get; set; } = 0;

        /// <summary>
        /// The generator of our rooms.
        /// </summary>
        public IRoomFactory RoomFactory { get; set; }
            = new DefaultRoomFactory();
        
        /// <summary>
        /// Our room grid, a 20x20 array.
        /// </summary>
        public Room[,] Rooms { get; set; }

        /// <summary>
        /// The index of the currently enabled room.
        /// </summary>
        public Vector2Int CurrentRoomIndex { get; set; } 
            = new Vector2Int(10, 10);

        /// <summary>
        /// The amount of rooms where all monsters were defeated.
        /// </summary>
        public int RoomsCleared { get; set; } = 0;

        /// <summary>
        /// A boolean forcing the next room to be a boss room.
        /// </summary>
        public bool ForceNextRoomIsBoss = false;
        
        private Player _player;
        
        /// <summary>
        /// The current room of the player.
        /// </summary>
        public Room CurrentRoom
        {
            get => Rooms[CurrentRoomIndex.x, CurrentRoomIndex.y];
            set => Rooms[CurrentRoomIndex.x, CurrentRoomIndex.y] = value;
        }
        
        private int _enemiesFib1 = 1;
        private int _enemiesFib2 = 1;
        private int _enemiesNow;
        
        /// <summary>
        /// The prefabs for the rooms.
        /// </summary>
        [field: SerializeField]
        public RoomPrefabData RoomPrefabData { get; set; }

        private void Awake()
        {
            Instance = this;
            _player = GameObject.Find("Player").GetComponent<Player>();

            Rooms = new Room[20, 20];
            
            this.GenerateRoom(new Vector2(10, 10), RoomType.StartingRoom);
            RoomsCleared = 1;
        }

        private void OnEnable()
        {
            EventManager.Bind<RoomDoorDownEvent>(this);
            EventManager.Bind<RoomDoorUpEvent>(this);
            EventManager.Bind<RoomDoorLeftEvent>(this);
            EventManager.Bind<RoomDoorRightEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.Unbind<RoomDoorDownEvent>(this);
            EventManager.Unbind<RoomDoorUpEvent>(this);
            EventManager.Unbind<RoomDoorLeftEvent>(this);
            EventManager.Unbind<RoomDoorRightEvent>(this);
            EventManager.Unbind<PlayerDeathEvent>(this);
        }

        /// <summary>
        /// Generate a room of the specified type to be placed at the specified location.
        /// Will not do anything if a room already exists at the specified position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="room"></param>
        public void GenerateRoom(Vector2 position, RoomType room)
        {

            if (room == RoomType.StartingRoom)
            {
                _enemiesFib1 = 0;
                _enemiesFib2 = 1;
                _enemiesNow = 0;
            }
            
            CurrentRoom ??= room switch
            {
                RoomType.BossRoom => RoomFactory.MakeBossRoom(),
                RoomType.MonsterRoom => RoomFactory.MakeMonsterRoom(),
                RoomType.StartingRoom => RoomFactory.MakeStartingRoom(),
                _ => null
            };
            
            if (CurrentRoom != null)
            {
                CurrentRoom.RoomController.Room = CurrentRoom;
                if (!CurrentRoom.RoomIsCleared)
                    CurrentRoom.EnemySpawnData.NumberOfEnemies = GetNumberOfEnemies();
                
                CurrentRoom.Generate();
                
                CurrentRoom.RoomController.RegisterEvents();
                
                StartCoroutine(nameof(SpawnRoom));
            }
        }
        
        /// <summary>
        /// Destroy all current rooms.
        /// </summary>
        public void DestroyRooms()
        {
            foreach (var room in Rooms)
            {
                room?.RoomController.UnregisterEvents();
                room?.RoomController?.DestroyRoom();
            }

            Rooms = new Room[20, 20];
        }
        
        /// <summary>
        /// Move the player to the next room.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveToNextRoom(DoorDirection direction)
        {
            // room has to be cleared before continuing.
            if (!CurrentRoom.RoomIsCleared)
                return;
            
            // Clear previous field
            CurrentRoom.RoomController.DestroyRoom();

            // unregister the events of the current room
            CurrentRoom.RoomController.UnregisterEvents();

            // Move to another position on the room grid
            var currentRoomIndex = CurrentRoomIndex;
            switch (direction)
            {
                case DoorDirection.Left:
                    currentRoomIndex.x += 1;
                    break;

                case DoorDirection.Right:
                    currentRoomIndex.x += 1;
                    break;

                case DoorDirection.Up:
                    currentRoomIndex.y -= 1;
                    break;

                case DoorDirection.Down:
                    currentRoomIndex.y += 1;
                    break;

                default:
                    currentRoomIndex += new Vector2Int(0, -1);
                    break;
            }

            CurrentRoomIndex = currentRoomIndex;

            RoomType roomType = DecideNextRoom();

            // generate the room.
            GenerateRoom(CurrentRoomIndex, roomType);
            
            // clear any projectiles currently in the room.
            foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile")) 
                Destroy(projectile);

            if (_player != null)
            {
                _player.transform.position = CurrentRoom.GetPlayerSpawnPosition(direction);
            }
        }
        
        // room object generation //
        
        /// <summary>
        /// Spawn room into game (corotine)
        /// </summary>
        private IEnumerator SpawnRoom()
        {
            // Spawn static game room objects into game
            for (int x = 0; x < CurrentRoom.RoomWidth; x++)
            {
                for (int y = 0; y < CurrentRoom.RoomHeight; y++)
                {
                    switch (CurrentRoom.Grid[x, y])
                    {
                        case GridSpace.Empty:
                            Spawn(x, y, RoomPrefabData.FillObj);
                            break;

                        case GridSpace.Floor:
                            Spawn(x, y, RoomPrefabData.FloorObj);
                            if (Random.value < 0.1f)
                            {
                                Spawn(x, y, RoomPrefabData.RubbleObj.transform.GetChild(Random.Range(0, 3)).gameObject);
                            }

                            break;
                        case GridSpace.Wall:
                            Spawn(x, y,
                                BorderOnFill(x, y)
                                    ? RoomPrefabData.WallObj
                                    : RoomPrefabData.RockObj.transform.GetChild(Random.Range(0, 4)).gameObject);
                            break;

                        case GridSpace.DoorU:
                            Spawn(x, y, RoomPrefabData.DoorUpObj);
                            CurrentRoom.DoorPositions[0] = new Vector2(x, y) - CurrentRoom.OffsetOfRoom;
                            break;

                        case GridSpace.DoorD:
                            Spawn(x, y, RoomPrefabData.DoorDownObj);
                            CurrentRoom.DoorPositions[1] = new Vector2(x, y) - CurrentRoom.OffsetOfRoom;
                            break;

                        case GridSpace.DoorL:
                            Spawn(x, y, RoomPrefabData.DoorLeftObj);
                            CurrentRoom.DoorPositions[2] = new Vector2(x, y) - CurrentRoom.OffsetOfRoom;
                            break;

                        case GridSpace.DoorR:
                            Spawn(x, y, RoomPrefabData.DoorRightObj);
                            CurrentRoom.DoorPositions[3] = new Vector2(x, y) - CurrentRoom.OffsetOfRoom;
                            break;
                    }
                }
            }
            
            yield return null;
            // Bake navMesh into game
            NavMeshSurface[] objs = FindObjectsOfType(typeof(NavMeshSurface)) as NavMeshSurface[];
            objs[0].BuildNavMesh();
            
            yield return null;
            // Spawn enemies into game
            int newestEnemy = Math.Min(
                CollectedInstrumentCount,
                RoomPrefabData.Enemies.Count - 1
                );

            int enemyTypes = newestEnemy + 1;

            int newestEnemyPercentage = (int) (100.0 / enemyTypes + 10.0);
            int newestEnemyAmount =
                (int) ((float) CurrentRoom.EnemySpawnLocations.Count / 100.0 * (float) newestEnemyPercentage);
            
            // Make sure there is always at least one new enemy in the field
            newestEnemyAmount =
                Math.Max(newestEnemyAmount, 1); 

            foreach (Vector2 enemy in CurrentRoom.EnemySpawnLocations)
            {
                if (newestEnemyAmount > 0)
                {
                    Spawn(enemy.x, enemy.y, RoomPrefabData.Enemies[newestEnemy].gameObject);
                    newestEnemyAmount--;
                }
                else
                {
                    // Check how many types of enemies may spawn and randomly get enemy to spawn from older ones
                    Spawn(enemy.x, enemy.y, RoomPrefabData.Enemies[Random.Range(0, newestEnemy)].gameObject);
                }
            }
            
        }

        /// <summary>
        /// Check if tile directly borders an empty tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private bool BorderOnFill(int x, int y)
        {
            bool outp = false;
            try
            {
                outp = outp || CurrentRoom.Grid[x - 1, y] == GridSpace.Empty;
                outp = outp || CurrentRoom.Grid[x + 1, y] == GridSpace.Empty;
                outp = outp || CurrentRoom.Grid[x, y - 1] == GridSpace.Empty;
                outp = outp || CurrentRoom.Grid[x, y + 1] == GridSpace.Empty;
            }
            catch
            {
                return true;
            }
            

            return outp;
        }
        
        /// <summary>
        /// Spawn object into game
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="toSpawn"></param>
        private void Spawn(float x, float y, GameObject toSpawn)
        {
            Room room = CurrentRoom;

            // Find the position to spawn
            Vector3 spawnPos = new Vector3(x, 0, y) - new Vector3(room.OffsetOfRoom.x, 0, room.OffsetOfRoom.y);
            // Spawn object
            // create object and add it to the list
            CurrentRoom.RoomController.Objects.Add(Instantiate(toSpawn, spawnPos,
                Quaternion.identity)); 
        }
        
        // END room object generation //

        /// <summary>
        /// Determine the type of the next room to be generated
        /// </summary>
        /// <returns></returns>
        private RoomType DecideNextRoom()
        {
            // calculation for chance boss room (after 5 rooms +20% per room)
            if (Random.value < 0.1f * (RoomsCleared - 5 + Math.Abs(RoomsCleared - 5)) || ForceNextRoomIsBoss)
                return RoomType.BossRoom;
            else
                return RoomsCleared == 0 ? RoomType.StartingRoom : RoomType.BossRoom;
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

        public void OnEvent(RoomDoorDownEvent invokedEvent)
            => MoveToNextRoom(DoorDirection.Down);

        public void OnEvent(RoomDoorUpEvent invokedEvent)
            => MoveToNextRoom(DoorDirection.Up);

        public void OnEvent(RoomDoorLeftEvent invokedEvent)
            => MoveToNextRoom(DoorDirection.Left);

        public void OnEvent(RoomDoorRightEvent invokedEvent)
            => MoveToNextRoom(DoorDirection.Right);

        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            _enemiesFib1 = 0;
            _enemiesFib2 = 1;
        }
        
    }
}