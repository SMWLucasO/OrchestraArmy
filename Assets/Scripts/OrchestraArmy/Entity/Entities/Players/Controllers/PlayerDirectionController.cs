using OrchestraArmy.Entity.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerDirectionController: IDirectionController
    {
        public DirectionalEntity Entity { get; set; }
        public Camera Camera { get; set; } = Camera.main;
        public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Top;
        
        public Vector3 AimDirection { get; set; }

        public void HandleDirection()
        {
            var entityPosition = Entity.transform.position;
            var centerPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var mousePosition = Mouse.current.position.ReadValue();
            var cameraRotation = Camera.transform.rotation.eulerAngles;

            var angleRadians = Mathf.Atan2(centerPosition.y - mousePosition.y,
                centerPosition.x - mousePosition.x);
            var angle = angleRadians * (180 / Mathf.PI);

            var compensatedAngle = angle - cameraRotation.y;
            var compensatedRadians = compensatedAngle * (Mathf.PI / 180);

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                if (angle <= -45 && angle >= -135)
                    CurrentDirection = EntityDirection.Top;
                else if (angle <= -135 || angle >= 135)
                    CurrentDirection = EntityDirection.Right;
                else if (angle >= 45 && angle <= 135)
                    CurrentDirection = EntityDirection.Down;
                else if (angle >= -45 && angle <= 135)
                    CurrentDirection = EntityDirection.Left;
            }
            else
            {
                CurrentDirection = EntityDirection.Top;
            }

            var directionVector = new Vector3(-Mathf.Cos(compensatedRadians), 0, -Mathf.Sin(compensatedRadians));

            if (!Keybindings.KeybindingManager.Instance.Keybindings["Shoot"].wasPressedThisFrame)
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                    AimDirection = CalculateAimDirection(mousePosition, entityPosition) ?? directionVector;
                else
                    AimDirection = Entity.transform.forward;
            }
        }

        private Vector3? CalculateAimDirection(Vector2 mousePosition, Vector3 entityPosition)
        {
            var ray = Camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit))
                return null;
            
            return hit.point - entityPosition;
        }
    }
}