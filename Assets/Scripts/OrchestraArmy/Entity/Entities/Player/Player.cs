using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Player.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player
{
    public class Player : LivingDirectionalEntity
    {
        /// <summary>
        /// The controller for the player's camera.
        /// </summary>
        public ICameraController CameraController { get; set; }
        
        public Player()
        {
            this.MovementController = new PlayerMovementController()
            {
                Entity = this
            };

            // The main camera is the camera which the player uses.
            this.CameraController = new PlayerCameraController()
            {
                Player = this
            };
        }

        protected override void Update()
        {
            base.Update();
            CameraController?.HandleCameraMovement();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.CameraController.Camera = Camera.main;
        }
        
        
    }
}