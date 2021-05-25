using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons
{
    
    /// <summary>
    /// The guitar weapon.
    /// </summary>
    public class Guitar : IWeapon
    {
        public WeaponType WeaponType { get; set; }
            = WeaponType.Guitar;

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