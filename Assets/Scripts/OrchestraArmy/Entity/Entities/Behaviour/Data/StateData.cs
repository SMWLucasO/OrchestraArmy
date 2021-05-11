﻿using OrchestraArmy.Entity.Entities.Players;

namespace OrchestraArmy.Entity.Entities.Enemies.Data
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
        
        
    }
}