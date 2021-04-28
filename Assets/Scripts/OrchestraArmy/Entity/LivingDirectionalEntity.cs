using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Data;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public abstract class LivingDirectionalEntity : DirectionalEntity
    {
        [field: SerializeField]
        public IMovementController MovementController { get; set; }

        [field: SerializeField]
        public LivingEntityData EntityData { get; set; }
        
        protected override void Update()
        {
            base.Update();
            MovementController?.HandleMovement();
        }
        
    }
   
}