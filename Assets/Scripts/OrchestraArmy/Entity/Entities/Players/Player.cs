using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.Controllers;

namespace OrchestraArmy.Entity.Entities.Players
{
    public class Player : LivingDirectionalEntity
    {
        /// <summary>
        /// The controller for the player's camera.
        /// </summary>
        public ICameraController CameraController { get; set; }

        /// <summary>
        /// The controller for the player's movement.
        /// </summary>
        public IMovementController MovementController { get; set; }

        /// <summary>
        /// The controller for the player's attacking.
        /// </summary>
        public IAttackController AttackController { get; set; }
        
        
        protected override void Update()
        {
            base.Update();
            DirectionController.HandleDirection();
            SpriteManager.UpdateSprite();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            MovementController?.HandleMovement();
        }
        
        protected override void LateUpdate()
        {
            base.LateUpdate();
            CameraController?.HandleCameraMovement();
        }

        protected override void OnEnable()
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
    }
}