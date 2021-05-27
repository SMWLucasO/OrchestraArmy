using System;
using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Level;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Event.Events.Rhythm;
using OrchestraArmy.Utils;
using UnityEngine;

namespace OrchestraArmy
{
    public class CombatManager: IListener<OffBeatEvent>, IListener<CombatInitiatedEvent>, IListener<LeaveCombatEvent>, IListener<PlayerDeathEvent>
    {
        private DoublyLoopedLinkedList<int> _enemies = new DoublyLoopedLinkedList<int>();
        private DoublyLinkedListNode<int> _current;
        private static CombatManager _instance;
        
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            _instance = new CombatManager();
        }

        private CombatManager()
        {
            EventManager.Bind<OffBeatEvent>(this);
            EventManager.Bind<CombatInitiatedEvent>(this);
            EventManager.Bind<LeaveCombatEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        /// <summary>
        /// On an offbeat (1, 3), fire the EnemyTurn event
        /// </summary>
        public void OnEvent(OffBeatEvent invokedEvent)
        {
            if (_current == null && _enemies.Start == null)
                return;

            if (_current == null)
                _current = _enemies.Start;

            EventManager.Invoke(new EnemyTurnEvent() {EnemyId = _current.Data});
            
            _current = _current.Next;
        }

        /// <summary>
        /// Register an enemy when he enters combat mode
        /// </summary>
        public void OnEvent(CombatInitiatedEvent invokedEvent)
        {
            if (_enemies.Contains(invokedEvent.EntityId)) return;
            
            Debug.Log($"Added {invokedEvent.EntityId}");
            _enemies.AddToEnd(invokedEvent.EntityId);

            _current = _current == null ? _enemies.Start : _enemies.Get(_current.Data);
        }
        
        /// <summary>
        /// Unregister the enemy when he leaves combat mode
        /// </summary>
        public void OnEvent(LeaveCombatEvent invokedEvent)
        {
            if (_enemies.Contains(invokedEvent.EntityId))
            {
                if (_current != null && invokedEvent.EntityId == _current.Data)
                    _current = _current.Next;
                
                _enemies.Remove(invokedEvent.EntityId);

                _current = _current == null ? _enemies.Start : _enemies.Get(_current.Data);
            }
        }

        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            _current = null;
            _enemies = new DoublyLoopedLinkedList<int>();
        }
    }
}