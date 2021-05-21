using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons
{
    public class Drum: IWeapon
    {
        public WeaponType WeaponType { get; set; }
            = WeaponType.Drum;

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