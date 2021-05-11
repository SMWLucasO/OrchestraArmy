using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players
{
    public class Player : LivingDirectionalEntity, IListener<PlayerDamageEvent>
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
        
        /// <summary>
        /// The player's weapon selection wheel.
        /// </summary>
        public WeaponWheel WeaponWheel { get; set; }
        
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

            // Get the weapon wheel for the player.
            WeaponWheel = GameObject.FindWithTag("UI:WeaponWheel").GetComponent<WeaponWheel>();
            
            // register player events.
            EventManager.Bind<PlayerDamageEvent>(this);
        }

        /// <summary>
        /// Event for when the player takes damage.
        /// </summary>
        /// <param name="playerDamageEvent"></param>
        public void OnEvent(PlayerDamageEvent playerDamageEvent)
        {
            int healthAfterAttack = EntityData.Health - playerDamageEvent.HealthLost;

            if (healthAfterAttack > 0)
                EntityData.Health = healthAfterAttack;
            else
            {
                // in this case, the player is dead.
                EntityData.Health = 0;
                EventManager.Invoke(new PlayerDeathEvent());
            }
        }
    }
}