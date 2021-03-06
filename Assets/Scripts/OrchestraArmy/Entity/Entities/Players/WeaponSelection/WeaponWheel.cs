using System;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection
{
    public class WeaponWheel : MonoBehaviour, IListener<PlayerDeathEvent>
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
        /// The weapon wheel's latest unlocked weapon's placeholder.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholder LatestUnlock { get; set; }
        
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
            
            // Switch weapons
            CurrentlySelected = CurrentlySelected.Previous;
            
            // Fire event for weapon switch.
            ExecuteWeaponSwitchedEvent(prev, newWeaponType);

            return true;
        }
        
        /// <summary>
        /// Lock the instrument received the most recent.
        /// </summary>
        public void LockLatestInstrument()
            => LatestUnlock.WeaponWheelPlaceholderData.Unlocked = false;
        
        
        /// <summary>
        /// Unlock the given instrument.
        /// </summary>
        /// <param name="weapon"></param>
        public void Unlock(WeaponType weapon)
        {
            WeaponWheelPlaceholder placeholder = CurrentlySelected;

            while (placeholder.WeaponWheelPlaceholderData.WeaponType != weapon)
                placeholder = placeholder.Next;

            // set unlocked.
            placeholder.WeaponWheelPlaceholderData.Unlocked = true;
            LatestUnlock = placeholder;

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
        {
            EventManager.Invoke(new PlayerWeaponChangedEvent() {
                PreviousWeapon = previousWeapon,
                NewlySelectedWeapon = newlySelectedWeapon
            });
            // Update the weapon wheel
            UpdateWeaponWheelImages();
        }

        /// <summary>
        /// Update the images on the weapon wheel UI.
        /// </summary>
        private void UpdateWeaponWheelImages()
        {
            // Set the sprite for the previous weapon at index 0
            _weaponPlaceholderImages[0].sprite =
                CurrentlySelected.Previous.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
            
            // Set the sprite for the current weapon at index 1
            _weaponPlaceholderImages[1].sprite =
                CurrentlySelected.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
            
            // Set the sprite for the next weapon at index 2
            _weaponPlaceholderImages[2].sprite =
                CurrentlySelected.Next.WeaponWheelPlaceholderData.WeaponPlaceholderIcon;
        }
        
        private void OnEnable()
        {
            _weaponPlaceholderImages = GameObject.FindWithTag("UI:WeaponWheel:ImagePlaceholders")
                .GetComponentsInChildren<Image>();
         
            // set initial UI images.
            UpdateWeaponWheelImages();    
            
            // Register player death event
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.Unbind<PlayerDeathEvent>(this);
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            // If our latest unlock is a guitar, it means we are on level 1.
            // Therefore, don't lock anything.
            if (LatestUnlock == null || 
                LatestUnlock.WeaponWheelPlaceholderData.WeaponType == WeaponType.Guitar)
                return;
            
            LockLatestInstrument();
            
            // If the player is currently holding the latest instrument, move it back by one.
            if (CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                == LatestUnlock.WeaponWheelPlaceholderData.WeaponType)
            {
                SwitchToPreviousWeapon();
            }
        }
    }
}
