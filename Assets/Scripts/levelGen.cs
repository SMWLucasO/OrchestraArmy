using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class levelGen : MonoBehaviour
{
    enum gridSpace {empty, floor, wall, door};
	gridSpace[,] grid;
	int roomHeight, roomWidth;
	Vector2 roomSizeWorldUnits = new Vector2(150,150);
	float worldUnitsInOneGridCell = 1;
	struct walker{
		public Vector2 dir;
		public Vector2 pos;
	}
	List<walker> walkers;
	float chanceWalkerChangeDir = .7f, chanceWalkerSpawn = 0.2f;
	float chanceWalkerDestoy = 0.2f;
	int maxWalkers = 6;
	private float percentToFill = 0.05f; 
	public GameObject wallObj, floorObj, doorObj;
	void Start () {
		Setup();
		CreateFloors();
		CreateWalls();
		RemoveSingleWalls();
		CreateDoors();
		SpawnLevel();
	}
	void Setup(){
		//find grid size
		roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
		roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
		//create grid
		grid = new gridSpace[roomWidth,roomHeight];
		//set grid's default state
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//make every cell "empty"
				grid[x,y] = gridSpace.empty;
			}
		}
		//set first walker
		//init list
		walkers = new List<walker>();
		//create a walker 
		walker newWalker = new walker();
		newWalker.dir = RandomDirection();
		//find center of grid
		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),
										Mathf.RoundToInt(roomHeight/ 2.0f));
		newWalker.pos = spawnPos;
		//add walker to list
		walkers.Add(newWalker);
	}
	void CreateFloors(){
		int iterations = 0;//loop will not run forever
		do{
			//create floor at position of every walker
			foreach (walker myWalker in walkers){
				grid[(int)myWalker.pos.x,(int)myWalker.pos.y] = gridSpace.floor;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if its not the only one, and at a low chance
				if (Random.value < chanceWalkerDestoy && walkers.Count > 1){
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++){
				if (Random.value < chanceWalkerChangeDir){
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
			//chance: spawn new walker
			numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if # of walkers < max, and at a low chance
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers){
					//create a walker 
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
			//move walkers
			for (int i = 0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;				
			}
			//avoid boarder of grid
			for (int i =0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				//clamp x,y to leave a 1 space boarder: leave room for walls
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth-2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight-2);
				walkers[i] = thisWalker;
			}
			//check to exit loop
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill){
				break;
			}
			iterations++;
		}while(iterations < 100000);
	}
	void CreateWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a floor, check the spaces around it
				if (grid[x,y] == gridSpace.floor){
					//if any surrounding spaces are empty, place a wall
					if (grid[x,y+1] == gridSpace.empty){
						grid[x,y+1] = gridSpace.wall;
					}
					if (grid[x,y-1] == gridSpace.empty){
						grid[x,y-1] = gridSpace.wall;
					}
					if (grid[x+1,y] == gridSpace.empty){
						grid[x+1,y] = gridSpace.wall;
					}
					if (grid[x-1,y] == gridSpace.empty){
						grid[x-1,y] = gridSpace.wall;
					}
				}
			}
		}
	}
	void RemoveSingleWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a wall, check the spaces around it
				if (grid[x,y] == gridSpace.wall){
					//assume all space around wall are floors
					bool allFloors = true;
					//check each side to see if they are all floors
					for (int checkX = -1; checkX <= 1 ; checkX++){
						for (int checkY = -1; checkY <= 1; checkY++){
							if (x + checkX < 0 || x + checkX > roomWidth - 1 || 
								y + checkY < 0 || y + checkY > roomHeight - 1){
								//skip checks that are out of range
								continue;
							}
							if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0)){
								//skip corners and center
								continue;
							}
							if (grid[x + checkX,y+checkY] != gridSpace.floor){
								allFloors = false;
							}
						}
					}
					if (allFloors){
						grid[x,y] = gridSpace.floor;
					}
				}
			}
		}
	}
	
	/*
	void CreateDoors(){
        //empty list of walkers
        walkers = new List<walker>();

        //create walker at top middle.
        walker topWalker = new walker();
        walker leftWalker = new walker();
        walker bottomWalker = new walker();
        walker rightWalker = new walker();

        topWalker.dir = Vector2.up;
        leftWalker.dir = Vector2.left;
        bottomWalker.dir = Vector2.down;
        rightWalker.dir = Vector2.right;

        print(1);

        topWalker.pos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),
										0);
        leftWalker.pos = new Vector2(0,
										Mathf.RoundToInt(roomHeight/ 2.0f));
        bottomWalker.pos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),
										roomHeight-1);
        rightWalker.pos = new Vector2(roomWidth-1,
										Mathf.RoundToInt(roomHeight/ 2.0f));

        print(2);

        walkers.Add(topWalker);
        walkers.Add(leftWalker);
        walkers.Add(bottomWalker);
        walkers.Add(rightWalker);


		bool goOn = true;
        print(grid[15,0]);
        do{
	        print((int)topWalker.pos.x+", "+(int)topWalker.pos.y);
	        print(grid[(int)topWalker.pos.x,(int)topWalker.pos.y]);
            if(grid[(int)topWalker.pos.x,(int)topWalker.pos.y] == gridSpace.wall 
				&& grid[(int)topWalker.pos.x,(int)topWalker.pos.y+1] == gridSpace.floor){
					grid[(int)topWalker.pos.x,(int)topWalker.pos.y] = gridSpace.door;
					goOn = false;
			}
			topWalker.pos += topWalker.dir;
		}while(goOn);
        print(3);

		goOn = true;
        do{
			if(grid[(int)leftWalker.pos.x,(int)leftWalker.pos.y] == gridSpace.wall 
				&& grid[(int)leftWalker.pos.x-1,(int)leftWalker.pos.y] == gridSpace.floor){
					grid[(int)leftWalker.pos.x,(int)leftWalker.pos.y] = gridSpace.door;
					goOn = false;
			leftWalker.pos += leftWalker.dir;
			}
		}while(goOn);
        print(4);

		goOn = true;
        do{
			if(grid[(int)bottomWalker.pos.x,(int)bottomWalker.pos.y] == gridSpace.wall 
				&& grid[(int)bottomWalker.pos.x,(int)bottomWalker.pos.y-1] == gridSpace.floor){
					grid[(int)bottomWalker.pos.x,(int)bottomWalker.pos.y] = gridSpace.door;
					goOn = false;
			bottomWalker.pos += bottomWalker.dir;
			}
		}while(goOn);

		goOn = true;
		do{
			if(grid[(int)rightWalker.pos.x,(int)rightWalker.pos.y] == gridSpace.wall 
				&& grid[(int)rightWalker.pos.x+1,(int)rightWalker.pos.y] == gridSpace.floor){
					grid[(int)rightWalker.pos.x,(int)rightWalker.pos.y] = gridSpace.door;
					goOn = false;
			rightWalker.pos += rightWalker.dir;
			}
		}while(goOn);

    }
	
	*/

	void CreateDoors()
	{
		int minx=roomWidth, miny=roomHeight, maxx=0, maxy=0;
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if (grid[x,y] != gridSpace.empty)
				{
					minx = Math.Min(minx, x);
					maxx = Math.Max(maxx, x);
					miny = Math.Min(miny, y);
					maxy = Math.Max(maxy, y);
				}
			}
		}

		int centerX = (int)(minx+maxx)/2, centerY=(int)(miny+maxy)/2;
		
		for (int i = 0; i < roomHeight; i++)	//down -> up
		{
			if (grid[centerX,i] == gridSpace.wall/* &&
			    grid[Mathf.RoundToInt(roomWidth/ 2.0f),i+1] == gridSpace.floor*/)
			{
				grid[centerX, i] = gridSpace.door;
				print(1);
				break;
			}
		}
		for (int i = roomHeight-1; i >= 0; i--)	//up -> down
		{
			if (grid[centerX,i] == gridSpace.wall/* &&
			    grid[Mathf.RoundToInt(roomWidth/ 2.0f),i-1] == gridSpace.floor*/)
			{
				grid[centerX, i] = gridSpace.door;
				print(2);
				break;
			}
		}
		for (int i = 0; i < roomWidth; i++)	//left -> right
		{
			if (grid[i,centerY] == gridSpace.wall/* &&
			    grid[i+1,Mathf.RoundToInt(roomHeight/ 2.0f)] == gridSpace.floor*/)
			{
				grid[i,centerY] = gridSpace.door;
				print(3);
				break;
			}
		}
		for (int i = roomWidth-1; i >= 0; i--)	//right -> left
		{
			if (grid[i,centerY] == gridSpace.wall/* &&
			    grid[i-1,Mathf.RoundToInt(roomHeight/ 2.0f)] == gridSpace.floor*/)
			{
				grid[i,centerY] = gridSpace.door;
				print(4);
				break;
			}
		}
	}

	void SpawnLevel(){
		for (int x = 0; x < roomWidth; x++){
			for (int y = 0; y < roomHeight; y++){
				switch(grid[x,y]){
					case gridSpace.empty:
						break;
					case gridSpace.floor:
						Spawn(x,y,floorObj);
						break;
					case gridSpace.wall:
						Spawn(x,y,wallObj);
						break;
					case gridSpace.door:
						Spawn(x,y,doorObj);
						break;
                    
				}
			}
		}
	}
	Vector2 RandomDirection(){
		//pick random int between 0 and 3
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
		//use that int to chose a direction
		switch (choice){
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
	int NumberOfFloors(){
		int count = 0;
		foreach (gridSpace space in grid){
			if (space == gridSpace.floor){
				count++;
			}
		}
		return count;
	}
	void Spawn(float x, float y, GameObject toSpawn){
		//find the position to spawn
		Vector3 offset = new Vector3(roomSizeWorldUnits.x / 2.0f,0.0f,roomSizeWorldUnits.y / 2.0f);
		Vector3 spawnPos = new Vector3(x,0.0f,y) * worldUnitsInOneGridCell - offset;
		//spawn object
		Instantiate(toSpawn, spawnPos, Quaternion.identity);
	}
}
