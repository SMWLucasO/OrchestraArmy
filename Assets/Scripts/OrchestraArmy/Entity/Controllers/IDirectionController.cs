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
        /// Handle the entity's direction relative to the camera.
        /// </summary>
        public void HandleDirection();

    }
}