using System;
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
        public IEnumerator TestWKeyMovesPlayerForward()
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
        public IEnumerator TestSKeyMovesPlayerBackward()
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
        public IEnumerator TestAKeyMovesPlayerLeft()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.Press(Keyboard.current.aKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.IsTrue(originalPosition.x > newPosition.x);
            Assert.AreEqual(originalPosition.z, newPosition.z, 0.1);
        }
        
        [UnityTest]
        public IEnumerator TestDKeyMovesPlayerRight()
        {
            var originalPosition = _game.Player.transform.position;

            //get it's position
            _game.Press(Keyboard.current.dKey);

            yield return new WaitForSeconds(1f);

            //assert that the position no longer equals the original one
            var newPosition = _game.Player.transform.position;

            Assert.AreEqual(originalPosition.y, newPosition.y, 0.1);
            Assert.IsTrue(originalPosition.x < newPosition.x);
            Assert.AreEqual(originalPosition.z, newPosition.z, 0.1);
        }

        [UnityTest]
        public IEnumerator TestTurnCameraLeft()
        {
            var original = _game.Camera.transform.forward;
            
            yield return _game.ClickMoveReleaseRightMouse(1f, 1f);

            var newPosition = _game.Camera.transform.forward;

            Assert.AreEqual(original.y, newPosition.y, 0.1);
            Assert.IsTrue((original.z - newPosition.z) > 0.01);
            Assert.IsTrue((original.x - newPosition.x) < -0.01);
        }

        [UnityTest]
        public IEnumerator TestTurnCameraRight()
        {
            var original = _game.Camera.transform.forward;
            
            yield return _game.ClickMoveReleaseRightMouse(1f, -1f);

            var newPosition = _game.Camera.transform.forward;

            Assert.AreEqual(original.y, newPosition.y, 0.1);
            Assert.IsTrue((original.z - newPosition.z) > 0.01);
            Assert.IsTrue((original.x - newPosition.x) > 0.01);
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return _game.TestTearDown("SampleScene");
        }
    }
}
