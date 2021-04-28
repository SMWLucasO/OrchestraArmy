using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level
{
    public enum GridSpace { Empty, Floor, Wall, DoorU, DoorD, DoorL, DoorR };
    public GridSpace[,] Grid;
    public int RoomHeight, RoomWidth;
    public bool Beaten;
    public Vector2 RoomSizeWorldUnits = new Vector2(150, 150);
    public Vector2 OffsetOfRoom = new Vector2(0, 0);
    private float _worldUnitsInOneGridCell = 1;
    struct Walker
    {
        public Vector2 Dir;
        public Vector2 Pos;
    }
    private List<Walker> _walkers;
    private float _chanceWalkerChangeDir = .7f, _chanceWalkerSpawn = 0.2f;
    private float _chanceWalkerDestoy = 0.2f;
    private int _maxWalkers = 6;
    private float _percentToFill = 0.05f;
    
    public Level(bool boss = false)
    {
        if (boss) {
            BossSettings();
        }
        Setup();
        CreateFloors();
        CreateWalls();
        RemoveSingleWalls();
        CreateDoors();
    }

    void BossSettings()
    {
        _chanceWalkerChangeDir = .99f;
        _chanceWalkerSpawn = 0.8f;
        _chanceWalkerDestoy = 0.8f;
        _maxWalkers = 20;
        _percentToFill = 0.1f;
    }
    
    void Setup()
    {
		this.Beaten = false;
        //find grid size
        RoomHeight = Mathf.RoundToInt(RoomSizeWorldUnits.x / _worldUnitsInOneGridCell);
        RoomWidth = Mathf.RoundToInt(RoomSizeWorldUnits.y / _worldUnitsInOneGridCell);
        //create grid
        Grid = new GridSpace[RoomWidth, RoomHeight];
        //set grid's default state
        for (int x = 0; x < RoomWidth - 1; x++)
        {
            for (int y = 0; y < RoomHeight - 1; y++)
            {
                //make every cell "empty"
                Grid[x, y] = GridSpace.Empty;
            }
        }
        //set first walker
        //init list
        _walkers = new List<Walker>();
        //create a walker 
        Walker newWalker = new Walker();
        newWalker.Dir = RandomDirection();
        //find center of grid
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(RoomWidth / 2.0f),
                                        Mathf.RoundToInt(RoomHeight / 2.0f));
        newWalker.Pos = spawnPos;
        //add walker to list
        _walkers.Add(newWalker);
    }
    
    void CreateFloors()
    {
        int iterations = 0;//loop will not run forever
        do
        {
            //create floor at position of every walker
            foreach (Walker myWalker in _walkers)
            {
                Grid[(int)myWalker.Pos.x, (int)myWalker.Pos.y] = GridSpace.Floor;
            }
            //chance: destroy walker
            int numberChecks = _walkers.Count; //might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                //only if its not the only one, and at a low chance
                if (Random.value < _chanceWalkerDestoy && _walkers.Count > 1)
                {
                    _walkers.RemoveAt(i);
                    break; //only destroy one per iteration
                }
            }
            //chance: walker pick new direction
            for (int i = 0; i < _walkers.Count; i++)
            {
                if (Random.value < _chanceWalkerChangeDir)
                {
                    Walker thisWalker = _walkers[i];
                    thisWalker.Dir = RandomDirection();
                    _walkers[i] = thisWalker;
                }
            }
            //chance: spawn new walker
            numberChecks = _walkers.Count; //might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                //only if # of walkers < max, and at a low chance
                if (Random.value < _chanceWalkerSpawn && _walkers.Count < _maxWalkers)
                {
                    //create a walker 
                    Walker newWalker = new Walker();
                    newWalker.Dir = RandomDirection();
                    newWalker.Pos = _walkers[i].Pos;
                    _walkers.Add(newWalker);
                }
            }
            //move walkers
            for (int i = 0; i < _walkers.Count; i++)
            {
                Walker thisWalker = _walkers[i];
                thisWalker.Pos += thisWalker.Dir;
                _walkers[i] = thisWalker;
            }
            //avoid boarder of grid
            for (int i = 0; i < _walkers.Count; i++)
            {
                Walker thisWalker = _walkers[i];
                //clamp x,y to leave a 1 space boarder: leave room for walls
                thisWalker.Pos.x = Mathf.Clamp(thisWalker.Pos.x, 1, RoomWidth - 2);
                thisWalker.Pos.y = Mathf.Clamp(thisWalker.Pos.y, 1, RoomHeight - 2);
                _walkers[i] = thisWalker;
            }
            //check to exit loop
            if ((float)NumberOfFloors() / (float)Grid.Length > _percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < 100000);
    }
    
    void CreateWalls()
    {
        //loop though every grid space
        for (int x = 0; x < RoomWidth - 1; x++)
        {
            for (int y = 0; y < RoomHeight - 1; y++)
            {
                //if theres a floor, check the spaces around it
                if (Grid[x, y] == GridSpace.Floor)
                {
                    //if any surrounding spaces are empty, place a wall
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
    
    void RemoveSingleWalls()
    {
        //loop though every grid space
        for (int x = 0; x < RoomWidth - 1; x++)
        {
            for (int y = 0; y < RoomHeight - 1; y++)
            {
                //if theres a wall, check the spaces around it
                if (Grid[x, y] == GridSpace.Wall)
                {
                    //assume all space around wall are floors
                    bool allFloors = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > RoomWidth - 1 ||
                                y + checkY < 0 || y + checkY > RoomHeight - 1)
                            {
                                //skip checks that are out of range
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                //skip corners and center
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
                    }
                }
            }
        }
    }

    void CreateDoors()  //place the four doors around the map
    {
        int minX = RoomWidth, minY = RoomHeight, maxX = 0, maxY = 0;    //calculate the center of the made map
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

        int centerX = (int)(minX + maxX) / 2, centerY = (int)(minY + maxY) / 2;

        OffsetOfRoom = new Vector2(centerX, centerY);   //when spawning the rooms, this vector will center them

        for (int i = 0; i < RoomHeight; i++)    //down -> up (makes the bottom door)
        {
            if (Grid[centerX, i] == GridSpace.Wall)
            {
                Grid[centerX, i] = GridSpace.DoorD;
                break;
            }
        }
        
        for (int i = RoomHeight - 1; i >= 0; i--)   //up -> down (makes the top door)
        {
            if (Grid[centerX, i] == GridSpace.Wall)
            {
                Grid[centerX, i] = GridSpace.DoorU;
                break;
            }
        }
        
        for (int i = 0; i < RoomWidth; i++) //left -> right (makes the left door)
        {
            if (Grid[i, centerY] == GridSpace.Wall)
            {
                Grid[i, centerY] = GridSpace.DoorL;
                break;
            }
        }
        
        for (int i = RoomWidth - 1; i >= 0; i--)    //right -> left (makes the right door)
        {
            if (Grid[i, centerY] == GridSpace.Wall)
            {
                Grid[i, centerY] = GridSpace.DoorR;
                break;
            }
        }
    }

    Vector2 RandomDirection()
    {
        //pick random int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        //use that int to chose a direction
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }
    
    int NumberOfFloors()
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

}
