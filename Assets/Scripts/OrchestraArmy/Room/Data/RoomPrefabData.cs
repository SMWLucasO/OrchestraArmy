using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrchestraArmy.Room.Data
{
    [Serializable]
    public class RoomPrefabData
    {
        
        /// <summary>
        /// Objects for the room surroundings.
        /// </summary>
        public GameObject FillObj,
            RockObj,
            RubbleObj,
            WallObj,
            FloorObj,
            DoorRightObj,
            DoorLeftObj,
            DoorUpObj,
            DoorNextLevelObj,
            DoorDownObj;
        
        
        /// <summary>
        /// The prefabs for the enemies within the room.
        /// </summary>
        public List<GameObject> Enemies
            = new List<GameObject>();

        /// <summary>
        /// The prefabs for bosses within the room.
        /// The spawn order goes from 0...n
        /// </summary>
        public List<GameObject> Bosses 
            = new List<GameObject>();

        /// <summary>
        /// The prefabs for boss drops within the room.
        /// The drop order goes from 0...n
        /// </summary>
        public List<GameObject> InstrumentDrops =
            new List<GameObject>();

    }
}