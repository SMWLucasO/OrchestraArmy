using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public class DirectionalEntity : MonoBehaviour
    {
        /// <summary>
        /// The sprite per direction of the Entity.
        /// </summary>
        public Dictionary<EntityDirection, Sprite> Sprites;

        /// <summary>
        /// The renderer for the 2D sprites of the directional entity.
        /// </summary>
        public SpriteRenderer Renderer { get; set; }
        
        /// <summary>
        /// The controller for the entity's direction.
        /// </summary>
        public IDirectionController DirectionController { get; set; }
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
