using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;

namespace OrchestraArmy.Event.Events.Player
{
    public class PlayerAttackHitEvent : IEvent
    {
        public Entity.Entities.Players.Player Attacker { get; set; }
        public int TargetId { get; set; }
        public IWeapon Weapon { get; set; }
    }
}