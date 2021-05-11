using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection
{
    public class WeaponWheel : MonoBehaviour
    {
        
        /// <summary>
        /// The weapon wheel's currently selected placeholder.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholder CurrentlySelected { get; set; }

        /// <summary>
        /// Select the next weapon for usage.
        /// </summary>
        /// <returns>bool determining if the weapon was switched.</returns>
        public bool SwitchToNextWeapon()
        {
            if (!WeaponIsUnlocked(CurrentlySelected.Next))
                return false;

            WeaponType prev = CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;
            WeaponType newWeaponType = CurrentlySelected.Next.WeaponWheelPlaceholderData.WeaponType;
            
            // switch weapons
            CurrentlySelected = CurrentlySelected.Next;
            
            ExecuteWeaponSwitchedEvent(prev, newWeaponType);

            return true;
        }
        
        /// <summary>
        /// Select the previous weapon for usage.
        /// </summary>
        /// <returns>bool determining if the weapon was switched.</returns>
        public bool SwitchToPreviousWeapon()
        {
            if (!WeaponIsUnlocked(CurrentlySelected.Previous))
                return false;
            
            WeaponType prev = CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;
            WeaponType newWeaponType = CurrentlySelected.Previous.WeaponWheelPlaceholderData.WeaponType;
            
            // switch weapons
            CurrentlySelected = CurrentlySelected.Previous;
            
            // fire event for weapon switch.
            ExecuteWeaponSwitchedEvent(prev, newWeaponType);

            return true;
        }
        
        /// <summary>
        /// Check whether the weapon in the weapon placeholder is unlocked.
        /// </summary>
        /// <param name="weaponWheelPlaceholder"></param>
        /// <returns></returns>
        private bool WeaponIsUnlocked(WeaponWheelPlaceholder weaponWheelPlaceholder) 
            => weaponWheelPlaceholder.WeaponWheelPlaceholderData.Unlocked;

        /// <summary>
        /// Execute the event which indicates that the weapon has been switched.
        /// </summary>
        /// <param name="previousWeapon"></param>
        /// <param name="newlySelectedWeapon"></param>
        private void ExecuteWeaponSwitchedEvent(WeaponType previousWeapon, WeaponType newlySelectedWeapon)
            => EventManager.Invoke(new PlayerWeaponChangedEvent()
            {
                PreviousWeapon = previousWeapon,
                NewlySelectedWeapon = newlySelectedWeapon
            });

    }
}
