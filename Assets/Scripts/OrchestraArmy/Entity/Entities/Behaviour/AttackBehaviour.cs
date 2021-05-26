using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class AttackBehaviour : IBehaviourState, IListener<EnemyTurnEvent>
    {
        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

        /// <summary>
        /// Enter this state.
        /// </summary>
        public void Enter()
        {
            EventManager.Bind<EnemyTurnEvent>(this);
            EventManager.Invoke(new CombatInitiatedEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {
            
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy))
            {
                if (Vector3.Distance(StateData.Player.transform.position, StateData.Enemy.transform.position) > 3)
                {
                    machine.SetState(new MoveToPlayerBehaviour());
                    return;
                }
            }

            if (Vector3.Distance(StateData.Player.transform.position, StateData.Enemy.transform.position) > 5)
            {
                machine.SetState(new WanderBehaviour());
                return;
            }
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            EventManager.Unbind<EnemyTurnEvent>(this);
            EventManager.Invoke(new LeaveCombatEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        public void OnEvent(EnemyTurnEvent invokedEvent)
        {
            if (invokedEvent.EnemyId != StateData.Enemy.GetInstanceID())
                return;
            Transform enemyTransform = StateData.Enemy.transform;
            Transform playerTransform = StateData.Player.transform;

            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            Vector3 enemyPosition = enemyTransform.position;
            enemyPosition.y = 0.5f;

            // calculate vector from enemy to player
            var playerPosition = playerTransform.position;
            enemyTransform.forward = (playerPosition - enemyPosition).normalized;
            
            // generate the enemy note to be shot.
            var obj = (GameObject) Object.Instantiate(
                Resources.Load("Prefabs/EnemyNoteProjectile"),
                enemyPosition + (enemyTransform.forward * scale.x),
                StateData.Enemy.transform.GetChild(0).transform.rotation
            );
            
            var attack = obj.GetComponent<EnemyNote>();
            // calculate the vector from the note prefab to the player
            attack.transform.forward = (playerPosition - obj.transform.position).normalized;
            
            // set the attacking source.
            attack.Source = obj.transform.position;
            
            // max distance = 5 Unity units
            attack.MaxDistance = 5;
            
            attack.Instrument = WeaponFactory.Make(StateData.Enemy.WeaponType);
            attack.Attacker = StateData.Enemy;
            attack.Tone = Tone.A; // TODO: determine this another way.

            EventManager.Invoke(new EnemyAttackEvent()
            {
                Tone = attack.Tone,
                Instrument = StateData.Enemy.WeaponType
            });
        }
    }
}