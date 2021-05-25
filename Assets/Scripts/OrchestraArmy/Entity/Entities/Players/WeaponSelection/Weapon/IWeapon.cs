namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon
{
    /// <summary>
    /// The player's weapon.
    /// </summary>
    public interface IWeapon
    {
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
