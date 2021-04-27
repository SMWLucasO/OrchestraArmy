using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrchestraArmy.Entity.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    [Serializable]
    public struct SpriteEntry
    {
        public EntityDirection Direction;
        public Sprite[] Sprites;
    }
    
    public class DirectionalEntity : MonoBehaviour
    {
        /// <summary>
        /// The sprite per direction of the Entity. Array of structs instead of dictionary so that it can be serialized
        /// </summary>
        public SpriteEntry[] Sprites;

        private Sprite[] _currentSpriteSet;

        public GameObject sphere;
        public Sprite[] CurrentSpriteSet
        {
            get => _currentSpriteSet;
            set
            {
                if (value != _currentSpriteSet)
                {
                    StopCoroutine(nameof(AnimatePlayer));
                    _currentSpriteSet = value;
                    _currentSprite = 0;
                    StartCoroutine(nameof(AnimatePlayer));
                }
            }
        }

        /// <summary>
        /// The renderer for the 2D sprites of the directional entity.
        /// </summary>
        protected SpriteRenderer Renderer { get; set; }
        
        /// <summary>
        /// The controller for the entity's direction.
        /// </summary>
        public IDirectionController DirectionController { get; set; }
        
        private int _currentSprite = 0;
        
        // Start is called before the first frame update
        protected void InitializeSprites()
        {
            Renderer = GetComponent<SpriteRenderer>();
            StartCoroutine(nameof(AnimatePlayer));
        }
        
        // If the sprite entry has multiple sprites, animate it
        IEnumerator AnimatePlayer()
        {
            while (true)
            {
                if (CurrentSpriteSet == null || CurrentSpriteSet.Length <= 1)
                {
                    yield return null;
                    continue;
                }

                if (_currentSprite >= CurrentSpriteSet.Length)
                {
                    _currentSprite = 0;
                }
                
                Renderer.sprite =  CurrentSpriteSet[_currentSprite++];
                
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
