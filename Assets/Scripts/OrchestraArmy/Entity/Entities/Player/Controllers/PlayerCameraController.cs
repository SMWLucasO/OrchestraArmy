using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player.Controllers
{
    public class PlayerCameraController : ICameraController
    {
        /// <summary>
        /// The distance the camera should be from the player.
        /// </summary>
        public const float DistanceFromPlayer = 10.0f;

        /// <summary>
        /// The increment in degrees which the camera will get.
        /// </summary>
        public const float CameraRotationIncrement = .1f;

        /// <summary>
        /// The rotation along the y-axis (Left/Right rotation)
        /// </summary>
        public float Yaw { get; set; }
        
        /// <summary>
        /// The rotation along the x-axis. (Up/Down rotation)
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// The camera controller's owner.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The camera which is owned by the player.
        /// </summary>
        public Camera Camera { get; set; }
        
        public void HandleCameraMovement()
        {

            // Camera position = (player position) - DistanceFromPlayer 
            // then do rotations.s

            var playerTransform = Player.transform;
            var cameraTransform = Camera.transform;
            
            Vector3 newCameraPosition = playerTransform.position;
            
            // Put camera behind the player.
            newCameraPosition -= playerTransform.forward.normalized * DistanceFromPlayer;
            
            // Add pitch/yaw based upon key pressed.
            if (Input.GetKey(KeyCode.UpArrow)) Pitch += CameraRotationIncrement;
            if (Input.GetKey(KeyCode.DownArrow)) Pitch -= CameraRotationIncrement;
            if (Input.GetKey(KeyCode.LeftArrow)) Yaw -= CameraRotationIncrement;
            if (Input.GetKey(KeyCode.RightArrow)) Yaw += CameraRotationIncrement;
            
            // Restrict within a certain y range. (no 360deg rotation)
            // [0, 80] is the allowed range.
            if (Pitch > 80) Pitch = 80;
            if (Pitch < 0) Pitch = 0;

            
            // Yaw should be between 0-360 degrees.
            Yaw %= 360;
            
            
            // Apply the rotations to the camera.
            newCameraPosition = Quaternion.Euler(Pitch, Yaw, 0) * newCameraPosition;
            
            // Change the position, but make sure that the camera is still pointing towards the player.
            cameraTransform.position = newCameraPosition;
            cameraTransform.LookAt(playerTransform);
        }
    }
}