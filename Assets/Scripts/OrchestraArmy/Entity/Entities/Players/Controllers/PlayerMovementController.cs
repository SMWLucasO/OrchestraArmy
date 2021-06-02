using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerMovementController : IMovementController
    {
        public MovingEntity Entity { get; set; }
        public void HandleMovement()
        {
            
            var forward = Entity.transform.forward;

            Vector3 currentlyFacing = GetHeightlessFacingDirection(forward);
            Vector3 movementVector = Vector3.zero;
            
            if (KeybindingManager.Instance.Keybindings["Move forward"].isPressed)
                movementVector += currentlyFacing;

            if (KeybindingManager.Instance.Keybindings["Move backward"].isPressed)
                movementVector -= currentlyFacing;

            // Only add to the position when a change is actually detected.
            Entity.RigidBody.velocity = (movementVector.normalized * (Entity.MovementData.WalkSpeed * Time.deltaTime));
        }

        /// <summary>
        /// Generate a facing vector containing only the x/z positions
        /// </summary>
        /// <param name="facingDirection">The direction which we are currently facing.</param>
        /// <returns>Vector3</returns>
        private Vector3 GetHeightlessFacingDirection(Vector3 facingDirection) 
            => new Vector3(facingDirection.x, 0, facingDirection.z);
        
        /// <summary>
        /// Generate a force vector towards the left side of the given vector.
        /// </summary>
        /// <param name="facingDirection">The vector to generate a force for.</param>
        /// <returns>Vector3</returns>
        private Vector3 MoveLeftForce(Vector3 facingDirection) 
            => Quaternion.Euler(0f, -90f, 0f) * facingDirection;

        /// <summary>
        /// Generate a force vector towards the right side of the given vector.
        /// </summary>
        /// <param name="facingDirection">The vector to generate a force for.</param>
        /// <returns>Vector3</returns>
        private Vector3 MoveRightForce(Vector3 facingDirection)
            => Quaternion.Euler(0f, 90f, 0f) * facingDirection;

    }
}