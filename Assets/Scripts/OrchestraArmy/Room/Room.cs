using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using OrchestraArmy.Room.Data;

namespace OrchestraArmy.Room
{
    
    public abstract class Room
    {
     
        /// <summary>
        /// The controller for this room.
        /// </summary>
        [field: SerializeField]
        public RoomController RoomController { get; set; }
        
        /// <summary>
        /// The grid of the room, to be filled with objects.
        /// </summary>
        public GridSpace[,] Grid { get; set; }

        /// <summary>
        /// The locations where the enemies are to be spawned at.
        /// </summary>
        public List<Vector2> EnemySpawnLocations { get; set; }
            = new List<Vector2>();


        /// <summary>
        /// The coordinates of the portals on the map.
        /// 0 = up
        /// 1 = down
        /// 2 = left
        /// 3 = right
        /// </summary>
        public Vector2[] DoorPositions { get; set; }
            = new Vector2[4];

        /// <summary>
        /// The coordinates of the portals on the grid.
        /// 0 = up
        /// 1 = down
        /// 2 = left
        /// 3 = right
        /// </summary>
        private Vector2[] _gridDoorPositions 
            = new Vector2[4];
        
        /// <summary>
        /// The width of the room.
        /// </summary>
        public int RoomWidth { get; set; }
        
        /// <summary>
        /// The height of the room.
        /// </summary>
        public int RoomHeight { get; set; }
        
        /// <summary>
        /// Bool to determine whether the room was cleared of enemies.
        /// </summary>
        public bool RoomIsCleared { get; set; }
        
        /// <summary>
        /// Size of the room in Vector2
        /// </summary>
        public Vector2 RoomSizeWorldUnits { get; set; } 
            = new Vector2(75, 75);

        /// <summary>
        /// For getting the center of the room
        /// </summary>
        public Vector2 OffsetOfRoom { get; set; } 
            = new Vector2(0, 0);

        /// <summary>
        /// Data for spawning enemies.
        /// </summary>
        public EnemySpawnData EnemySpawnData { get; set; }
            = new EnemySpawnData();

        /// <summary>
        /// Data for the room generation.
        /// </summary>
        public RoomGenerationData RoomGenerationData { get; set; }
            = new RoomGenerationData();
        
        /// <summary>
        /// Generate the room.
        /// </summary>
        public virtual void Generate()
        {
            SetupSettings();

            Setup();
            CreateFloors();
            CreateWalls();
            RemoveSingleWalls();
            CreateDoors();

            if (!RoomIsCleared)
                RecursivelyGenerateEnemySpawnLocation();

        }

        private void Setup()
        {

            RoomHeight = Mathf.RoundToInt(RoomSizeWorldUnits.x / RoomGenerationData.WorldUnitsInOneGridCell);
            RoomWidth = Mathf.RoundToInt(RoomSizeWorldUnits.y / RoomGenerationData.WorldUnitsInOneGridCell);
            
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
            
            // Create a walker 
            Walker newWalker = new Walker {Direction = RandomDirection()};

            // Find center of grid
            Vector2 spawnPosition = new Vector2(Mathf.RoundToInt(RoomWidth / 2.0f),
                Mathf.RoundToInt(RoomHeight / 2.0f));
            newWalker.Position = spawnPosition;

            // Add walker to list
            RoomGenerationData.Walkers.Add(newWalker);
        }
        
        /// <summary>
        /// Create floors with random walkers
        /// </summary>
        protected void CreateFloors()
        {
            // Loop will not run forever
            int iterations = 0;
            do
            {
                // Create floor at position of every walker
                foreach (Walker myWalker in RoomGenerationData.Walkers)
                {
                    // Add position to floor list
                    Grid[(int) myWalker.Position.x, (int) myWalker.Position.y] = GridSpace.Floor;
                    EnemySpawnData.Floors.Add(new Vector2((int) myWalker.Position.x, (int) myWalker.Position.y));
                }

                // Chance: destroy walker
                // Might modify count while in this loop
                int numberChecks = RoomGenerationData.Walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {
                    // Only if its not the only one, and at a low chance
                    if (Random.value < RoomGenerationData.WalkerDestructionChance && RoomGenerationData.Walkers.Count > 1)
                    {
                        // Only destroy one per iteration
                        RoomGenerationData.Walkers.RemoveAt(i);
                        break;
                    }
                }

                // Chance: walker pick new direction
                for (int i = 0; i < RoomGenerationData.Walkers.Count; i++)
                {
                    if (Random.value < RoomGenerationData.WalkerDirectionChangeChance)
                    {
                        Walker thisWalker = RoomGenerationData.Walkers[i];
                        thisWalker.Direction = RandomDirection();
                        RoomGenerationData.Walkers[i] = thisWalker;
                    }
                }

                // Chance: spawn new walker
                // Might modify count while in this loop
                numberChecks = RoomGenerationData.Walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {
                    // Only if # of walkers < max, and at a low chance
                    if (Random.value < RoomGenerationData.WalkerSpawnChance 
                        && RoomGenerationData.Walkers.Count < RoomGenerationData.MaxAliveWalkers)
                    {
                        // Create a walker 
                        Walker newWalker = new Walker();
                        newWalker.Direction = RandomDirection();
                        newWalker.Position = RoomGenerationData.Walkers[i].Position;
                        RoomGenerationData.Walkers.Add(newWalker);
                    }
                }

                // Move walkers
                for (int i = 0; i < RoomGenerationData.Walkers.Count; i++)
                {
                    Walker thisWalker = RoomGenerationData.Walkers[i];
                    thisWalker.Position += thisWalker.Direction;
                    RoomGenerationData.Walkers[i] = thisWalker;
                }

                // Avoid boarder of grid
                for (int i = 0; i < RoomGenerationData.Walkers.Count; i++)
                {
                    Walker thisWalker = RoomGenerationData.Walkers[i];

                    // Clamp x,y to leave a 1 space boarder: leave room for walls
                    thisWalker.Position.x = Mathf.Clamp(thisWalker.Position.x, 1, RoomWidth - 2);
                    thisWalker.Position.y = Mathf.Clamp(thisWalker.Position.y, 1, RoomHeight - 2);
                    RoomGenerationData.Walkers[i] = thisWalker;
                }

                // Check to exit loop
                // might be able to replace NumberOfFloors with the Floors.Count within EnemySpawnData.
                if ((float) NumberOfFloors() / (float) Grid.Length > RoomGenerationData.PercentToFill)
                {
                    break;
                }

                iterations++;
            } while (iterations < 100000);
        }

        /// <summary>
        /// Create walls surrounding the floors
        /// </summary>
        protected void CreateWalls()
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
        protected void RemoveSingleWalls()
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
                            EnemySpawnData.Floors.Add(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Place the four doors around the map
        /// </summary>
        protected virtual void CreateDoors()
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
                    _gridDoorPositions[1] = new Vector2(centerX, i);
                    break;
                }
            }

            // Up -> down (makes the top door)
            for (int i = RoomHeight - 1; i >= 0; i--) 
            {
                if (Grid[centerX, i] == GridSpace.Wall)
                {
                    Grid[centerX, i] = GridSpace.DoorU;
                    _gridDoorPositions[0] = new Vector2(centerX, i);
                    break;
                }
            }

            // Left -> right (makes the left door)
            for (int i = 0; i < RoomWidth; i++)
            {
                if (Grid[i, centerY] == GridSpace.Wall)
                {
                    Grid[i, centerY] = GridSpace.DoorL;
                    _gridDoorPositions[2] = new Vector2(i, centerY);
                    break;
                }
            }

            // Right -> left (makes the right door)
            for (int i = RoomWidth - 1; i >= 0; i--)
            {
                if (Grid[i, centerY] == GridSpace.Wall)
                {
                    Grid[i, centerY] = GridSpace.DoorR;
                    _gridDoorPositions[3] = new Vector2(i, centerY);
                    break;
                }
            }
        }

        /// <summary>
        /// Create enemy spawn locations with a try-catch
        /// </summary>
        protected void RecursivelyGenerateEnemySpawnLocation()
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
                if (EnemySpawnData.DistanceBetweenEnemies > 0) 
                {
                    // Subtract 1 from distance between enemies
                    EnemySpawnData.DistanceBetweenEnemies--;
                    Debug.Log("Distance between enemies too high. New distance: " + EnemySpawnData.DistanceBetweenEnemies);
                    RecursivelyGenerateEnemySpawnLocation();
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
        protected void CreateEnemySpawnLocations()
        {
            EnemySpawnLocations = new List<Vector2>();
            Vector2 location;
            List<Vector2> possibleFloors = EnemySpawnData.Floors;

            // For every enemy
            for (int i = EnemySpawnData.NumberOfEnemies; i > 0; i--)
            {
                // Get random location
                location = possibleFloors[Random.Range(0, possibleFloors.Count)]; 
                
                // Add location to enemy spawn
                EnemySpawnLocations.Add(location); 

                // Remove from possibleFloors in a manhattan distance around enemy to prevent 
                // groups of enemies in one spot
                List<Vector2> tempFloors = new List<Vector2>();
                Vector2 vec;
                int diff;
                
                foreach (Vector2 floor in possibleFloors)
                {
                    // Get distance between current location and possible floor
                    vec = location - floor; 
                    
                    // Get Manhattan distance
                    diff = (int) (Math.Abs(vec.x) + Math.Abs(vec.y)); 
                    
                    // If difference is large enough to keep enemies apart
                    if (diff > EnemySpawnData.DistanceBetweenEnemies) 
                    {
                        // Add floor to save to temp list
                        tempFloors.Add(floor); 
                    }
                }

                // Make temp list permanent
                possibleFloors = tempFloors; 
            }
        }

        /// <summary>
        /// Get number of floors in this room
        /// </summary>
        protected float NumberOfFloors()
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
        /// Get a random direction from four options
        /// </summary>
        protected Vector2 RandomDirection()
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
        /// Set the settings for the room generation.
        /// </summary>
        public abstract void SetupSettings();


        /// <summary>
        /// Get the player spawn location for the specified door.
        /// </summary>
        /// <param name="doorDirection"></param>
        /// <returns></returns>
        public virtual Vector3 GetPlayerSpawnPosition(DoorDirection doorDirection)
        {
            Vector2 gridDoorPosition = GetOppositeDoorPosition(doorDirection, _gridDoorPositions);
            Vector2 mapDoorPosition = GetOppositeDoorPosition(doorDirection, DoorPositions);

            return GetFreePositionAroundPoint(gridDoorPosition, mapDoorPosition);
        }

        /// <summary>
        /// Get the opposite door of the given input.
        /// </summary>
        /// <param name="doorDirection"></param>
        /// <param name="doorPositions"></param>
        /// <returns></returns>
        private Vector2 GetOppositeDoorPosition(DoorDirection doorDirection, Vector2[] doorPositions) =>
            doorDirection switch
            {
                DoorDirection.Down => doorPositions[0],
                DoorDirection.Up => doorPositions[1],
                DoorDirection.Right => doorPositions[2],
                _ => doorPositions[3]
            };
        
        /// <summary>
        /// Get a position to be placed at at a given point.
        /// </summary>
        /// <param name="gridDoorPosition"></param>
        /// <param name="mapDoorPosition"></param>
        /// <returns></returns>
        private Vector3 GetFreePositionAroundPoint(Vector2 gridDoorPosition, Vector2 mapDoorPosition)
        {

            // Positions to be checked in this specific order.
            (int, int)[] positions =
            {
                (1, 0), (-1, 0), (0, -1), (0, 1), (-1, -1), (1, 1), (-1, 1), (1, -1),
            };

            foreach (var position in positions)
            {
                if (gridDoorPosition.x + position.Item1 < 0 || gridDoorPosition.y + position.Item2 < 0)
                    continue;
                
                if (gridDoorPosition.x + position.Item1 >= Grid.GetLength(0) || gridDoorPosition.y + position.Item2 >= Grid.GetLength(1))
                    continue;
                    
                if (Grid[(int) (gridDoorPosition.x + position.Item1), (int) (gridDoorPosition.y + position.Item2)] == GridSpace.Floor)
                    return new Vector3(mapDoorPosition.x + position.Item1, 0.5f, mapDoorPosition.y + position.Item2);
            }
            
            // center of map.
            return new Vector3(RoomSizeWorldUnits.x / 2 - OffsetOfRoom.x, 0.5f,
                RoomSizeWorldUnits.y / 2 - OffsetOfRoom.y);
        }
        
    }
    
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
    }

}