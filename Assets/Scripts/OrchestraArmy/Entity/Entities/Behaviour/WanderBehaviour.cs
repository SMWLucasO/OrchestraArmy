using System;
using OrchestraArmy.Entity.Entities.Enemies.Data;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public class WanderBehaviour : IBehaviourState
    {
        /// <summary>
        /// variables needed for wander behaviour
        /// </summary>

        private float _displacementLength = Random.value * 25;           //distance from center
        private float _displacementAngle = Random.value * (2*Mathf.PI);  //angle of displacement
        private float _displacementAltStrength = 5f;
        
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
                machine.SetState(new WanderBehaviour());    //TODO:connect to aggroBehaviour
            }
            
        }

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