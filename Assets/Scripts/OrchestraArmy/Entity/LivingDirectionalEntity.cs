using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Data;
using OrchestraArmy.Entity.Entities;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public abstract class LivingDirectionalEntity : MovingEntity
    {
        [field: SerializeField]
        public LivingEntityData EntityData { get; set; }
    }

}