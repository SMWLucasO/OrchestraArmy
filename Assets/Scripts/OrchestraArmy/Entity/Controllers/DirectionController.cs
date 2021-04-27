using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace OrchestraArmy.Entity.Controllers
{
    public class DirectionController: IDirectionController
    {
        public DirectionalEntity Entity { get; set; }
        public Camera Camera { get; set; } = Camera.main;
        public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Top;
        public Vector3 AimDirection = Vector3.forward;

        private void UpdateSprite()
        {
            var newSprite = Entity.Sprites.FirstOrDefault(s => s.Direction == CurrentDirection);

            if (newSprite.Sprites == null || newSprite.Sprites.Length == 0)
                return;

            Entity.CurrentSpriteSet = newSprite.Sprites;
        }
        
        public void HandleDirection()
        {
            var entityPosition = Entity.transform.position;
            var entityScreenPosition = Camera.WorldToScreenPoint(entityPosition);
            var mousePosition = Input.mousePosition;
            var angle = Mathf.Atan2(entityScreenPosition.y - mousePosition.y, entityScreenPosition.x - mousePosition.x) * (180 / Mathf.PI);
            
            if (angle < -45 && angle > -135)
                CurrentDirection = EntityDirection.Top;
            else if (angle < -135 || angle > 135)
                CurrentDirection = EntityDirection.Right;
            else if (angle > 45 && angle < 135)
                CurrentDirection = EntityDirection.Down;
            else if (angle > -45 && angle < 135)
                CurrentDirection = EntityDirection.Left;

            Plane plane = new Plane(Vector3.up, 0);

            Ray ray = Camera.ScreenPointToRay(mousePosition);
            var mouseWorldPosition = Camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.transform.position.y));

            if (plane.Raycast(ray, out var dist))
            {
                mouseWorldPosition = ray.GetPoint(dist);
            }
            
            AimDirection = (mouseWorldPosition - entityPosition).normalized;
            AimDirection.y = 0;

            if (Input.GetMouseButtonDown(1))
            {
                GameObject.Instantiate(Entity.sphere, entityPosition, Quaternion.LookRotation(AimDirection));
            }

            Debug.Log(AimDirection.x);
            Debug.Log(AimDirection.y);
            Debug.Log(AimDirection.z);
            Debug.Log("---------------");
            Debug.DrawRay(Entity.transform.position, AimDirection * 100, Color.red);
            
            UpdateSprite();
        }
    }
}