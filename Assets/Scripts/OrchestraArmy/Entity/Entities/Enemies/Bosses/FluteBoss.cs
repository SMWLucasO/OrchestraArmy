using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class FluteBoss : Boss
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Drum;
        
        public FluteBoss()
        {
            WeaponType = WeaponType.Flute;
        }
        
    }
}