using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Drops
{
    public class InstrumentDrop : DirectionalEntity
    {
        
        /// <summary>
        /// The type of instrument drop.
        /// </summary>
        [field: SerializeField]
        public WeaponType WeaponType { get; set; }
        
    }
}