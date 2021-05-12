using System.Collections;
using OrchestraArmy.Entity.Entities.Projectiles.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class Projectile: LivingDirectionalEntity
    {
        public Vector3 Source { get; set; }

        public float Speed { get; set; }
        public float MaxDistance { get; set; }
        protected bool Hit { get; set; } = false;
        
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

            do
            {
                distance = Vector3.Distance(Source, transform.position);
                MovementController.HandleMovement();

                yield return null;
            } while (distance < MaxDistance && !Hit);
            
            Destroy(gameObject);
        }
    }
}