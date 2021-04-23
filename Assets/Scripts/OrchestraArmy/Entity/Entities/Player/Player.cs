using System;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Event;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player
{
    public class Player : LivingDirectionalEntity, IListener<PlayerDeathEvent>, IListener<PlayerDamageEvent>
    {
        
        public ICameraController CameraController;


        void Start()
        {
            //testing code.
            EventManager.Bind<PlayerDeathEvent>(this);
            EventManager.Bind<PlayerDamageEvent>(this);
            
            EventManager.Invoke(new PlayerDeathEvent());
            
            EventManager.Invoke(new PlayerDamageEvent
            {
                HealthLost = 10
            });
            
            EventManager.Unbind<PlayerDeathEvent>(this);
            EventManager.Unbind<PlayerDamageEvent>(this);
        }
        
        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            this.EntityData.Stamina += 1;
        }

        public void OnEvent(PlayerDamageEvent invokedEvent)
        {
            this.EntityData.Health -= invokedEvent.HealthLost;
        }
    }
}