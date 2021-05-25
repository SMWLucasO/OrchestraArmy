using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons
{
    public class Flute : IWeapon
    {
        public WeaponType WeaponType { get; set; }
            = WeaponType.Flute;

        public int GetTotalDamage()
        {
            return 10;
        }

        public int GetStaminaCost()
        {
            return 10;
        }
    }
}