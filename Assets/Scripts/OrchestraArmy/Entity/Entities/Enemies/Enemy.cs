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

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public abstract class Enemy : LivingDirectionalEntity, IListener<EnemyDeathEvent>, IListener<PlayerAttackHitEvent>, IListener<PlayerDeathEvent>, IListener<PlayerWeaponChangedEvent>
    {
        
        public BehaviourStateMachine Behaviour { get; set; }

        public float LastCollisionTime { get; set; }
        
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
        /// The mesh renderer of the enemy.
        /// </summary>
        private MeshRenderer _meshRenderer;

        protected override void OnEnable()
        {
            base.OnEnable();

            LastCollisionTime = Time.time;

            Behaviour = new BehaviourStateMachine()
            {
                CurrentState = new WanderBehaviour()
            };

            SpawnLocation = transform.position;
            
            // set initial state data.
            Behaviour.CurrentState.StateData = new StateData()
            {
                Player = GameObject.FindWithTag("Player").GetComponent<Player>(),
                Enemy = this
            };

            NavMeshAgent = this.GetComponent<NavMeshAgent>();

            _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            Material material = _meshRenderer.material;

            ApplyVisibilityChangesForWeapon(
                    GameObject.FindWithTag("Player").GetComponent<Player>().WeaponWheel
                    .CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                );
            
            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            
            // Register enemy events.
            EventManager.Bind<EnemyDeathEvent>(this);
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
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public virtual void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            if (enemyDeathEvent.KilledEnemy.GetInstanceID() != GetInstanceID()) return;

            Room.Room currentRoom = RoomManager.Instance.CurrentRoom;
            currentRoom.EnemySpawnData.NumberOfEnemies -= 1;

            if (currentRoom.EnemySpawnData.NumberOfEnemies < 1 && !currentRoom.RoomIsCleared)
            {
                currentRoom.RoomIsCleared = true;
                EventManager.Invoke(new RoomClearedOfEnemiesEvent());
            }

            Behaviour.ClearState();
            
            Destroy(gameObject);
        }

        /// <summary>
        /// Temporary player attacking event.
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.TryGetComponent<Player>(out Player player))
                return;

            if (!((Time.time - LastCollisionTime) > 1))
                return;

            LastCollisionTime = Time.time;
            EventManager.Invoke(new PlayerDamageEvent() { HealthLost = 10 });
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
                EventManager.Invoke(new EnemyDeathEvent() { KilledEnemy = this });
            }
        }

        public void OnDestroy()
        {
            EventManager.Unbind<EnemyDeathEvent>(this);
            EventManager.Unbind<PlayerAttackHitEvent>(this);
            EventManager.Unbind<PlayerDeathEvent>(this);
            EventManager.Unbind<PlayerWeaponChangedEvent>(this);
            
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            Behaviour.ClearState();
        }
        
        public void OnEvent(PlayerWeaponChangedEvent invokedEvent)
            => ApplyVisibilityChangesForWeapon(invokedEvent.NewlySelectedWeapon);

        private void ApplyVisibilityChangesForWeapon(WeaponType selectedWeapon)
        {
            if (this is Boss) return;
            
            var material = _meshRenderer.material;
            Color color = material.color;
            
            // set the transparency 
            color.a = selectedWeapon == HittableBy ? 1f : 0.3f;
            material.color = color;
        }
    }
}