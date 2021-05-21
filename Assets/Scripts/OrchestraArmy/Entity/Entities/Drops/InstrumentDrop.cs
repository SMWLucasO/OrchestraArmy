using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Pickup;
using OrchestraArmy.Room;
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

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            EventManager.Invoke(new InstrumentPickupEvent() {InstrumentPickedUp = WeaponType});
            
            RoomManager.Instance.CurrentRoom.RoomController.Objects.Remove(gameObject);
            Destroy(this);
        }
    }
}