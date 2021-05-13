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
            
            
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            
        }
    }
}