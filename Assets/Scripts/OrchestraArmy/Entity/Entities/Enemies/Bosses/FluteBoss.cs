using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class FluteBoss : Boss
    {
        
        public FluteBoss()
        {
            WeaponType = WeaponType.Flute;
        }
        
    }
}