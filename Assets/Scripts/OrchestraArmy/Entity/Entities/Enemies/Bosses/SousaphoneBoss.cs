using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class SousaphoneBoss : Boss
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Flute;
        
        public SousaphoneBoss()
        {
            WeaponType = WeaponType.Sousaphone;
        }
        
    }
}