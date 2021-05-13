using System;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Data
{
    [Serializable]
    public class WeaponWheelPlaceholderData
    {
        [SerializeField]
        private bool _unlocked = false;

        [SerializeField]
        private WeaponType _weaponType = WeaponType.Guitar;

        private IWeapon _weapon;
        
        /// <summary>
        /// Boolean indicating whether the weapon is unlocked or not. 
        /// </summary>
        public bool Unlocked { 
            get => _unlocked;
            set => _unlocked = value;
        }

        /// <summary>
        /// The type of the weapon which this placeholder holds.
        /// </summary>
        public WeaponType WeaponType => _weaponType;

        /// <summary>
        /// The icon for this weapon placeholder.
        /// </summary>
        [field: SerializeField] 
        public Sprite WeaponPlaceholderIcon { get; set; }
        
        /// <summary>
        /// The weapon on the weapon wheel placeholder.
        /// </summary>
        public IWeapon Weapon
        {
            get => _weapon ??= WeaponFactory.Make(WeaponType);
            set => _weapon = value;
        }
        
    }
}