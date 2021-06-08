using OrchestraArmy.Entity.Entities.Enemies.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class TheConductor : Boss
    {
        public override WeaponType HittableBy { get; set; }
            = WeaponType.Sousaphone;

        public override WeaponType WeaponType { get; set; }
            = WeaponType.Sousaphone;


        protected override void OnEnable()
        {
            base.OnEnable();

            Behaviour.CurrentState.StateData.ProjectileCount = 3;
            Behaviour.CurrentState.StateData.ProjectileType = typeof(FinalBossNote);
            Behaviour.CurrentState.StateData.AttackController = new ConductorAttackController();
        }
        
        public override void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            if (enemyDeathEvent.KilledEnemy.GetInstanceID() != GetInstanceID()) return;
            
            EventManager.Invoke(new FinalBossDeathEvent());
            Destroy(gameObject);
        }
        
    }
}