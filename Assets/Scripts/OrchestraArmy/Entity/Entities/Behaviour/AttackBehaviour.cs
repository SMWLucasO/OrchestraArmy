using System;
using OrchestraArmy.Entity.Entities.Enemies.Data;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public class AttackBehaviour : IBehaviourState
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
            EventManager.Invoke(new CombatInitiatedEvent() {Entity = StateData.Enemy});
        }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {
            
            //next behaviour check
            Vector3 direction = (StateData.Player.RigidBody.position-StateData.Enemy.RigidBody.position).normalized;    //angle to the player
            Ray r = new Ray(StateData.Enemy.RigidBody.position, direction);             //ray to the player
            float attackRange = 4f;                                                          //2 units detection range
            
            Physics.Raycast(r,out RaycastHit hitEntity, attackRange);
            if (!hitEntity.transform.CompareTag("Player"))                                    //if further then 4 units from player
                machine.SetState(new AggroBehaviour());    //TODO:connect to aggroBehaviour
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            EventManager.Invoke(new LeaveCombatEvent() {Entity = StateData.Enemy});
        }
    }
}