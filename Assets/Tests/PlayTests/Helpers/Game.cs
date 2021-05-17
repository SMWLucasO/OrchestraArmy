using System.Collections;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tests.PlayTests.Helpers
{
    public class Game: InputTestFixture
    {
        /// <summary>
        /// The player.
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The player's camera.
        /// </summary>
        public Camera Camera { get; set; }
        
        /// <summary>
        /// The player's weapon wheel.
        /// </summary>
        public WeaponWheel WeaponWheel { get; set; }
        
        public void SetMousePositionRelativeToPlayer(float xMod, float yMod)
        {
            var originalPosition = Player.transform.position;
            var mouseScreenPosition = Camera.WorldToScreenPoint(originalPosition);
            
            Set(Mouse.current.position, new Vector2(mouseScreenPosition.x + xMod, mouseScreenPosition.y + yMod));
        }
        
        public IEnumerator TestSetup(string scene)
        {
            Setup();
            InputSystem.AddDevice<Keyboard>();
            InputSystem.AddDevice<Mouse>();

            yield return LoadScene(scene);
        }
        
        public IEnumerator LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync(scene);
            
            Player = GameObject.FindWithTag("Player").GetComponent<Player>();
            WeaponWheel = Player.WeaponWheel;
            Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

            yield return null;
        }
        
        /// <summary>
        /// Destruction at the end.
        /// </summary>
        public IEnumerator TestTearDown(string scene)
        {
            TearDown();

            yield return SceneManager.UnloadSceneAsync(scene);
        }
    }
}