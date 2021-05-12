using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Data;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public class MovingEntity : DirectionalEntity
    {
        /// <summary>
        /// Rigidbody for movement.
        /// </summary>
        public Rigidbody RigidBody { get; set; }
        
        /// <summary>
        /// Movement relevant data.
        /// </summary>
        [field: SerializeField]
        public MovingEntityData MovementData { get; set; }
        
        /// <summary>
        /// The controller for the entity's movement.
        /// </summary>
        public IMovementController MovementController { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.RigidBody = GetComponent<Rigidbody>();
        }
    }
}