using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Enum;
using UnityEngine;

namespace OrchestraArmy.Event.Events.Enemy
{
    public class EnemyAttackEvent : IEvent
    {
        /// <summary>
        /// Enemy instrument
        /// </summary>
        public WeaponType Instrument;
        
        /// <summary>
        /// Enemy current tone
        /// </summary>
        public Tone Tone;
        
        /// <summary>
        /// Enemy position
        /// </summary>
        public Vector3 Position;
    }
}