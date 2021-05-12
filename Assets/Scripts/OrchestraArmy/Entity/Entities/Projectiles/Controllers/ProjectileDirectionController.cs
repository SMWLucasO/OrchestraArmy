using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles.Controllers
{
    public class ProjectileDirectionController: IDirectionController
    {
        public DirectionalEntity Entity { get; set; }
        public Camera Camera { get; set; }
        public EntityDirection CurrentDirection { get; private set; }
        public Vector3 AimDirection { get; }

        public void HandleDirection()
        {
            CurrentDirection = EntityDirection.Top;
        }
    }
}