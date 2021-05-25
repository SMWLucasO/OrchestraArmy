using System;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using OrchestraArmy.Entity.Entities.Enemies;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class WanderBehaviour : IBehaviourState
    {

        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

        /// <summary>
        /// The point where the enemy is going.
        /// </summary>
        public Vector3 WanderDestination { get; set; }
        
        /// <summary>
        /// Enter this state.
        /// </summary>
        public void Enter()
        {
            
        }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {

            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 3))
            {
                //machine.SetState(new AttackBehaviour());
                return;
            }


            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy))
            {
                machine.SetState(new MoveToPlayerBehaviour());
                return;
            }
            
            NavMeshPathStatus pathStatus = StateData.Enemy.NavMeshAgent.pathStatus;
            if (!StateData.Enemy.transform.position.Equals(StateData.Enemy.NavMeshAgent.destination) &&
                pathStatus != NavMeshPathStatus.PathInvalid && pathStatus != NavMeshPathStatus.PathPartial)
                return;
            
            Vector3 randomTowards = new Vector3(Random.Range(3, 5), 0, Random.Range(3, 5));
            StateData.Enemy.NavMeshAgent.SetDestination(StateData.Enemy.SpawnLocation + randomTowards);
            
        }

        /*
        OLD:
        
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
        public void Exit()
        {
            
        }
        
        /// <summary>
        /// calculate the x and y force of a given angle and velocity
        /// </summary>
        public Vector3 setAngle(Vector3 vec, float angleChange) {
            float len = vec.magnitude;
            return new Vector3(Mathf.Cos(angleChange),0,Mathf.Sin(angleChange));
        }

    }
}