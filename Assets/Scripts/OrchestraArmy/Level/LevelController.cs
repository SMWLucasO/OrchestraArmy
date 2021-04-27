using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;

public class LevelController : MonoBehaviour, IListener<LevelDoorDownEvent>, IListener<LevelDoorUpEvent>, IListener<LevelDoorLeftEvent>, IListener<LevelDoorRightEvent>
{

    public enum doorDirection { left, right, up, down };
    public GameObject wallObj, floorObj, doorRObj, doorLObj, doorUObj, doorDObj, cameraObj;

    private List<GameObject> Instantiated; // list to save all current level objects in game

    private Level[,] Levels;
    private Vector2 CurrentLevel;
    // Start is called before the first frame update
    void Start()
    {
        Setup();

    }

    void Setup()
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
        //TODO: determine if next room is boss level

        Levels[(int)position.x, (int)position.y] = new Level(); // create new level

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

        // create level if it does not exist
        if (Levels[(int)CurrentLevel.x, (int)CurrentLevel.y] == null)
        {
            CreateLevel(CurrentLevel);
        }

        SpawnLevel();

    }

    void SpawnLevel()
    {

        Level level = Levels[(int)CurrentLevel.x, (int)CurrentLevel.y];
        for (int x = 0; x < level.roomWidth; x++)
        {
            for (int y = 0; y < level.roomHeight; y++)
            {
                switch (level.grid[x, y])
                {
                    case Level.gridSpace.empty:
                        break;
                    case Level.gridSpace.floor:
                        Spawn(x, y, floorObj);
                        break;
                    case Level.gridSpace.wall:
                        Spawn(x, y, wallObj);
                        break;
                    case Level.gridSpace.doorU:
                        Spawn(x, y, doorUObj);
                        break;
                    case Level.gridSpace.doorD:
                        Spawn(x, y, doorDObj);
                        break;
                    case Level.gridSpace.doorL:
                        Spawn(x, y, doorLObj);
                        break;
                    case Level.gridSpace.doorR:
                        Spawn(x, y, doorRObj);
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
