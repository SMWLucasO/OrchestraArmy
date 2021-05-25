using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Sprites;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    [Serializable]
    public struct SpriteEntry
    {
        public EntityDirection Direction;
        public Sprite[] Sprites;
    }
    
    public abstract class DirectionalEntity : MonoBehaviour
    {
        /// <summary>
        /// The sprite per direction of the Entity. Array of structs instead of dictionary so that it can be serialized
        /// </summary>
        public SpriteEntry[] Sprites;

        /// <summary>
        /// The renderer for the 2D sprites of the directional entity.
        /// </summary>
        public SpriteRenderer Renderer { get; set; }
        
        /// <summary>
        /// The controller for the entity's direction.
        /// </summary>
        public IDirectionController DirectionController { get; set; }
        
        public SpriteManager SpriteManager { get; set; }
        
        protected void InitializeSprites()
        {
            Renderer = GetComponentInChildren<SpriteRenderer>();
            SpriteManager = new SpriteManager()
            {
                Entity = this
            };
            SpriteManager.StartAnimation();
        }
        
        protected virtual void Start() {}
        // Update is called once per frame
        protected virtual void Update() {}
        protected virtual void FixedUpdate() {}

        protected virtual void LateUpdate() {}
        
        protected  virtual void OnEnable() {}

        protected virtual void OnDisable() { }
        
        protected  virtual void Awake() {}

    }
}
