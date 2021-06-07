using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles.Controllers
{
    public class PlayerFollowingProjectileMovementController : IMovementController
    {
        /// <summary>
        /// The projectile which is moving towards the player.
        /// </summary>
        public MovingEntity Entity { get; set; }
        
        /// <summary>
        /// The player we are seeking towards
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Boolean determining whether we are still seeking the player.
        /// </summary>
        public bool WithinSeekCancellationRange { get; set; } = false;
        
        public void HandleMovement()
        {
            Transform entityTransform = Entity.transform;
            Transform playerTransform = Player.transform;
            Vector3 direction = entityTransform.forward;
            
            if (Vector3.Distance(playerTransform.position, entityTransform.position) < 1)
                WithinSeekCancellationRange = true;
            
            // Seek towards the player (target) as long as we aren't in the seek cancellation range.
            if (!WithinSeekCancellationRange)
                direction = (playerTransform.position - entityTransform.position).normalized;

            entityTransform.forward = direction;
            Entity.RigidBody.velocity = entityTransform.forward * Entity.MovementData.WalkSpeed;
        }
    }
}