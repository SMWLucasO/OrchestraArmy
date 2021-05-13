using OrchestraArmy.Entity.Entities.Players;
using UnityEngine;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IAttackController
    {
        /// <summary>
        /// The player which is attacking
        /// </summary>
       public Player Player { get; set; }
        
        /// <summary>
        /// Handle the player's attack.
        /// </summary>
        public void HandleAttack();
    }
}