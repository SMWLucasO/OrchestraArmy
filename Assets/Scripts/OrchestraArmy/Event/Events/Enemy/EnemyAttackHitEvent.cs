using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Event.Events.Enemy
{
    public class EnemyAttackHitEvent : IEvent
    {
        public Entity.Entities.Enemies.Enemy Attacker { get; set; }
        public Entity.Entities.Players.Player Target { get; set; }
        public IWeapon Weapon { get; set; }
        public Tone Tone { get; set; }
    }
}