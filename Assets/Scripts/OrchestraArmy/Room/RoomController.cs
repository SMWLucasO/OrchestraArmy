using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    [Serializable]
    public abstract class RoomController
    {

        /// <summary>
        /// The room for this controller.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// All objects within the room. (Rubble, floors, walls, etc.)
        /// </summary>
        public List<GameObject> Objects { get; set; }
            = new List<GameObject>();
        
        /// <summary>
        /// Destroy the room which this controller controls.
        /// </summary>
        public void DestroyRoom() 
            => Objects.ForEach(GameObject.Destroy);

        /// <summary>
        /// Unregister any events within the controller.
        /// </summary>
        public virtual void UnregisterEvents() {}

        /// <summary>
        /// Register any events within the controller.
        /// </summary>
        public virtual void RegisterEvents() {}

    }
}