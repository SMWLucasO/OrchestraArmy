using System.Collections.Generic;
using UnityEngine;

namespace OrchestraArmy.Room.Data
{
    public class EnemySpawnData
    {

        /// <summary>
        /// The minimum distance between enemies when spawning these.
        /// </summary>
        public int DistanceBetweenEnemies { get; set; } = 10;
        
        /// <summary>
        /// All locations where an enemy can be spawned.
        /// </summary>
        public List<Vector2> Floors { get; set; }
            = new List<Vector2>();
        
        /// <summary>
        /// The number of enemies to be spawned. 
        /// </summary>
        public int NumberOfEnemies { get; set; }
        
    }
}