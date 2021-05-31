using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using UnityEngine;

namespace OrchestraArmy.Event.Events.Enemy
{
    public class BossDeathEvent: IEvent
    {
        
        /// <summary>
        /// The position at which the boss died.
        /// </summary>
        public Vector3 PositionOfDeath { get; set; }
        
        /// <summary>
        /// The instrument which is to be dropped by the boss.
        /// </summary>
        public WeaponType InstrumentToAward { get; set; }
        
    }
}