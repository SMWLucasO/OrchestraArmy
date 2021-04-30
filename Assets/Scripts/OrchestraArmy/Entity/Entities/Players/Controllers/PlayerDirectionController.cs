using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
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
            var cameraRotation = Camera.transform.rotation.eulerAngles;

            if (Mathf.Abs(Vector3.Distance(mousePosition, entityScreenPosition)) < 40)
            {
                return;
            }
            
            var angleRadians = Mathf.Atan2(entityScreenPosition.y - mousePosition.y, entityScreenPosition.x - mousePosition.x);
            var angle = angleRadians * (180 / Mathf.PI);
            var compensatedAngle = angle - cameraRotation.y;
            var compensatedRadians = compensatedAngle * (Mathf.PI / 180);

            if (angle <= -45 && angle >= -135)
                CurrentDirection = EntityDirection.Top;
            else if (angle <= -135 || angle >= 135)
                CurrentDirection = EntityDirection.Right;
            else if (angle >= 45 && angle <= 135)
                CurrentDirection = EntityDirection.Down;
            else if (angle >= -45 && angle <= 135)
                CurrentDirection = EntityDirection.Left;

            var directionVector = new Vector3(-Mathf.Cos(compensatedRadians), 0, -Mathf.Sin(compensatedRadians));
            
            Entity.transform.forward = directionVector;     

            var child = Entity.transform.GetChild(0);
            child.rotation = new Quaternion();
            child.Rotate(new Vector3(0, angle + 90 % 360, 0), Space.Self);
        }
    }
}