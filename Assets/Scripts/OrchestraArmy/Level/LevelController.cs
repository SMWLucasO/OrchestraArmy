using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour, IListener<LevelDoorDownEvent>, IListener<LevelDoorUpEvent>, IListener<LevelDoorLeftEvent>, IListener<LevelDoorRightEvent>
{

    private enum DoorDirection { Left, Right, Up, Down };
    public GameObject fillObj, rockObj,rubbleObj, wallObj, floorObj, doorRObj,doorLObj,doorUObj,doorDObj;

    private List<GameObject> _instantiated; // list to save all current level objects in game

    private Level[,] _levels;
    private Vector2 _currentLevel;

    private int _roomsCleared=0;
    // Start is called before the first frame update
    void Start()
    {
        _instantiated = new List<GameObject>();
        _levels = new Level[20, 20]; // can move 10 rooms in each way
        _currentLevel = new Vector2(10, 10); // start at the halfway point of the level grid
        CreateLevel(_currentLevel);  //make a grid
        SpawnLevel();

        // bind events
        EventManager.Bind<LevelDoorUpEvent>(this);
        EventManager.Bind<LevelDoorDownEvent>(this);
        EventManager.Bind<LevelDoorLeftEvent>(this);
        EventManager.Bind<LevelDoorRightEvent>(this);   // door actions (go though door)

    }

    void CreateLevel(Vector2 position)
    {
        if (Random.value < 0.1f * (_roomsCleared - 5 + Math.Abs(_roomsCleared - 5))) {    //calculation for chance boss level (after 5 rooms +20% per room)
            print("boss room");
            _levels[(int)position.x, (int)position.y] = new Level(true); // create boss level
        }
        else {
            _levels[(int)position.x, (int)position.y] = new Level(); // create new level
        }
    }

    void ChangeCurrentLevel(DoorDirection direction)
    {
        // clear previous field
        DestroyCurrentLevel();

        // move to another position on the level grid
        switch (direction)
        {
            case DoorDirection.Left:
                _currentLevel += new Vector2(-1, 0);
                break;
            case DoorDirection.Right:
                _currentLevel += new Vector2(1, 0);
                break;
            case DoorDirection.Up:
                _currentLevel += new Vector2(0, -1);
                break;
            case DoorDirection.Down:
                _currentLevel += new Vector2(0, 1);
                break;
            default:
                _currentLevel += new Vector2(0, -1);
                break;
        }

        // check if array is in bounds, if not return to bounds      //the 3d shape of the map is suppost to be a torroid
        if(_currentLevel.x > 19)
        {
            _currentLevel.x = 0;//19;
        }

        if(_currentLevel.x < 0)
        {
            _currentLevel.x = 19;//0;
        }

        if(_currentLevel.y < 0)
        {
            _currentLevel.y = 19;//0;
        }

        if(_currentLevel.y > 19)
        {
            _currentLevel.y = 0;//19;
        }

        print(_currentLevel);

        // create level if it does not exist
        if (_levels[(int)_currentLevel.x, (int)_currentLevel.y] == null)
        {
            CreateLevel(_currentLevel);
            _roomsCleared++;
        }

        SpawnLevel();
        

    }
    
    bool borderOnFill(int x, int y) {       //check if tile directly borders an empty tile
        Level level = _levels[(int) _currentLevel.x, (int) _currentLevel.y];
        bool outp = false;
        try {
            outp = outp || level.Grid[x-1, y] == Level.GridSpace.Empty;
        }
        catch { return true;}
        try {
            outp = outp || level.Grid[x+1,y]==Level.GridSpace.Empty;
        }
        catch{ return true;}
        try {
            outp = outp || level.Grid[x,y-1]==Level.GridSpace.Empty;
        }
        catch{ return true;}
        try {
            outp = outp || level.Grid[x,y+1]==Level.GridSpace.Empty;
        }
        catch{ return true;}

        return outp;
    }


    void SpawnLevel()
    {
        Level level = _levels[(int)_currentLevel.x, (int)_currentLevel.y];
        for (int x = 0; x < level.RoomWidth; x++){
            for (int y = 0; y < level.RoomHeight; y++){
                switch(level.Grid[x,y]){
                    case Level.GridSpace.Empty:
                        Spawn(x,y,fillObj);
                        break;
                    case Level.GridSpace.Floor:
                        Spawn(x,y,floorObj);
                        if (Random.value<0.1f) {
                            Spawn(x,y,rubbleObj.transform.GetChild(Random.Range(0,3)).gameObject);
                        }
                        break;
                    case Level.GridSpace.Wall:
                        Spawn(x,y,borderOnFill(x,y)?wallObj:rockObj.transform.GetChild (Random.Range(0,4)).gameObject);
                        break;
                    case Level.GridSpace.DoorU:
                        Spawn(x,y,doorUObj);
                        break;
                    case Level.GridSpace.DoorD:
                        Spawn(x,y,doorDObj);
                        break;
                    case Level.GridSpace.DoorL:
                        Spawn(x,y,doorLObj);
                        break;
                    case Level.GridSpace.DoorR:
                        Spawn(x,y,doorRObj);
                        break;
                    
                }
            }
        }
    }

    void DestroyCurrentLevel() // destroy a level to make space for a new one
    {
        foreach (GameObject g in _instantiated)
        {
            Destroy(g);
        }
    }

    void Spawn(float x, float y, GameObject toSpawn)
    {
        Level level = _levels[(int)_currentLevel.x, (int)_currentLevel.y];

        //find the position to spawn
        Vector3 spawnPos = new Vector3(x, 0, y) - new Vector3(level.OffsetOfRoom.x, 0, level.OffsetOfRoom.y);
        //spawn object
        _instantiated.Add(Instantiate(toSpawn, spawnPos, Quaternion.identity)); // create object and add it to the list
    }


    public void OnEvent(LevelDoorDownEvent invokedEvent)
    {
        ChangeCurrentLevel(DoorDirection.Down);
    }

    public void OnEvent(LevelDoorUpEvent invokedEvent)
    {
        ChangeCurrentLevel(DoorDirection.Up);
    }

    public void OnEvent(LevelDoorLeftEvent invokedEvent)
    {
        ChangeCurrentLevel(DoorDirection.Left);
    }

    public void OnEvent(LevelDoorRightEvent invokedEvent)
    {
        ChangeCurrentLevel(DoorDirection.Right);
    }


    /// DIT IS VOOR TESTEN EN MOET VERWIJDERD WORDEN
    private void Update()
    {
        if (Input.GetKey("i")) {
            ChangeCurrentLevel(DoorDirection.Up);
        }else if (Input.GetKey("k")) {
            ChangeCurrentLevel(DoorDirection.Down);
        }else if (Input.GetKey("j")) {
            ChangeCurrentLevel(DoorDirection.Left);
        }else if (Input.GetKey("l")) {
            ChangeCurrentLevel(DoorDirection.Right);
        }else if (Input.GetKey("b")) {
            _roomsCleared = 10;
        }
    }
}
