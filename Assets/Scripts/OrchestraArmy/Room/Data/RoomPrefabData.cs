using System;
using UnityEngine;

namespace OrchestraArmy.Room.Data
{
    [Serializable]
    public class RoomPrefabData
    {
        
        /// <summary>
        /// Objects for the room surroundings.
        /// </summary>
        [field: SerializeField]
        public GameObject FillObj,
            RockObj,
            RubbleObj,
            WallObj,
            FloorObj,
            DoorRightObj,
            DoorLeftObj,
            DoorUpObj,
            DoorDownObj;
        
    }
}