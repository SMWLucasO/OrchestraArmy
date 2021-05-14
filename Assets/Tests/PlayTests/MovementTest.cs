using System.Collections;
using NUnit.Framework;
using Tests.PlayTests.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class MovementTest
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }

        [UnityTest]
        public IEnumerator TestWKeyMovesPlayerInMouseDirection()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.SetMousePositionRelativeToPlayer(0, 100);
            _game.Input.Press(Keyboard.current.wKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.AreEqual(originalPosition.x, newPosition.x, 0.1);
            Assert.IsTrue(originalPosition.z < newPosition.z);
        }

        [UnityTest]
        public IEnumerator TestSKeyMovesPlayerInMouseDirection()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.SetMousePositionRelativeToPlayer(0, -100);
            _game.Input.Press(Keyboard.current.wKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.AreEqual(originalPosition.x, newPosition.x, 0.1);
            Assert.IsTrue(originalPosition.z > newPosition.z);
        }

        [UnityTest]
        public IEnumerator TestAKeyMovesPlayerInMouseDirection()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.SetMousePositionRelativeToPlayer(-100, 0);
            _game.Input.Press(Keyboard.current.wKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.IsTrue(originalPosition.x > newPosition.x);
            Assert.AreEqual(originalPosition.z, newPosition.z, 0.1);
        }

        [UnityTest]
        public IEnumerator TestDKeyMovesPlayerInMouseDirection()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.SetMousePositionRelativeToPlayer(100, 0);
            _game.Input.Press(Keyboard.current.wKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.IsTrue(originalPosition.x < newPosition.x);
            Assert.AreEqual(originalPosition.z, newPosition.z, 0.1);
        }

        [TearDown]
        public void Teardown()
        {
            _game.Destroy();
        }
    }
}
