using System;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
            EventManager.Invoke(new EnemyCombatInitiatedEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {
            Transform enemyTransform = StateData.Enemy.transform;
            Transform playerTransform = StateData.Player.transform;
         
            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            float distance = BehaviourUtil.GetScaleInclusiveDistance(StateData.Player, StateData.Enemy);
            
            // If the player can't be seen: go to the MoveToPlayer behaviour.
            if (!BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 8 + scale.x))
            {
                machine.SetState(new MoveToPlayerBehaviour());
                return;
            }
            
            // Movement for combat
            Vector3 playerPosition = playerTransform.position;
            Vector3 playerRelativePosition = enemyTransform.InverseTransformPoint(playerPosition);
            NavMeshAgent navAgent = StateData.Enemy.NavMeshAgent;

            float destDist = 4.0f;
            try
            {
                destDist = (navAgent.destination - playerPosition).magnitude;
            } catch(Exception){}
            
            // Prevent destination outside attack range
            if (destDist<2.5 || destDist>7.5)
            {
                navAgent.isStopped = true;
                navAgent.ResetPath();
            }   
            
            // Avoid to close combat SHOULD NEVER HAPPEN if player is slower than enemy
            if (distance < 2)   
            {
                // Set destination in opposite direction of player to optimal attack range (4 units) 
                navAgent.SetDestination((-playerRelativePosition.normalized * (5 - distance)) + enemyTransform.position);
                return;
            }

            // Prevent jittering movement
            if ((navAgent.destination - enemyTransform.position).magnitude < 0.1f)
            {
                // Do random movement but keep player inside attack area
                // < 2.5 units diameter circle
                Vector3 movement = Random.insideUnitSphere * 1.249f; 
                movement.y = 0;
    
                // Shift so player always in attack area
                movement += (playerRelativePosition - playerRelativePosition.normalized * 5.0f); 
                
                // Move to point movement  (movement is position relative to enemy)
                navAgent.SetDestination(movement + enemyTransform.position);  
            }
            
            
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            EventManager.Unbind<EnemyTurnEvent>(this);
            EventManager.Invoke(new EnemyLeaveCombatEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        public void OnEvent(EnemyTurnEvent invokedEvent)
        {
            StateData.ProjectileTone = Tone.A;
            StateData.AttackController.Enemy = StateData.Enemy;
            StateData.AttackController.Player = StateData.Player;

            StateData.AttackController.HandleAttack();

            if (invokedEvent.EnemyId != StateData.Enemy.GetInstanceID())
                return;
            
            // only fire the attack event when it is the relevant enemies turn
            EventManager.Invoke(new EnemyAttackEvent()
            {
                Tone = StateData.ProjectileTone,
                Instrument = StateData.Enemy.WeaponType,
                Position = StateData.Enemy.transform.position
            });
        }
    }
}