using System;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Enemies
{

    public class Enemy : LivingDirectionalEntity, IListener<EnemyDeathEvent>
    {
        public BehaviourStateMachine Behaviour { get; set; }

        public float LastCollisionTime { get; set; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            LastCollisionTime = Time.time;
            
            // register enemy events.
            EventManager.Bind<EnemyDeathEvent>(this);
        }

        /// <summary>
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public void OnEvent(EnemyDeathEvent enemyDeathEvent) 
            => Destroy(gameObject);

        /// <summary>
        /// Temporary player attacking event.
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.TryGetComponent<Player>(out Player player))
                return;
            
            if (!((Time.time - LastCollisionTime) > 1))
                return;
            
            LastCollisionTime = Time.time;
            EventManager.Invoke(new PlayerDamageEvent() { HealthLost = 10 });
        }
    }
}