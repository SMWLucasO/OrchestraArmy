using System.Collections;
using NUnit.Framework;
using OrchestraArmy.Entity;
using Tests.PlayTests.Helpers;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class DirectionTest
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }
        
        [UnityTest]
        [TestCase(0, 100, EntityDirection.Top, ExpectedResult = (IEnumerator) null)]
        [TestCase(0, -100, EntityDirection.Down, ExpectedResult = (IEnumerator) null)]
        [TestCase(-100, 0, EntityDirection.Left, ExpectedResult = (IEnumerator) null)]
        [TestCase(100, 0, EntityDirection.Right, ExpectedResult = (IEnumerator) null)]
        public IEnumerator TestPlayerFacesMouseDirection(float x, float y, EntityDirection direction)
        {
            _game.SetMousePositionRelativeToPlayer(x, y);

            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(direction, _game.Player.DirectionController.CurrentDirection);
        }
        
        [TearDown]
        public void Teardown()
        {
            _game.Destroy();
        }
    }
}