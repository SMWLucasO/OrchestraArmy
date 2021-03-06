using OrchestraArmy.Entity.Entities;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IMovementController
    {
        /// <summary>
        /// The entity whose movement is to be controlled.
        /// </summary>
        public MovingEntity Entity { get; set; }
        
        /// <summary>
        /// Handle the entity's movement.
        /// </summary>
        public void HandleMovement();

    }
}