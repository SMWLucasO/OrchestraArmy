using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon
{
    /// <summary>
    /// The player's weapon.
    /// </summary>
    public interface IWeapon
    {

        /// <summary>
        /// The type of this weapon.
        /// </summary>
        public WeaponType WeaponType { get; set; }
        
        /// <summary>
        /// The damage caused by the weapon upon attack.
        /// </summary>
        /// <returns></returns>
        public int GetTotalDamage();
        
        /// <summary>
        /// The stamina cost caused by the weapon's attack.
        /// </summary>
        /// <returns></returns>
        public int GetStaminaCost();
    }
}
