using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Player.Controllers;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player
{
    public class Player : LivingDirectionalEntity
    {
        /// <summary>
        /// The controller for the player's camera.
        /// </summary>
        public ICameraController CameraController { get; set; }

        void Start()
        {
            InitializeSprites();
            DirectionController = new PlayerDirectionController()
            {
                Entity = this
            };
            
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

        private void Update()
        {
            base.Update();
            DirectionController.HandleDirection();
            SpriteManager.UpdateSprite();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            CameraController?.HandleCameraMovement();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

    }
}