using System;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection
{
    public class WeaponWheel : MonoBehaviour, IListener<PlayerWeaponChangedEvent>
    {

        /// <summary>
        /// Image[0] = previous weapon
        /// Image[1] = current weapon
        /// Image[2] = next weapon
        ///
        /// Contains the UI image components for the placeholders.
        /// </summary>
        private Image[] _weaponPlaceholderImages; 
        
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

        private void UpdateWeaponWheelImages()
        {
            _weaponPlaceholderImages[0].sprite =
                CurrentlySelected.Previous.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
            
            _weaponPlaceholderImages[1].sprite =
                CurrentlySelected.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
            
            _weaponPlaceholderImages[2].sprite =
                CurrentlySelected.Next.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
        }
        
        private void OnEnable()
        {
            _weaponPlaceholderImages = GameObject.FindWithTag("UI:WeaponWheel:ImagePlaceholders")
                .GetComponentsInChildren<Image>();
         
            // set initial UI images.
            UpdateWeaponWheelImages();    
            
            // Register weapon changed event.
            EventManager.Bind<PlayerWeaponChangedEvent>(this);
        }

        public void OnEvent(PlayerWeaponChangedEvent invokedEvent)
            => UpdateWeaponWheelImages();
    }
}
