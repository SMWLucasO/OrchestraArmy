using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Projectiles.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class FinalBossNote : Projectile
    {

        protected override void OnEnable()
        {
            MovementController = new FinalBossProjectileMovementController()
            {
                Entity = this,
                StartingPosition = transform.position,
                Player = GameObject.FindWithTag("Player").GetComponent<Player>()
            };
        }
        
        
    }
}