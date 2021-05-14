using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerCameraController : ICameraController
    {
        

        /// <summary>
        /// The increment in degrees which the camera will get.
        /// Increment is in seconds.
        /// </summary>
        public const float CameraRotationIncrement = 50f;

        /// <summary>
        /// The offset at which the camera rotates around the player.
        /// </summary>
        public Vector3 CameraOffset = new Vector3(0, -1.3f, 1f);
        
        /// <summary>
        /// The camera controller's owner.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The camera which is owned by the player.
        /// </summary>
        public Camera Camera { get; set; } = Camera.main;

        /// <summary>
        /// The rotation along the y-axis (left/right)
        /// </summary>
        public float Yaw { get; set; }
        
        public void HandleCameraMovement()
        {

            Transform playerTransform = Player.transform;
            Transform cameraTransform = Camera.transform;

            // Get the initial position for the point to place the camera at.
            Vector3 cameraPosition = playerTransform.position;

            // Get the player's forward, so that we can put the camera behind the player.
            // we do not need the y-axis, so we set it to 0 for the forward.
            Vector3 playerForward = playerTransform.forward.normalized;
            playerForward.y = 0;

            // calculate the x-rotation
            if (Input.GetKey(KeybindingManager.Instance.Keybindings["Rotate camera right"]))
                Yaw -= CameraRotationIncrement * Time.deltaTime;
            
            if (Input.GetKey(KeybindingManager.Instance.Keybindings["Rotate camera left"]))
                Yaw += CameraRotationIncrement * Time.deltaTime;

            // Get the offset for rotations around the player
            Vector3 offset = CameraOffset;

            // [0, 360] degrees available.
            Yaw %= 360;
            
            // place the camera around the player, whilst also pointing it towards the player.
            cameraTransform.position = cameraPosition - (Quaternion.Euler(0, Yaw, 0) * offset);
            
            cameraTransform.LookAt(playerTransform); 
        }
    }
}