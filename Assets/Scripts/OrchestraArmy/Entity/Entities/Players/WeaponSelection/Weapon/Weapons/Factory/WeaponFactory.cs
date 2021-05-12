namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory
{
    /// <summary>
    /// A simple factory for generating the weapons.
    /// </summary>
    public static class WeaponFactory
    {

        /// <summary>
        /// Simple method for creating a weapon.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static IWeapon Make(WeaponType weapon)
        {
            // NOTE: since there will only be a max of 4 weapons, this is fine. 
            // But otherwise, we will need to refactor it.
            // We are applying the principle 'Keep it simple, stupid' and 'You arent gonna need it' here.
            return weapon switch
            {
                WeaponType.Guitar => new Guitar(),
                WeaponType.Flute => new Flute(),
                WeaponType.Sousaphone => new Sousaphone(),
                WeaponType.Drum => new Drum(),
                _ => null
            };
        }
        
    }
}