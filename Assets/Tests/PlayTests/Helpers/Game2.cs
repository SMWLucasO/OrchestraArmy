using System.Collections;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tests.PlayTests.Helpers
{
    public class Game2: InputTestFixture
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
        
        public override void Setup()
        {
            base.Setup();
            InputSystem.AddDevice<Keyboard>();
            InputSystem.AddDevice<Mouse>();
        }

        /// <summary>
        /// Coroutine to load the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IEnumerator WaitForSceneLoad(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync("SampleScene", mode);
        }

        /// <summary>
        /// Coroutine to destroy the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static IEnumerator WaitForSceneDestruction(string scene)
        {
            yield return SceneManager.UnloadSceneAsync(scene);
        }

        /// <summary>
        /// coroutine for loading everything to do the test.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public IEnumerator WaitForSetupTestDataForScene(string scene)
        {
            yield return WaitForSceneLoad(scene);

            Player = GameObject.FindWithTag("Player").GetComponent<Player>();
            WeaponWheel = Player.WeaponWheel;
            Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        
        /// <summary>
        /// Destruction at the end.
        /// </summary>
        public override void TearDown()
        {
            base.TearDown();
            InputSystem.RemoveDevice(Keyboard.current);
            InputSystem.RemoveDevice(Mouse.current);
        }
    }
}