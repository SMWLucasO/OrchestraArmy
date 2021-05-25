using System;
using OrchestraArmy.Entity.Entities.Enemies.Data;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public class AggroBehaviour : IBehaviourState
    {
        /// <summary>
        /// variables needed for wander behaviour
        /// </summary>
        
        
        
        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

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
            Vector3 direction = (StateData.Player.RigidBody.position-StateData.Enemy.RigidBody.position).normalized;    //angle to the player
            Ray r = new Ray(StateData.Enemy.RigidBody.position, direction);             //ray to the player
            float attackRange = 2f;                                                          //2 units detection range
            
            Physics.Raycast(r,out RaycastHit hitEntity, attackRange);
            if (hitEntity.transform.CompareTag("Player"))                                    //if closer then 2 units from player
                machine.SetState(new AttackBehaviour());    //TODO:connect to attackBehaviour
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            
        }
    }
}