using System;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class WanderBehaviour : IBehaviourState
    {


        private float _timePerWanderInSecondsMin = 1.5f, 
            _timePerWanderInSecondsMax = 4f;

        private float _timeElapsedSinceLastWanderCheckInSeconds = 0;
        
        private float _timeElapsedSinceWanderInSeconds = 0;
        
        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

        /// <summary>
        /// Enter this state.
        /// </summary>
        public void Enter() { }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {

            Vector3 scale = StateData.Enemy.transform.localScale;
            
            // When the enemy is within 6 units of the player & it can detect the player: attack.
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 7 + scale.x))
            {
                machine.SetState(new AttackBehaviour());
                return;
            }


            // When the player is within 11 units of the enemy: move towards player.
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 11 + scale.x))
            {
                machine.SetState(new MoveToPlayerBehaviour());
                return;
            }
            
            // if the entity has not arrived, and the end can be reached: wait till the entity has arrived.
            NavMeshPathStatus pathStatus = StateData.Enemy.NavMeshAgent.pathStatus;
            if (!StateData.Enemy.transform.position.Equals(StateData.Enemy.NavMeshAgent.destination) &&
                pathStatus != NavMeshPathStatus.PathInvalid && pathStatus != NavMeshPathStatus.PathPartial)
                return;

            _timeElapsedSinceWanderInSeconds += Time.deltaTime;
            _timeElapsedSinceLastWanderCheckInSeconds += Time.deltaTime;

            // only change directions at most once per second.
            if (Math.Floor(_timeElapsedSinceLastWanderCheckInSeconds) < 1)
                return;

            _timeElapsedSinceLastWanderCheckInSeconds = 0;
            
            // Chance of movement, depending on [min, max]
            if (_timeElapsedSinceWanderInSeconds < Random.Range(_timePerWanderInSecondsMin, _timePerWanderInSecondsMax))
                return;

            _timeElapsedSinceWanderInSeconds = 0;
            
            // vv generate a position to move towards. vv
            
            Vector3 randomDirectionUnitCircle = Random.insideUnitSphere;
            randomDirectionUnitCircle.y = 0;
            
            Vector3 randomDirection = StateData.Enemy.transform.forward + randomDirectionUnitCircle;
            Vector3 newPosition = StateData.Enemy.SpawnLocation + (randomDirection * 3);
            
            StateData.Enemy.NavMeshAgent.SetDestination(newPosition);
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit() { }

    }
}