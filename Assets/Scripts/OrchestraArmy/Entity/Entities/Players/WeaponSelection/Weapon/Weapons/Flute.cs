namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons
{
    public class Flute : IWeapon
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