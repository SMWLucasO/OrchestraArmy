using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Room
{
    public class Room : IListener<EnemyDeathEvent>
    {
        /// <summary>
        /// Enum for the different types of objects on the map
        /// </summary>
        public enum GridSpace
        {
            Empty,
            Floor,
            Wall,
            DoorU,
            DoorD,
            DoorL,
            DoorR
        };

        /// <summary>
        /// The field for this room
        /// </summary>
        public GridSpace[,] Grid { get; set; }

        /// <summary>
        /// The locations for enemies to spawn
        /// </summary>
        public List<Vector2> EnemySpawnLocations { get; set; }

        /// <summary>
        /// The height of this field
        /// </summary>
        public int RoomHeight { get; set; }

        /// <summary>
        /// The width of this field
        /// </summary>
        public int RoomWidth { get; set; }

        /// <summary>
        /// To see is the room is cleared of enemies yet
        /// </summary>
        public bool Beaten { get; set; }

        /// <summary>
        /// Size of the room in Vector2
        /// </summary>
        public Vector2 RoomSizeWorldUnits { get; set; } = new Vector2(150, 150);

        /// <summary>
        /// For getting the center of the room
        /// </summary>
        public Vector2 OffsetOfRoom { get; set; } = new Vector2(0, 0);

        private float _worldUnitsInOneGridCell = 1;

        private struct Walker
        {
            public Vector2 Dir;
            public Vector2 Pos;
        }

        private List<Walker> _walkers;

        private float _chanceWalkerChangeDir = .7f, _chanceWalkerSpawn = 0.2f;
        private float _chanceWalkerDestoy = 0.2f;

        private int _maxWalkers = 6;

        private float _percentToFill = 0.05f;

        private int _numberOfEnemies;
        
        private List<Vector2> _floors = new List<Vector2>();
        
        private int _distanceBetweenEnemies = 10;

        
        public Room(int numberOfEnemies, bool spawn = false, bool boss = false)
        {
            _numberOfEnemies = numberOfEnemies;
            
            if (spawn) 
            {
                CreateSpawn();
                CreateWalls();
                CreateDoors();
            }
            else 
            {
                if (boss) 
                {
                    BossSettings();
                }
                Setup();
                CreateFloors();
                CreateWalls();
                RemoveSingleWalls();
                CreateDoors();
            }
            
            if (!Beaten)
                CreateEnemySpawnLocationsRecursive();
        }

        /// <summary>
        /// Create room according to boss level settings
        /// </summary>
        private void BossSettings()
        {
            _chanceWalkerChangeDir = .99f;
            _chanceWalkerSpawn = 0.8f;
            _chanceWalkerDestoy = 0.8f;
            _maxWalkers = 20;
            _percentToFill = 0.1f;
        }

        private void Setup()
        {
            // Register enemy events.
            EventManager.Bind<EnemyDeathEvent>(this);

            _floors = new List<Vector2>();
            this.Beaten = false;

            // Find grid size
            RoomHeight = Mathf.RoundToInt(RoomSizeWorldUnits.x / _worldUnitsInOneGridCell);
            RoomWidth = Mathf.RoundToInt(RoomSizeWorldUnits.y / _worldUnitsInOneGridCell);

            // Create grid
            Grid = new GridSpace[RoomWidth, RoomHeight];

            // Set grid's default state
            for (int x = 0; x < RoomWidth - 1; x++)
            {
                for (int y = 0; y < RoomHeight - 1; y++)
                {
                    // Make every cell "empty"
                    Grid[x, y] = GridSpace.Empty;
                }
            }

            // Set first walker
            // Init list
            _walkers = new List<Walker>();

            // Create a walker 
            Walker newWalker = new Walker();
            newWalker.Dir = RandomDirection();

            // Find center of grid
            Vector2 spawnPos = new Vector2(Mathf.RoundToInt(RoomWidth / 2.0f),
                Mathf.RoundToInt(RoomHeight / 2.0f));
            newWalker.Pos = spawnPos;

            // Add walker to list
            _walkers.Add(newWalker);
        }
        
        void CreateSpawn()  //creates the spawn room layout 
        {
            this.Beaten = false;
            //find grid size
            RoomHeight = Mathf.RoundToInt(RoomSizeWorldUnits.x / _worldUnitsInOneGridCell);
            RoomWidth = Mathf.RoundToInt(RoomSizeWorldUnits.y / _worldUnitsInOneGridCell);
            //create grid
            Grid = new GridSpace[RoomWidth, RoomHeight];
            Vector2 center = RoomSizeWorldUnits / 2;
            for (int x = (int)-center.x; x < center.x; x++) {
                for (int y = (int)-center.y; y < center.y; y++) {
                    if (x*x+y*y<=100) {
                        Grid[(int) (x+center.x),(int) (y+center.y)] = GridSpace.Floor;
                    } else {
                        Grid[(int)(x+center.x),(int)(y+center.y)] = GridSpace.Empty;
                    }
                }
            }
        }
        
        /// <summary>
        /// Create floors with random walkers
        /// </summary>
        private void CreateFloors()
        {
            // Loop will not run forever
            int iterations = 0;
            do
            {
                // Create floor at position of every walker
                foreach (Walker myWalker in _walkers)
                {
                    // Add position to floor list
                    Grid[(int) myWalker.Pos.x, (int) myWalker.Pos.y] = GridSpace.Floor;
                    _floors.Add(new Vector2((int) myWalker.Pos.x, (int) myWalker.Pos.y));
                }

                // Chance: destroy walker
                // Might modify count while in this loop
                int numberChecks = _walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {
                    // Only if its not the only one, and at a low chance
                    if (Random.value < _chanceWalkerDestoy && _walkers.Count > 1)
                    {
                        // Only destroy one per iteration
                        _walkers.RemoveAt(i);
                        break;
                    }
                }

                // Chance: walker pick new direction
                for (int i = 0; i < _walkers.Count; i++)
                {
                    if (Random.value < _chanceWalkerChangeDir)
                    {
                        Walker thisWalker = _walkers[i];
                        thisWalker.Dir = RandomDirection();
                        _walkers[i] = thisWalker;
                    }
                }

                // Chance: spawn new walker
                // Might modify count while in this loop
                numberChecks = _walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {
                    // Only if # of walkers < max, and at a low chance
                    if (Random.value < _chanceWalkerSpawn && _walkers.Count < _maxWalkers)
                    {
                        // Create a walker 
                        Walker newWalker = new Walker();
                        newWalker.Dir = RandomDirection();
                        newWalker.Pos = _walkers[i].Pos;
                        _walkers.Add(newWalker);
                    }
                }

                // Move walkers
                for (int i = 0; i < _walkers.Count; i++)
                {
                    Walker thisWalker = _walkers[i];
                    thisWalker.Pos += thisWalker.Dir;
                    _walkers[i] = thisWalker;
                }

                // Avoid boarder of grid
                for (int i = 0; i < _walkers.Count; i++)
                {
                    Walker thisWalker = _walkers[i];

                    // Clamp x,y to leave a 1 space boarder: leave room for walls
                    thisWalker.Pos.x = Mathf.Clamp(thisWalker.Pos.x, 1, RoomWidth - 2);
                    thisWalker.Pos.y = Mathf.Clamp(thisWalker.Pos.y, 1, RoomHeight - 2);
                    _walkers[i] = thisWalker;
                }

                // Check to exit loop
                if ((float) NumberOfFloors() / (float) Grid.Length > _percentToFill)
                {
                    break;
                }

                iterations++;
            } while (iterations < 100000);
        }


        /// <summary>
        /// Create walls surrounding the floors
        /// </summary>
        private void CreateWalls()
        {
            // Loop though every grid space
            for (int x = 0; x < RoomWidth - 1; x++)
            {
                for (int y = 0; y < RoomHeight - 1; y++)
                {
                    // If theres a floor, check the spaces around it
                    if (Grid[x, y] == GridSpace.Floor)
                    {
                        // If any surrounding spaces are empty, place a wall
                        if (Grid[x, y + 1] == GridSpace.Empty)
                        {
                            Grid[x, y + 1] = GridSpace.Wall;
                        }

                        if (Grid[x, y - 1] == GridSpace.Empty)
                        {
                            Grid[x, y - 1] = GridSpace.Wall;
                        }

                        if (Grid[x + 1, y] == GridSpace.Empty)
                        {
                            Grid[x + 1, y] = GridSpace.Wall;
                        }

                        if (Grid[x - 1, y] == GridSpace.Empty)
                        {
                            Grid[x - 1, y] = GridSpace.Wall;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replace single walls with a floor
        /// </summary>
        private void RemoveSingleWalls()
        {
            // Loop though every grid space
            for (int x = 0; x < RoomWidth - 1; x++)
            {
                for (int y = 0; y < RoomHeight - 1; y++)
                {
                    // If theres a wall, check the spaces around it
                    if (Grid[x, y] == GridSpace.Wall)
                    {
                        // Assume all space around wall are floors
                        bool allFloors = true;
                        
                        // Check each side to see if they are all floors
                        for (int checkX = -1; checkX <= 1; checkX++)
                        {
                            for (int checkY = -1; checkY <= 1; checkY++)
                            {
                                if (x + checkX < 0 || x + checkX > RoomWidth - 1 ||
                                    y + checkY < 0 || y + checkY > RoomHeight - 1)
                                {
                                    // Skip checks that are out of range
                                    continue;
                                }

                                if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                                {
                                    // Skip corners and center
                                    continue;
                                }

                                if (Grid[x + checkX, y + checkY] != GridSpace.Floor)
                                {
                                    allFloors = false;
                                }
                            }
                        }

                        if (allFloors)
                        {
                            Grid[x, y] = GridSpace.Floor;
                            // Add position to floor list
                            _floors.Add(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Place the four doors around the map
        /// </summary>
        private void CreateDoors()
        {
            // Calculate the center of the made map
            int minX = RoomWidth, minY = RoomHeight, maxX = 0, maxY = 0; 
            for (int x = 0; x < RoomWidth; x++)
            {
                for (int y = 0; y < RoomHeight; y++)
                {
                    if (Grid[x, y] != GridSpace.Empty)
                    {
                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            int centerX = (int) (minX + maxX) / 2, centerY = (int) (minY + maxY) / 2;

            // When spawning the rooms, this vector will center them
            OffsetOfRoom = new Vector2(centerX, centerY);

            // Down -> up (makes the bottom door)
            for (int i = 0; i < RoomHeight; i++) 
            {
                if (Grid[centerX, i] == GridSpace.Wall)
                {
                    Grid[centerX, i] = GridSpace.DoorD;
                    break;
                }
            }

            // Up -> down (makes the top door)
            for (int i = RoomHeight - 1; i >= 0; i--) 
            {
                if (Grid[centerX, i] == GridSpace.Wall)
                {
                    Grid[centerX, i] = GridSpace.DoorU;
                    break;
                }
            }

            // Left -> right (makes the left door)
            for (int i = 0; i < RoomWidth; i++)
            {
                if (Grid[i, centerY] == GridSpace.Wall)
                {
                    Grid[i, centerY] = GridSpace.DoorL;
                    break;
                }
            }

            // Right -> left (makes the right door)
            for (int i = RoomWidth - 1; i >= 0; i--)
            {
                if (Grid[i, centerY] == GridSpace.Wall)
                {
                    Grid[i, centerY] = GridSpace.DoorR;
                    break;
                }
            }
        }

        /// <summary>
        /// Create enemy spawn locations with a try-catch
        /// </summary>
        private void CreateEnemySpawnLocationsRecursive()
        {
            try
            {
                // Try to create the enemy spawn locations
                CreateEnemySpawnLocations();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Spreading out with distancebetweenenemies was not possible
                //If distance between enemies is higher than 0
                if (_distanceBetweenEnemies > 0) 
                {
                    // Subtract 1 from distance between enemies
                    _distanceBetweenEnemies--;
                    Debug.Log("Distance between enemies too high. New distance: " + _distanceBetweenEnemies);
                    CreateEnemySpawnLocationsRecursive();
                }
                else
                {
                    // There are more enemies than floors in this room, quit application
                    Debug.Log("Too many enemies for this room.");
                    Application.Quit();
                }
            }
        }

        /// <summary>
        /// Create enemies on floor-tiles
        /// </summary>
        private void CreateEnemySpawnLocations()
        {
            EnemySpawnLocations = new List<Vector2>();
            Vector2 _location;
            List<Vector2> _possibleFloors = _floors;

            // For every enemy
            for (int i = _numberOfEnemies; i > 0; i--)
            {
                // Get random location
                _location = _possibleFloors[Random.Range(0, _possibleFloors.Count)]; 
                
                // Add location to enemy spawn
                EnemySpawnLocations.Add(_location); 

                // Remove from _possiblefloors in a manhatten distance around enemy to prevent 
                // groups of enemies in one spot
                List<Vector2> _tempFloors = new List<Vector2>();
                Vector2 vec;
                int diff;
                
                foreach (Vector2 floor in _possibleFloors)
                {
                    // Get distance between current location and possible floor
                    vec = _location - floor; 
                    
                    // Get Manhattan distance
                    diff = (int) (Math.Abs(vec.x) + Math.Abs(vec.y)); 
                    
                    // If difference is large enough to keep enemies apart
                    if (diff > _distanceBetweenEnemies) 
                    {
                        // Add floor to save to temp list
                        _tempFloors.Add(floor); 
                    }
                }

                // Make temp list permanent
                _possibleFloors = _tempFloors; 
            }
        }

        /// <summary>
        /// Get a random direction from four options
        /// </summary>
        private Vector2 RandomDirection()
        {
            // Pick random int between 0 and 3
            int choice = Mathf.FloorToInt(Random.value * 3.99f);
            
            // Use that int to choose a direction
            return choice switch
            {
                0 => Vector2.down,
                1 => Vector2.left,
                2 => Vector2.up,
                _ => Vector2.right
            };
        }

        /// <summary>
        /// Get number of floors in this room
        /// </summary>
        private int NumberOfFloors()
        {
            int count = 0;
            foreach (GridSpace space in Grid)
            {
                if (space == GridSpace.Floor)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            // One enemy less
            _numberOfEnemies--;

            // If all enemies are dead
            if (_numberOfEnemies < 1)
                // Level beaten
                Beaten = true;
        }
    }
}