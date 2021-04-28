using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player.Controllers
{
    public class PlayerDirectionController: IDirectionController
    {
        public DirectionalEntity Entity { get; set; }
        public Camera Camera { get; set; } = Camera.main;
        public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Top;
        
        public Vector3 AimDirection = Vector3.up;

        public void HandleDirection()
        {
            var entityPosition = Entity.transform.position;
            var entityScreenPosition = Camera.WorldToScreenPoint(entityPosition);
            var mousePosition = Input.mousePosition;
            var angleRadians = Mathf.Atan2(entityScreenPosition.y - mousePosition.y, entityScreenPosition.x - mousePosition.x);
            var angle = angleRadians * (180 / Mathf.PI);

            if (angle < -45 && angle > -135)
            {
                CurrentDirection = EntityDirection.Top;
                AimDirection = Entity.transform.forward;
            }
            else if (angle < -135 || angle > 135)
            {
                CurrentDirection = EntityDirection.Right;
                AimDirection = Entity.transform.right;
            }
            else if (angle > 45 && angle < 135)
            {
                CurrentDirection = EntityDirection.Down;
                AimDirection = -Entity.transform.forward;
            }
            else if (angle > -45 && angle < 135)
            {
                CurrentDirection = EntityDirection.Left;
                AimDirection = -Entity.transform.right;
            }
        }
    }
}