using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace OrchestraArmy.Entity.Controllers
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
            
            // code that could be used to support attacks in all possible directions based on mouse position, but is off at some angles by a bit.
            //
            // var screenSpace = Camera.WorldToScreenPoint(entityPosition);
            // var mouseWorldPosition = Camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, screenSpace.z));
            //
            // AimDirection = (mouseWorldPosition - entityPosition);
            // AimDirection.y = 0;
            // AimDirection.Normalize();

            if (Input.GetMouseButtonDown(1))
            {
                GameObject.Instantiate(Entity.sphere, entityPosition, Quaternion.LookRotation(AimDirection));
            }

            // var camRotation = Camera.transform.rotation.eulerAngles;
            //
            // Entity.transform.GetChild(0).rotation = Quaternion.Euler(0, camRotation.y, 0);
        }
    }
}