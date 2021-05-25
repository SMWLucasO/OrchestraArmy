using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.PlayTests.Helpers
{
    public class Game
    {
        public InputTestFixture Input;
        public Camera Camera;
        public Player Player;
        public WeaponWheel WeaponWheel;

        private GameObject _ui;

        public Game()
        {
            //initialize input for testing
            Input = new InputTestFixture();
            
            //add input devices
            InputSystem.AddDevice<Keyboard>();
            InputSystem.AddDevice<Mouse>();
            
            //get Prefabs
            var cameraPrefab = Resources.Load("Prefabs/Main Camera");
            var uiPrefab = Resources.Load("Prefabs/UI/UI");
            var playerPrefab = Resources.Load("Prefabs/Entities/Player");

            _ui = (GameObject)Object.Instantiate(uiPrefab);

            Camera = ((GameObject)Object.Instantiate(cameraPrefab)).GetComponent<Camera>();
            WeaponWheel = GameObject.FindWithTag("UI:WeaponWheel").GetComponent<WeaponWheel>();
            Player = ((GameObject) Object.Instantiate(playerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity))
                .GetComponent<Player>();
        }

        public void SetMousePositionRelativeToPlayer(float xMod, float yMod)
        {
            var originalPosition = Player.transform.position;
            var mouseScreenPosition = Camera.WorldToScreenPoint(originalPosition);
            
            Input.Set(Mouse.current.position, new Vector2(mouseScreenPosition.x + xMod, mouseScreenPosition.y + yMod));
        }

        public void Destroy()
        {
            Object.Destroy(Camera.gameObject);
            Object.Destroy(Player.gameObject);
            Object.Destroy(_ui);
            
            InputSystem.RemoveDevice(Keyboard.current);
            InputSystem.RemoveDevice(Mouse.current);
        }
    }
}