using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class DrumBoss : Boss
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Guitar;

        public DrumBoss()
        {
            WeaponType = WeaponType.Drum;
        }
    }
}