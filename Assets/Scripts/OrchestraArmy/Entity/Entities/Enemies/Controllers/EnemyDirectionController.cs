using OrchestraArmy.Entity.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OrchestraArmy.Entity.Entities.Enemies.Controllers
{
    public class EnemyDirectionController: IDirectionController
    {
        public DirectionalEntity Entity { get; set; }
        public Camera Camera { get; set; } = Camera.main;
        public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Top;
        
        public Vector3 AimDirection { get; set; }

        private Vector3 _previousPosition;

        public void HandleDirection()
        {
            // Get enemy direction
            Vector3 enemyDirection = Entity.transform.position - _previousPosition;

            // Get camera direction
            Vector3 cameraDirection = Camera.transform.forward;


            // Find the angle between two vectors
            float angle = GetAngle(cameraDirection, enemyDirection);

            // Set the direction based on the angle
            /*
                  \   T   /
                    \   /
                 L    .    R
                    /   \
                  /   D   \
            */
            if(Mathf.PI/4*1 <= angle && angle < Mathf.PI/4*3 )
            {
                CurrentDirection = EntityDirection.Top;
            }
            else if(Mathf.PI/4*3 <= angle && angle < Mathf.PI/4*5)
            {
                CurrentDirection = EntityDirection.Left;
            }
            else if(Mathf.PI/4*5 <= angle && angle < Mathf.PI/4*7)
            {
                CurrentDirection = EntityDirection.Down;
            }
            else
            {
                CurrentDirection = EntityDirection.Right;
            }
        
        }
        

        private Vector3? CalculateAimDirection(Vector2 mousePosition, Vector3 entityPosition)
        {
            var ray = Camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit))
                return null;
            
            return hit.point - entityPosition;
        }

        private float GetAngle(Vector3 a, Vector3 b)
        {
            float dotProduct = (b.x * a.z) + (a.x * b.z);
            float magnitude = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(a.x, 2) + Mathf.Pow(a.z, 2))) * 
            Mathf.Abs(Mathf.Sqrt(Mathf.Pow(b.x, 2) + Mathf.Pow(b.z, 2)));
            return Mathf.Acos(dotProduct/magnitude);
        }
    }
}