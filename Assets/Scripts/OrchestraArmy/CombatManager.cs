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
    /// <summary>
    /// Keeps track of the current combat state
    /// </summary>
    public class CombatManager: IListener<OffBeatEvent>, IListener<EnemyCombatInitiatedEvent>, IListener<EnemyLeaveCombatEvent>, IListener<PlayerDeathEvent>
    {
        /// <summary>
        /// List of the current enemies that are registered
        /// </summary>
        private DoublyLoopedLinkedList<int> _enemies = new DoublyLoopedLinkedList<int>();
        /// <summary>
        /// The current enemy
        /// </summary>
        private DoublyLinkedListNode<int> _current;
        /// <summary>
        /// Variable to keep instance alive
        /// </summary>
        private static CombatManager _instance;
        
        /// <summary>
        /// Initializes the class with RuntimeInitializeOnLoadMethod. Done because it is not a gameobject or part of one
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            _instance = new CombatManager();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private CombatManager()
        {
            EventManager.Bind<OffBeatEvent>(this);
            EventManager.Bind<EnemyCombatInitiatedEvent>(this);
            EventManager.Bind<EnemyLeaveCombatEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
        }

        /// <summary>
        /// On an offbeat (1, 3), fire the EnemyTurn event
        /// </summary>
        public void OnEvent(OffBeatEvent invokedEvent)
        {
            //if there is no current enemy and the list is empty => return
            if (_current == null && _enemies.Start == null)
                return;

            //if there is no current enemy => assume start
            if (_current == null)
                _current = _enemies.Start;

            //fire the enemy turn event
            EventManager.Invoke(new EnemyTurnEvent() {EnemyId = _current.Data});
            
            //set current to the next
            _current = _current.Next;
        }

        /// <summary>
        /// Register an enemy when he enters combat mode
        /// </summary>
        public void OnEvent(EnemyCombatInitiatedEvent invokedEvent)
        {
            if (_enemies.Contains(invokedEvent.EntityId)) return;
            
            if (_enemies.Start == null)
                EventManager.Invoke(new InitiatedCombatEvent());
            
            _enemies.AddToEnd(invokedEvent.EntityId);

            //current is registered by value, so refetch. I miss pointers
            _current = _current == null ? _enemies.Start : _enemies.Get(_current.Data);
        }
        
        /// <summary>
        /// Unregister the enemy when he leaves combat mode
        /// </summary>
        public void OnEvent(EnemyLeaveCombatEvent invokedEvent)
        {
            if (_enemies.Contains(invokedEvent.EntityId))
            {
                //if the current is set and it is equal to what we delete => set to next
                if (_current != null && invokedEvent.EntityId == _current.Data)
                    _current = _current.Next;
                
                _enemies.Remove(invokedEvent.EntityId);
                
                
                if (_enemies.Start == null)
                    EventManager.Invoke(new LeaveCombatEvent());

                //current is registered by value, so refetch. I miss pointers
                _current = _current == null ? _enemies.Start : _enemies.Get(_current.Data);
            }
        }

        /// <summary>
        /// Reset on player death
        /// </summary>
        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            _current = null;
            _enemies = new DoublyLoopedLinkedList<int>();
        }
    }
}