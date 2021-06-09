using System.Collections;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Projectiles.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class Projectile : MovingEntity
    {
        public Vector3 Source { get; set; }
        
        public float MaxDistance { get; set; }
        protected bool Hit { get; set; } = false;
        public IMovementController MovementController { get; set; }
        
        protected override void OnEnable()
        {
            MovementController = new ProjectileMovementController()
            {
                Entity = this
            };

            DirectionController = new ProjectileDirectionController();
            
            DirectionController.HandleDirection();
            InitializeSprites();
            
            SpriteManager.UpdateSprite();

            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            yield return new WaitForSeconds(0.01f);
            
            var distance = 0f;
            var previous = Source;

            do
            {
                var position = transform.position;
                distance += Vector3.Distance(previous, position);
                previous = position;
                MovementController.HandleMovement();

                yield return null;
            } while (distance < MaxDistance && !Hit);
            
            Destroy(gameObject);
        }
    }
}