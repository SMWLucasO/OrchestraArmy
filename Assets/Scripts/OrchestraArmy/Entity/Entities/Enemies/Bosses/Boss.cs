using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public abstract class Boss : Enemy
    {
        
        /// <summary>
        /// The WeaponType this boss is of.
        /// </summary>
        public WeaponType WeaponType { get; set; }
        
        public override void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            base.OnEvent(enemyDeathEvent);
            
            if (enemyDeathEvent.KilledEnemy.GetInstanceID() == GetInstanceID())
                EventManager.Invoke(new BossDeathEvent() { PositionOfDeath = transform.position, InstrumentToAward = WeaponType });
        }
        
    }
}