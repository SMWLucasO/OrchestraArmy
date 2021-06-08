using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Projectiles.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class FinalBossNote : EnemyNote
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            
            MovementController = new FinalBossProjectileMovementController()
            {
                Entity = this,
                StartingPosition = transform.position,
                Player = GameObject.FindWithTag("Player").GetComponent<Player>()
            };
        }
    }
}