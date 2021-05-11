using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons
{
    
    /// <summary>
    /// The guitar weapon.
    /// </summary>
    public class Guitar : IWeapon
    {
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