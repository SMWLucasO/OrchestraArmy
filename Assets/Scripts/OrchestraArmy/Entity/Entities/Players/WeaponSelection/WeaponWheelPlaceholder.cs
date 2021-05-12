using System;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Data;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection
{
    /// <summary>
    /// The weapon's placeholder.
    /// </summary>
    [Serializable]
    public class WeaponWheelPlaceholder : MonoBehaviour
    {
        /// <summary>
        /// The previous weapon placeholder on the weapon wheel.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholder Previous { get; set; }
        
        /// <summary>
        /// The next weapon placeholder on the weapon wheel.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholder Next { get; set; }
        
        /// <summary>
        /// The data of this weapon wheel placeholder.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholderData  WeaponWheelPlaceholderData { get; set; }
        
    }

}