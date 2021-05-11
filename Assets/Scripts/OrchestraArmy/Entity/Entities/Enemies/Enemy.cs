using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public class Enemy : LivingDirectionalEntity, IListener<EnemyDeathEvent>
    {
        public BehaviourStateMachine Behaviour { get; set; }

        protected override void Awake()
        {
            base.Awake();
            
            // register enemy events.
            EventManager.Bind(this);
        }

        /// <summary>
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public void OnEvent(EnemyDeathEvent enemyDeathEvent) 
            => Destroy(gameObject);
    }
}