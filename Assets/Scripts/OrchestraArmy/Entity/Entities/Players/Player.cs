using System;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Pickup;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Room;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players
{
    [Serializable]
    public struct InstrumentSpriteEntry
    {
        public SpriteEntry[] SpriteEntries;
        public WeaponType Instrument;
    }
    
    public class Player : LivingDirectionalEntity, IListener<PlayerDamageEvent>, IListener<PlayerWeaponChangedEvent>,
        IListener<InstrumentPickupEvent>, IListener<PlayerDeathEvent>
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
        public IToneController ToneController { get; set; }

        /// <summary>
        /// Sprites based on instruments, current instrument's SpriteEntry is written to Sprites
        /// </summary>
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
            ToneController.HandleTone();
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
            
            this.ToneController = new PlayerToneController();

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
            EventManager.Bind<InstrumentPickupEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.Unbind<PlayerDamageEvent>(this);
            EventManager.Unbind<PlayerWeaponChangedEvent>(this);
            EventManager.Unbind<InstrumentPickupEvent>(this);
            EventManager.Unbind<PlayerDeathEvent>(this);
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
                EventManager.Invoke(new PlayerDeathEvent());
                
                //reset values to max
                EntityData.Health = 100;
            }
        }

        public void OnEvent(InstrumentPickupEvent invokedEvent)
        {
            // add instrument to weapons
            WeaponWheel.Unlock(invokedEvent.InstrumentPickedUp);

            // We collected an instrument, add one.
            RoomManager.Instance.CollectedInstrumentCount += 1;
            
            // spawn portal at center of room.
            Room.Room currentRoom = RoomManager.Instance.CurrentRoom;
            float roomMidX = currentRoom.RoomWidth / 2;
            float roomMidY = currentRoom.RoomHeight / 2;
            
            currentRoom.RoomController.Objects.Add(
                GameObject.Instantiate(
                    RoomManager.Instance.RoomPrefabData.DoorNextLevelObj, 
                    new Vector3(roomMidX,0, roomMidY) - new Vector3(currentRoom.OffsetOfRoom.x, 0, currentRoom.OffsetOfRoom.y),
                    Quaternion.identity
                )
            );
        }
        
        public void OnEvent(PlayerWeaponChangedEvent invokedEvent)
        {
            Sprites = InstrumentSprites.First(s => s.Instrument == invokedEvent.NewlySelectedWeapon).SpriteEntries;
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
        {            
            // Refill player health/stamina.
            EntityData.Health = 100;
            EntityData.Stamina = 100;
        }
    }
}