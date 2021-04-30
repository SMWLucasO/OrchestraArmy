using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Data;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public abstract class LivingDirectionalEntity : DirectionalEntity
    {
        
        public Rigidbody RigidBody { get; set; }
        
        [field: SerializeField]
        public LivingEntityData EntityData { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.RigidBody = GetComponent<Rigidbody>();
        }
    }
   
}