using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerCameraController : ICameraController
    {
        

        /// <summary>
        /// The increment in degrees which the camera will get.
        /// Increment is in seconds.
        /// </summary>
        public const float CameraRotationIncrement = 0.25f;

        /// <summary>
        /// The offset at which the camera rotates around the player.
        /// </summary>
        public Vector3 CameraOffset = new Vector3(0, -1.3f, 1.3f);
        
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

        private Vector2 _dragOrigin;
        
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

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _dragOrigin = Mouse.current.position.ReadValue();
                Cursor.lockState = CursorLockMode.Locked;
            }

            var mouseDelta = Mouse.current.delta.x.ReadValue();
            
            if (Mouse.current.rightButton.isPressed && !Mathf.Approximately(mouseDelta,0))
            {
                Yaw += mouseDelta * CameraRotationIncrement;
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                
                //lets wrap the cursor to above the player, resetting the aim to forward in the progress. Done because the sudden swap from facing forward to the aimdirection can feel disorientating
                var position = _dragOrigin != Mouse.current.position.ReadValue()
                    ? new Vector2(Screen.width / 2f, Screen.height / 1.2f)
                    : _dragOrigin;
                
                Mouse.current.WarpCursorPosition(position);
                
                //work around, WarpCursorPosition only updates in the next input update cycle. This can be to slow, so we'll just set the state manually
                InputState.Change(Mouse.current.position, position);
            }
            
            // Get the offset for rotations around the player
            Vector3 offset = CameraOffset;

            // [0, 360] degrees available.
            Yaw %= 360;
            
            // place the camera around the player, whilst also pointing it towards the player.
            cameraTransform.position = cameraPosition - (Quaternion.Euler(0, Yaw, 0) * offset);
            
            cameraTransform.LookAt(playerTransform);
            Player.transform.forward = Camera.transform.forward;
        }
    }
}