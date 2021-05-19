using System;
using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.DoorAccess;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using Object = UnityEngine.Object;


namespace OrchestraArmy.Room
{
    
    /// <summary>
    /// The direction to which the door points.
    /// </summary>
    public enum DoorDirection
    {
        Left,
        Right,
        Up,
        Down
    };
    
    public class RoomController : IListener<EnemyDeathEvent>
    {

        /// <summary>
        /// The room for this controller.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// The enemies within the room.
        /// </summary>
        public List<Enemy> Enemies { get; set; }
            = new List<Enemy>();

        /// <summary>
        /// All objects within the room. (Rubble, floors, walls, etc.)
        /// </summary>
        public List<GameObject> Objects { get; set; }
            = new List<GameObject>();

        public RoomController()
        {
            
            // Register the events which are controlled by this class.
            EventManager.Bind<EnemyDeathEvent>(this);
        }
        
        /// <summary>
        /// Destroy the room which this controller controls.
        /// </summary>
        public void DestroyRoom()
        {
            Objects.ForEach(GameObject.Destroy);
            
            // Shouldn't be necessary, but just in case.
            Enemies.ForEach(GameObject.Destroy);
        }

        /// <summary>
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            // One enemy less
            Room.EnemySpawnData.NumberOfEnemies--;

            // If all enemies are dead
            if (Room.EnemySpawnData.NumberOfEnemies < 1)
                // Level beaten
                Room.RoomIsCleared = true;
        }

    }
}