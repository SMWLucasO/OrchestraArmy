using System.Collections;
using System.Linq;
using UnityEngine;

namespace OrchestraArmy.Entity
{
    public class SpriteManager
    {
        public Camera Camera = Camera.main;
        public DirectionalEntity Entity;
        
        private Sprite[] _currentSpriteSet;
        private int _currentSprite = 0;
        private Coroutine animation = null;
        public Sprite[] CurrentSpriteSet
        {
            get => _currentSpriteSet;
            set
            {
                if (value != _currentSpriteSet)
                {
                    if (animation != null)
                        Entity.StopCoroutine(animation);
                    _currentSpriteSet = value;
                    _currentSprite = 0;
                    animation = Entity.StartCoroutine(AnimateSprite());
                }
            }
        }

        public void UpdateSprite()
        {
            var direction = Entity.DirectionController.CurrentDirection;
            var cameraRotation = Camera.transform.rotation.eulerAngles;

            if ((direction == EntityDirection.Top || direction == EntityDirection.Down) && (cameraRotation.y > 90 && cameraRotation.y < 270))
            {
                var newDirection = (int) direction - 2;

                if (newDirection < 0)
                    newDirection += 4;

                direction = (EntityDirection) newDirection;
            }
            
            var newSprite = Entity.Sprites.FirstOrDefault(s => s.Direction == direction);

            if (newSprite.Sprites == null || newSprite.Sprites.Length == 0)
                return; 

            CurrentSpriteSet = newSprite.Sprites;
        }
        
        
        public void StartAnimation()
        {
            animation = Entity.StartCoroutine(AnimateSprite());
        }
        
        
        // If the sprite entry has multiple sprites, animate it
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