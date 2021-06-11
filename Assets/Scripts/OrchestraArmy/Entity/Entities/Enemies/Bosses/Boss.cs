using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public abstract class Boss : Enemy
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            Behaviour.CurrentState.StateData.ProjectileType = typeof(BossNote);
        }
        
        public override void EnemyDeath(Enemy enemy)
        {
            if (enemy.GetInstanceID() != GetInstanceID()) return;
            
            EventManager.Invoke(new BossDeathEvent() { PositionOfDeath = transform.position, InstrumentToAward = WeaponType });
            Destroy(gameObject);
        }

        protected override void SetVariableHp()
        {
            return;
        }
    }
}