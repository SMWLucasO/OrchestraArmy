using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class GuitarEnemy : Enemy
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Guitar;
        
    }
}