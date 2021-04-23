using OrchestraArmy.Entity.Entities.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Controllers
{
    public interface ICameraController
    {
        /// <summary>
        /// The player to control the camera for.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The camera to be controlled.
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Handle the player's camera's movement.
        /// </summary>
        public void HandleCameraMovement();

    }
}