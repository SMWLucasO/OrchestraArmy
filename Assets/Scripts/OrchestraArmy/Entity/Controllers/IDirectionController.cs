using UnityEngine;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IDirectionController
    {
        /// <summary>
        /// The living entity to control the direction for.
        /// </summary>
        public DirectionalEntity Entity { get; set; }
        
        /// <summary>
        /// The camera to set the direction relative to.
        /// </summary>
        public Camera Camera { get; set; }
        
        /// <summary>
        /// The current direction
        /// </summary>
        public EntityDirection CurrentDirection { get; }
        
        /// <summary>
        /// Aim direction, bit more precise than move direction. Only set on mouse click.
        /// </summary>
        public Vector3 AimDirection { get; }


        /// <summary>
        /// Handle the entity's direction relative to the camera.
        /// </summary>
        public void HandleDirection();
    }
}