using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour, IListener<LevelDoorDownEvent>, IListener<LevelDoorUpEvent>, IListener<LevelDoorLeftEvent>, IListener<LevelDoorRightEvent>
{

    public enum doorDirection { left, right, up, down };
    public GameObject fillObj, rockObj,rubbleObj, wallObj, floorObj, doorRObj,doorLObj,doorUObj,doorDObj, cameraObj;

    private List<GameObject> Instantiated; // list to save all current level objects in game

    private Level[,] Levels;
    private Vector2 CurrentLevel;

    private int RoomsCleared=0;
    // Start is called before the first frame update
    void Start()
    {
        Instantiated = new List<GameObject>();
        Levels = new Level[20, 20]; // can move 10 rooms in each way
        CurrentLevel = new Vector2(10, 10); // start at the halfway point of the level grid
        CreateLevel(CurrentLevel);
        SpawnLevel();

        // bind events
        EventManager.Bind<LevelDoorUpEvent>(this);
        EventManager.Bind<LevelDoorDownEvent>(this);
        EventManager.Bind<LevelDoorLeftEvent>(this);
        EventManager.Bind<LevelDoorRightEvent>(this);

    }

    void CreateLevel(Vector2 position)
    {
        if (Random.value < 0.1f * (RoomsCleared - 5 + Math.Abs(RoomsCleared - 5))) {    //calculation for chance boss level (after 5 rooms +20% per room)
            //TODO: make boss level
        }
        else {
            Levels[(int)position.x, (int)position.y] = new Level(); // create new level
        }
    }

    void ChangeCurrentLevel(doorDirection direction)
    {
        // clear previous field
        DestroyCurrentLevel();

        // move to another position on the level grid
        switch (direction)
        {
            case doorDirection.left:
                CurrentLevel += new Vector2(-1, 0);
                break;
            case doorDirection.right:
                CurrentLevel += new Vector2(1, 0);
                break;
            case doorDirection.up:
                CurrentLevel += new Vector2(0, -1);
                break;
            case doorDirection.down:
                CurrentLevel += new Vector2(0, 1);
                break;
            default:
                CurrentLevel += new Vector2(0, -1);
                break;
        }

        // check if array is in bounds, if not return to bounds      //the 3d shape of the map is suppost to be a torroid.... 
        if(CurrentLevel.x > 19)
        {
            CurrentLevel.x = 0;//19;
        }

        if(CurrentLevel.x < 0)
        {
            CurrentLevel.x = 19;//0;
        }

        if(CurrentLevel.y < 0)
        {
            CurrentLevel.y = 19;//0;
        }

        if(CurrentLevel.y > 19)
        {
            CurrentLevel.y = 0;//19;
        }

        print(CurrentLevel);

        // create level if it does not exist
        if (Levels[(int)CurrentLevel.x, (int)CurrentLevel.y] == null)
        {
            CreateLevel(CurrentLevel);
            RoomsCleared++;
        }

        SpawnLevel();

    }
    
    bool borderOnFill(int x, int y) {       //check if tile directly borders an empty tile
        Level level = Levels[(int) CurrentLevel.x, (int) CurrentLevel.y];
        bool outp = false;
        try {
            outp = outp || level.grid[x-1, y] == Level.gridSpace.empty;
        }
        catch { return true;}
        try {
            outp = outp || level.grid[x+1,y]==Level.gridSpace.empty;
        }
        catch{ return true;}
        try {
            outp = outp || level.grid[x,y-1]==Level.gridSpace.empty;
        }
        catch{ return true;}
        try {
            outp = outp || level.grid[x,y+1]==Level.gridSpace.empty;
        }
        catch{ return true;}

        return outp;
    }


    void SpawnLevel()
    {

        Level level = Levels[(int)CurrentLevel.x, (int)CurrentLevel.y];
        for (int x = 0; x < level.roomWidth; x++){
            for (int y = 0; y < level.roomHeight; y++){
                switch(level.grid[x,y]){
                    case Level.gridSpace.empty:
                        Spawn(x,y,fillObj);
                        break;
                    case Level.gridSpace.floor:
                        Spawn(x,y,floorObj);
                        break;
                    case Level.gridSpace.wall:
                        Spawn(x,y,borderOnFill(x,y)?wallObj:rockObj.transform.GetChild (Random.Range(0,4)).gameObject);
                        break;
                    case Level.gridSpace.doorU:
                        Spawn(x,y,doorUObj);
                        break;
                    case Level.gridSpace.doorD:
                        Spawn(x,y,doorDObj);
                        break;
                    case Level.gridSpace.doorL:
                        Spawn(x,y,doorLObj);
                        break;
                    case Level.gridSpace.doorR:
                        Spawn(x,y,doorRObj);
                        break;
                    
                }
            }
        }
    }

    void DestroyCurrentLevel() // destroy a level to make space for a new one
    {
        foreach (GameObject g in Instantiated)
        {
            Destroy(g);
        }
    }

    void Spawn(float x, float y, GameObject toSpawn)
    {
        Level level = Levels[(int)CurrentLevel.x, (int)CurrentLevel.y];

        //find the position to spawn
        Vector3 spawnPos = new Vector3(x, 0, y) - new Vector3(level.offsetOfRoom.x, 0, level.offsetOfRoom.y);
        //spawn object
        Instantiated.Add(Instantiate(toSpawn, spawnPos, Quaternion.identity)); // create object and add it to the list
    }


    public void OnEvent(LevelDoorDownEvent invokedEvent)
    {
        ChangeCurrentLevel(doorDirection.down);
    }

    public void OnEvent(LevelDoorUpEvent invokedEvent)
    {
        ChangeCurrentLevel(doorDirection.up);
    }

    public void OnEvent(LevelDoorLeftEvent invokedEvent)
    {
        ChangeCurrentLevel(doorDirection.left);
    }

    public void OnEvent(LevelDoorRightEvent invokedEvent)
    {
        ChangeCurrentLevel(doorDirection.right);
    }

}
