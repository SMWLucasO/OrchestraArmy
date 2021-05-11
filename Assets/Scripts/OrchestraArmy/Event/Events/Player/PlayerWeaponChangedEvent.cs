using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Event.Events.Player
{
    public class PlayerWeaponChangedEvent : IEvent
    {
        
        /// <summary>
        /// The weapon which the player had before switching.
        /// </summary>
        public WeaponType PreviousWeapon { get; set; }
        
        /// <summary>
        /// The weapon the player has just selected.
        /// </summary>
        public WeaponType NewlySelectedWeapon { get; set; }
    }
}