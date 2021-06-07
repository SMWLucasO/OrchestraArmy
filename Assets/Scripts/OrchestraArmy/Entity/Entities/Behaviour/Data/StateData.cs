using System;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Entity.Entities.Players;

namespace OrchestraArmy.Entity.Entities.Behaviour.Data
{
    public class StateData
    {
        /// <summary>
        /// The owner of this state.
        /// </summary>
        public Enemy Enemy { get; set; }
        
        /// <summary>
        /// The target of this state.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The type of the projectile to be shot.
        /// </summary>
        public Type ProjectileType { get; set; }

        /// <summary>
        /// The amount of projectiles that get fired at once.
        /// </summary>
        public int ProjectileCount { get; set; } = 1;
        
    }
}