using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class SousaphoneBoss : Boss
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Flute;

        public override WeaponType WeaponType { get; set; }
            = WeaponType.Sousaphone;
    }
}