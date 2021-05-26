using System;
using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Rhythm;
using OrchestraArmy.Utils;
using UnityEngine;

namespace OrchestraArmy
{
    public class CombatManager: MonoBehaviour, IListener<BeatEvent>, IListener<CombatInitiatedEvent>, IListener<LeaveCombatEvent>
    {
        private HashSet<int> _registeredEnemies = new HashSet<int>();
        private DoublyLoopedLinkedList<Enemy> _enemies = new DoublyLoopedLinkedList<Enemy>();
        public void OnEnable()
        {
            EventManager.Bind<BeatEvent>(this);
        }

        public void OnEvent(BeatEvent invokedEvent)
        {
            throw new System.NotImplementedException();
        }

        public void OnEvent(CombatInitiatedEvent invokedEvent)
        {
            if (_registeredEnemies.Contains(invokedEvent.Entity.GetInstanceID()))
            {
                _registeredEnemies.Add(invokedEvent.Entity.GetInstanceID());
                _enemies.AddToEnd(invokedEvent.Entity);
            }
        }

        public void OnEvent(LeaveCombatEvent invokedEvent)
        {
            if (_registeredEnemies.Contains(invokedEvent.Entity.GetInstanceID()))
            {
                _registeredEnemies.Remove(invokedEvent.Entity.GetInstanceID());
                _enemies.Remove(invokedEvent.Entity);
            }
        }

        public void OnDisable()
        {
            EventManager.Unbind<BeatEvent>(this);
        }
    }
}