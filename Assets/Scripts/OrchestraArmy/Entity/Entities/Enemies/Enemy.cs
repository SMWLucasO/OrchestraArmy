using System;
using OrchestraArmy.Entity.Entities.Behaviour;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Enemies.Bosses;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Event.Events.Room;
using OrchestraArmy.Room;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using OrchestraArmy.Entity.Entities.Enemies.Controllers;
using OrchestraArmy.SaveData;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public abstract class Enemy : LivingDirectionalEntity, IListener<PlayerAttackHitEvent>, IListener<PlayerDeathEvent>, IListener<PlayerWeaponChangedEvent>
    {
        /// <summary>
        /// used for dynamic difficulty
        /// </summary>
        private int _difficulty = 0;

        public BehaviourStateMachine Behaviour { get; set; }

        /// <summary>
        /// The type of instrument which the enemy can be damaged with.
        /// </summary>
        public abstract WeaponType HittableBy { get; set; }
        
        /// <summary>
        /// The weapon of the enemy.
        /// </summary>
        public abstract WeaponType WeaponType { get; set; }
        
        /// <summary>
        /// The NavMeshAgent for the enemy.
        /// </summary>
        public NavMeshAgent NavMeshAgent { get; set; }
        
        /// <summary>
        /// The location where this player started.
        /// </summary>
        public Vector3 SpawnLocation { get; set; }

        /// <summary>
        /// The sprite renderer of the enemy.
        /// </summary>
        private SpriteRenderer _spriteRenderer;
        
        /// <summary>
        /// particles that spawn if enemy is damaged
        /// </summary>
        public GameObject DamageParticles;
        
        protected override void Update()
        {
            DirectionController.HandleDirection();
            SpriteManager.UpdateSprite();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            InitializeSprites(0.2f);

            DirectionController = new EnemyDirectionController()
            {
                Entity = this
            };

            Behaviour = new BehaviourStateMachine()
            {
                CurrentState = new WanderBehaviour()
            };

            SpawnLocation = transform.position;
            
            // set initial state data.
            Behaviour.CurrentState.StateData = new StateData()
            {
                Player = GameObject.FindWithTag("Player").GetComponent<Player>(),
                Enemy = this,
                ProjectileType = typeof(EnemyNote),
                AttackController = new EnemyAttackController()
            };

            NavMeshAgent = this.GetComponent<NavMeshAgent>();

            _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            /*
            _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            Material material = _meshRenderer.material;
            */

            ApplyVisibilityChangesForWeapon(
                    GameObject.FindWithTag("Player").GetComponent<Player>().WeaponWheel
                    .CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                );

            // Register enemy events.
            //EventManager.Bind<EnemyDeathEvent>(this);
            EventManager.Bind<PlayerAttackHitEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
            EventManager.Bind<PlayerWeaponChangedEvent>(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Behaviour.Update();
        }

        /// <summary>
        /// For when an enemy dies.
        /// </summary>
        /// <param name="enemy"></param>
        public virtual void EnemyDeath(Enemy enemy)
        {
            if (enemy.GetInstanceID() != GetInstanceID()) return;

            Room.Room currentRoom = RoomManager.Instance.CurrentRoom;
            currentRoom.EnemySpawnData.NumberOfEnemies -= 1;

            if (currentRoom.EnemySpawnData.NumberOfEnemies < 1 && !currentRoom.RoomIsCleared)
            {
                currentRoom.RoomIsCleared = true;
                EventManager.Invoke(new RoomClearedOfEnemiesEvent());
            }

            Destroy(gameObject);
        }

        public void OnEvent(PlayerAttackHitEvent invokedEvent)
        {
            if (gameObject.GetInstanceID() != invokedEvent.TargetId)
                return;

            // Enemy can only be hit by specific instrument.
            if (invokedEvent.Weapon.WeaponType != HittableBy)
                return;
            
            EntityData.Health -= invokedEvent.Weapon.GetTotalDamage();

            if (EntityData.Health <= 0)
            {
                EnemyDeath(this);
            }
            
            //update healthbar
            transform.Find("Canvas/BackgroundBar/FilledPart").GetComponent<Image>().fillAmount =
                EntityData.Health / (float)EntityData.MaxHealth;

            Vector3 particlePosition = transform.position;
            particlePosition.y = 0.5f;
            
            //spawn damage particles
            Instantiate(DamageParticles, particlePosition, Quaternion.identity);
        }

        public void OnDestroy()
        {
            EventManager.Unbind<PlayerAttackHitEvent>(this);
            EventManager.Unbind<PlayerDeathEvent>(this);
            EventManager.Unbind<PlayerWeaponChangedEvent>(this);
            
            Behaviour.ClearState();
        }

        public void OnEvent(PlayerDeathEvent invokedEvent) => Behaviour.ClearState();
        
        public void OnEvent(PlayerWeaponChangedEvent invokedEvent)
            => ApplyVisibilityChangesForWeapon(invokedEvent.NewlySelectedWeapon);

        protected void ApplyVisibilityChangesForWeapon(WeaponType selectedWeapon)
        {
            if (this is Boss) return;
            
            Color color = _spriteRenderer.color;
            
            // set the transparency 
            if (selectedWeapon == HittableBy)
            {
                color.a = 1f;
                transform.Find("Canvas").GetComponent<Canvas>().enabled = true;
            }
            else
            {
                color.a = 0.3f;
                transform.Find("Canvas").GetComponent<Canvas>().enabled = false;
            }

            _spriteRenderer.color = color;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetVariableHp()
        {
            _difficulty = DataSaver.LoadData<SettingsData>("settingsData").Difficulty;
            EntityData.MaxHealth += (int)(((LevelManager.Instance.Level-1)*0.5f) * 10*(_difficulty+1));
            EntityData.Health = EntityData.MaxHealth;
            Debug.Log((int)(((LevelManager.Instance.Level-1)*0.5f) * 10*(_difficulty+1)));
            Debug.Log(EntityData.MaxHealth);
        }

        protected override void Awake()
        {
            base.Awake();
            SetVariableHp();
        }
    }
}