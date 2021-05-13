using System;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players
{
    [Serializable]
    public struct InstrumentSpriteEntry
    {
        public SpriteEntry[] SpriteEntries;
        public WeaponType Instrument;
    }
    
    public class Player : LivingDirectionalEntity, IListener<PlayerDamageEvent>, IListener<PlayerWeaponChangedEvent>
    {
        /// <summary>
        /// The controller for the player's camera.
        /// </summary>
        public ICameraController CameraController { get; set; }

        /// <summary>
        /// The controller for the player's attacking.
        /// </summary>
        public IAttackController AttackController { get; set; }
        
        /// <summary>
        /// The controller for selecting the player's weapon(instrument).
        /// </summary>
        public IWeaponSelectionController WeaponSelectionController { get; set; }

        [field: SerializeField]
        public InstrumentSpriteEntry[] InstrumentSprites { get; set; }
        
        /// <summary>
        /// The player's weapon selection wheel.
        /// </summary>
        public WeaponWheel WeaponWheel { get; set; }
        
        protected override void Update()
        {
            base.Update();
            DirectionController.HandleDirection();
            WeaponSelectionController.HandleWeaponSelection();
            SpriteManager.UpdateSprite();
            AttackController.HandleAttack();
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

            this.WeaponSelectionController = new PlayerWeaponSelectionController()
            {
                Player = this
            };

            this.AttackController = new PlayerAttackController()
            {
                Player = this
            };

            // Get the weapon wheel for the player.
            WeaponWheel = GameObject.FindWithTag("UI:WeaponWheel").GetComponent<WeaponWheel>();
            Sprites = InstrumentSprites.First(s => s.Instrument == WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType).SpriteEntries;
            
            // register player events.
            EventManager.Bind<PlayerDamageEvent>(this);
            EventManager.Bind<PlayerWeaponChangedEvent>(this);
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

        public void OnEvent(PlayerWeaponChangedEvent invokedEvent)
        {
            Sprites = InstrumentSprites.First(s => s.Instrument == invokedEvent.NewlySelectedWeapon).SpriteEntries;
        }
    }
}