using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Projectiles.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class BossNote : EnemyNote
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Make use of seeking notes for bosses.
            MovementController = new PlayerFollowingProjectileMovementController()
            {
                Entity = this,
                Player = GameObject.FindWithTag("Player").GetComponent<Player>()
            };
        }
    }
}