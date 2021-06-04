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
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            _game = new Game();
            yield return _game.TestSetup("SampleScene");
        }

        [UnityTest]
        public IEnumerator TestWKeyMovesPlayerInMouseDirection()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.Press(Keyboard.current.wKey);

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
            _game.Press(Keyboard.current.sKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.AreEqual(originalPosition.x, newPosition.x, 0.1);
            Assert.IsTrue(originalPosition.z > newPosition.z);
        }

        [UnityTest]
        public IEnumerator TestAKeyTurnsCameraLeft()
        {
            var original = _game.Camera.transform.forward;

            //get it's position
            _game.Press(Keyboard.current.aKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Camera.transform.forward;

            Assert.AreEqual(original.y, newPosition.y, 0.1);
            Assert.IsTrue(original.z > newPosition.z);
            Assert.IsTrue(original.x > newPosition.x);
        }

        [UnityTest]
        public IEnumerator TestDKeyTurnsCameraLeft()
        {
            var original = _game.Camera.transform.forward;

            //get it's position
            _game.Press(Keyboard.current.dKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Camera.transform.forward;

            Assert.AreEqual(original.y, newPosition.y, 0.1);
            Assert.IsTrue(original.z > newPosition.z);
            Assert.IsTrue(original.x < newPosition.x);
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return _game.TestTearDown("SampleScene");
        }
    }
}
