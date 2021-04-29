using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Sprites
{
    public class SpriteManager
    {
        /// <summary>
        /// The camera, used to determine if a sprite should be inverted
        /// </summary>
        public Camera Camera = Camera.main;
        
        /// <summary>
        /// The entity
        /// </summary>
        public DirectionalEntity Entity;
        
        /// <summary>
        /// The current sprite set. A sprite set is a collection of sprites that will be animated.
        /// </summary>
        private Sprite[] _currentSpriteSet;
        
        /// <summary>
        /// The current sprite within the animation
        /// </summary>
        private int _currentSprite = 0;
        
        /// <summary>
        /// The coroutine for the animation
        /// </summary>
        private Coroutine _animation = null;
        
        /// <summary>
        /// Property for _currentSpriteSet. Will restart the animation when replaced.
        /// </summary>
        public Sprite[] CurrentSpriteSet
        {
            get => _currentSpriteSet;
            set
            {
                if (value != _currentSpriteSet)
                {
                    if (_animation != null)
                        Entity.StopCoroutine(_animation);
                    _currentSpriteSet = value;
                    _currentSprite = 0;
                    _animation = Entity.StartCoroutine(AnimateSprite());
                }
            }
        }

        /// <summary>
        /// Updates the current spriteset if needed
        /// </summary>
        public void UpdateSprite()
        {
            var direction = Entity.DirectionController.CurrentDirection;
            var newSprite = Entity.Sprites.FirstOrDefault(s => s.Direction == direction);

            if (newSprite.Sprites == null || newSprite.Sprites.Length == 0)
                return; 

            CurrentSpriteSet = newSprite.Sprites;
        }
        
        /// <summary>
        /// Start the animation
        /// </summary>
        public void StartAnimation()
        {
            _animation = Entity.StartCoroutine(AnimateSprite());
        }
        
        /// <summary>
        /// Animate a sprite set if it contains more then 1 sprite. Executed as a Coroutine.
        /// </summary>
        IEnumerator AnimateSprite()
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
                
                Entity.Renderer.sprite = CurrentSpriteSet[_currentSprite++];
                
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}