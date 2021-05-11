using System;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Data
{
    [Serializable]
    public class WeaponWheelPlaceholderData
    {

        [SerializeField]
        private bool _unlocked = false;

        /// <summary>
        /// Boolean indicating whether the weapon is unlocked or not. 
        /// </summary>
        public bool Unlocked { 
            get => _unlocked;
            set => _unlocked = value;
        }
        
        /// <summary>
        /// The weapon on the weapon wheel placeholder.
        /// </summary>
        public IWeapon Weapon { get; set; }
        
    }
}