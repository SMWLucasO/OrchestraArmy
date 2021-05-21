using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Event.Events.Player
{
    public class PlayerAttackEvent: IEvent
    {
        public WeaponType Instrument;
        public Tone Tone;
    }
}