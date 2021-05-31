using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;

namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class FluteEnemy : Enemy
    {
        public override WeaponType HittableBy { get; set; }
            = WeaponType.Flute;

        public override WeaponType WeaponType { get; set; }
            = WeaponType.Flute;
    }
}