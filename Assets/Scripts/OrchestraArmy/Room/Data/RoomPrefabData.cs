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
            DoorDownObj;
        
        
        /// <summary>
        /// The prefabs for the enemies within the room.
        /// </summary>
        public List<GameObject> Enemies
            = new List<GameObject>();
        
    }
}