using System;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Pickup;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Music.Controllers;
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
    
    public class Player : LivingDirectionalEntity, IListener<PlayerWeaponChangedEvent>,
        IListener<InstrumentPickupEvent>, IListener<EnemyAttackHitEvent>, IListener<PlayerAttackEvent>
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
        /// The controller for the rhythm.
        /// </summary>
        public MusicGenerator MusicGenerator;
        
        /// <summary>
        /// The controller for selecting the player's weapon(instrument).
        /// </summary>
        public IWeaponSelectionController WeaponSelectionController { get; set; }
        
        /// <summary>
        /// The controller for the player's tone.
        /// </summary>
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

        /// <summary>
        /// particles that spawn if player is damaged
        /// </summary>
        public GameObject DamageParticles;
        
        /// <summary>
        /// The controller for the entity's movement.
        /// </summary>
        public IMovementController MovementController { get; set; }
            
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
            EventManager.Bind<PlayerWeaponChangedEvent>(this);
            EventManager.Bind<InstrumentPickupEvent>(this);
            EventManager.Bind<EnemyAttackHitEvent>(this);
            EventManager.Bind<PlayerAttackEvent>(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.Unbind<PlayerWeaponChangedEvent>(this);
            EventManager.Unbind<InstrumentPickupEvent>(this);
            EventManager.Unbind<EnemyAttackHitEvent>(this);
            EventManager.Unbind<PlayerAttackEvent>(this);
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

        /// <summary>
        /// Event for when the player takes damage from enemy hit.
        /// </summary>
        public void OnEvent(EnemyAttackHitEvent invokedEvent)
        {
            // do some fancy damage calc here
            int damagePoints = 10;
            int healthAfterAttack = EntityData.Health - damagePoints;

            if (healthAfterAttack > 0)
            {
                EntityData.Health = healthAfterAttack;
                
                Vector3 particlePosition = transform.position;
                particlePosition.y = 0.5f;
                
                //spawn damage particles
                Instantiate(DamageParticles, particlePosition, Quaternion.identity);
            }
            else
            {
                // in this case, the player is dead.
                EventManager.Invoke(new PlayerDeathEvent());
                
                // reset values to max
                // Refill player health/stamina.
                EntityData.Health = EntityData.MaxHealth;
                EntityData.Stamina = 100;
            }
        }

        public void OnEvent(PlayerAttackEvent invokedEvent)
        {
            // Update stamina
            EntityData.Stamina += (int)(EntityData.MaxStamina * MusicGenerator.RhythmController.GetStaminaDamage(MusicGenerator.BPM));
            Debug.Log(EntityData.Stamina);            

            // Update health if needed
            if(EntityData.Stamina < 0)
            {
                int healthAfterAttack = EntityData.Health + EntityData.Stamina;
                EntityData.Stamina = 0;

                if (healthAfterAttack > 0)
                {
                    EntityData.Health = healthAfterAttack;
                }
                else
                {
                    // in this case, the player is dead.
                    EventManager.Invoke(new PlayerDeathEvent());
                    
                    // reset values to max
                    // Refill player health/stamina.
                    EntityData.Health = EntityData.MaxHealth;
                    EntityData.Stamina = 100;
                }
            } 
        }
    }
}