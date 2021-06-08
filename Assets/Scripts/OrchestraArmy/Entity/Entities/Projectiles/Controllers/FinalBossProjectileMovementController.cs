using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles.Controllers
{
    public class FinalBossProjectileMovementController : IMovementController
    {
        
        /// <summary>
        /// The projectile.
        /// </summary>
        public MovingEntity Entity { get; set; }
        
        /// <summary>
        /// The player to move towards eventually.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The position from which the note got shot.
        /// </summary>
        public Vector3 StartingPosition;

        /// <summary>
        /// The distance traveled before seeking.
        /// </summary>
        public float DistanceBeforeSeek = 3f;
        
        /// <summary>
        /// The movement controller currently being executed.
        /// </summary>
        public IMovementController MovementController { get; set; }

        /// <summary>
        /// Boolean determining whether the initial movement controller is set.
        /// </summary>
        private bool _initialMovementInitiated = false;

        public void HandleMovement()
        {
            // Initially, we just move the projectile forward.
            if (!_initialMovementInitiated)
            {
                _initialMovementInitiated = true;
                MovementController = new ProjectileMovementController()
                {
                    Entity = Entity
                };
            }

            MovementController.HandleMovement();
        }
    }
}