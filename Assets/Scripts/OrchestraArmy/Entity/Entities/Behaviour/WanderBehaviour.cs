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
            
            // When the enemy is within 3 units of the player & it can detect the player: attack.
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 6 + scale.x))
            {
                machine.SetState(new AttackBehaviour());
                return;
            }


            // When the player is within 5 units of the enemy: move towards player.
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

        /*
        TODO: OLD (kept just in case. Bram van der Leest may remove this.):
        
        _displacementLength += (Random.value * 2 - 1) * _displacementAltStrength;
            _displacementLength = Mathf.Abs(_displacementLength);                           //calculate the new displacementLength
            
            _displacementAngle += (Random.value * 2 - 1) * _displacementAltStrength;
            _displacementAngle = Mathf.Abs(_displacementAngle) >= (2 * Mathf.PI)            //calculate the new displacementAngle
                ? Mathf.Abs(_displacementAngle) - (2 * Mathf.PI)
                : Mathf.Abs(_displacementAngle);
            
            Vector3 currentVector = StateData.Enemy.RigidBody.velocity;                     //the current heading
            currentVector = currentVector.normalized * 75;                                  //the current heading at 75 speed (50<=velocity<=100)
            currentVector = currentVector + (new Vector3(Mathf.Sin(_displacementAngle),
                                                         0,
                                                         Mathf.Cos(_displacementAngle)) * _displacementLength);   //calculate the new heading
            
            StateData.Enemy.RigidBody.velocity = currentVector;                             //overwrite the old velocity
            
            //try to see the player
            Vector3 direction = (StateData.Player.RigidBody.position-StateData.Enemy.RigidBody.position).normalized;    //angle to the player
            Ray r = new Ray(StateData.Enemy.RigidBody.position, direction);
            float detectionRange = 20f;                                                     //20 units detection range
            
            Physics.Raycast(r,out RaycastHit hitEntity, detectionRange);
            if (hitEntity.transform.CompareTag("Player"))
            {
                machine.SetState(new AggroBehaviour());    //TODO:connect to aggroBehaviour
            }
        
        */

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit() { }

    }
}