using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Event.Events.Pickup
{
    public class InstrumentPickupEvent : IEvent
    {
        
        /// <summary>
        /// The instrument which was picked up.
        /// </summary>
        public WeaponType InstrumentPickedUp { get; set; }
        
    }
}