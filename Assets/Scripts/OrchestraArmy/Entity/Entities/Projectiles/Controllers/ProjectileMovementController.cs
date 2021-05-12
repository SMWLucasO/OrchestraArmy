using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles.Controllers
{
    public class ProjectileMovementController: IMovementController
    {
        public MovingEntity Entity { get; set; }
        public void HandleMovement()
        {
            var projectile = (Projectile) Entity;
            Entity.RigidBody.velocity = Entity.transform.forward * projectile.Speed;
        }
    }
}