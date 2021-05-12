﻿using System;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Enemies
{
    public abstract class Enemy : LivingDirectionalEntity, IListener<EnemyDeathEvent>, IListener<PlayerAttackHitEvent>
    {
        public BehaviourStateMachine Behaviour { get; set; }

        public float LastCollisionTime { get; set; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            LastCollisionTime = Time.time;
            
            // register enemy events.
            EventManager.Bind<EnemyDeathEvent>(this);
            EventManager.Bind<PlayerAttackHitEvent>(this);
        }

        /// <summary>
        /// Event for when an enemy dies.
        /// </summary>
        /// <param name="enemyDeathEvent"></param>
        public void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            if (enemyDeathEvent.KilledEnemy.GetInstanceID() == GetInstanceID())
                Destroy(gameObject);
        }

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

        public void OnEvent(PlayerAttackHitEvent invokedEvent)
        {
            if (gameObject.GetInstanceID() != invokedEvent.TargetId)
                return;

            EntityData.Health -= invokedEvent.Weapon.GetTotalDamage();

            if (EntityData.Health <= 0)
            {
                EventManager.Invoke(new EnemyDeathEvent() {KilledEnemy = this});
            }
        }

        public void OnDestroy()
        {
            EventManager.Unbind<EnemyDeathEvent>(this);
            EventManager.Unbind<PlayerAttackHitEvent>(this);
        }
    }
}