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
        private DoublyLinkedListNode<Enemy> _current;
        public void OnEnable()
        {
            EventManager.Bind<BeatEvent>(this);
            EventManager.Bind<CombatInitiatedEvent>(this);
            EventManager.Bind<LeaveCombatEvent>(this);
        }

        public void OnEvent(BeatEvent invokedEvent)
        {
            EventManager.Invoke(new EnemyTurnEvent() {EnemyId = _current.Data.GetInstanceID()});
            _current = _current.Next;
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
            if (invokedEvent.Entity == _current.Data)
                _current = _current.Next;
            
            if (_registeredEnemies.Contains(invokedEvent.Entity.GetInstanceID()))
            {
                _registeredEnemies.Remove(invokedEvent.Entity.GetInstanceID());
                _enemies.Remove(invokedEvent.Entity);
            }
        }

        public void OnDisable()
        {
            EventManager.Unbind<BeatEvent>(this);
            EventManager.Unbind<CombatInitiatedEvent>(this);
            EventManager.Unbind<LeaveCombatEvent>(this);
        }
    }
}