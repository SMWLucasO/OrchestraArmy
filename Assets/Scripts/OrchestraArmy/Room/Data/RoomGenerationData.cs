using System.Collections.Generic;
using UnityEngine;

namespace OrchestraArmy.Room.Data
{

    public struct Walker
    {
        /// <summary>
        /// The direction which the walker is going.
        /// </summary>
        public Vector2 Direction;
        
        /// <summary>
        /// The current position of the walker.
        /// </summary>
        public Vector2 Position;
    }
    
    public class RoomGenerationData
    {

        /// <summary>
        /// The walkers currently alive generating the room.
        /// </summary>
        public List<Walker> Walkers { get; set; }
            = new List<Walker>();

        /// <summary>
        /// The chance that a walker will change direction.
        /// </summary>
        public float WalkerDirectionChangeChance { get; set; } = 0.7f;

        /// <summary>
        /// The chance that a new walker will spawn.
        /// </summary>
        public float WalkerSpawnChance { get; set; } = 0.2f;

        /// <summary>
        /// The chance that the walker will be destructed.
        /// </summary>
        public float WalkerDestructionChance { get; set; } = 0.2f;

        /// <summary>
        /// The amount of walkers that are allowed to exist at one point in time.
        /// </summary>
        public int MaxAliveWalkers { get; set; } = 6;

        /// <summary>
        /// The width, height and depth of a grid cell in Unity3D's unit system.
        /// </summary>
        public float WorldUnitsInOneGridCell { get; set; } = 1;

        /// <summary>
        /// The percentage of the map that should be filled with content.
        /// </summary>
        public float PercentToFill { get; set; } = 0.05f;


    }
}