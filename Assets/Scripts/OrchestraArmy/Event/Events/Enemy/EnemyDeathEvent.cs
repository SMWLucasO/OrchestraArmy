namespace OrchestraArmy.Event.Events.Enemy
{
    public class EnemyDeathEvent : IEvent
    {
        
        /// <summary>
        /// The enemy that just got killed.
        /// </summary>
        public Entity.Entities.Enemies.Enemy KilledEnemy { get; set; }
    }
}