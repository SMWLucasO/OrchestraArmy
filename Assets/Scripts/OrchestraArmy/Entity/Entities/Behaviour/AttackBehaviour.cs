﻿using System;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Behaviour
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
            
            // TODO: aanvallen d.m.v. projectielen implementeren.
            
            /*//next behaviour check
             TODO: OLD, Bram van der Leest may remove this.
            Vector3 direction = (StateData.Player.RigidBody.position-StateData.Enemy.RigidBody.position).normalized;    //angle to the player
            Ray r = new Ray(StateData.Enemy.RigidBody.position, direction);             //ray to the player
            float attackRange = 4f;                                                          //2 units detection range
            
            Physics.Raycast(r,out RaycastHit hitEntity, attackRange);
            if (!hitEntity.transform.CompareTag("Player"))                                    //if further then 4 units from player
                machine.SetState(new AggroBehaviour());    //TODO:connect to aggroBehaviour*/
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            
        }
    }
}