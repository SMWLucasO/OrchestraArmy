using System.Collections;
using NUnit.Framework;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using Tests.PlayTests.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class WeaponSelectionTest
    {
        private Game2 _game;
        
        [SetUp]
        public void Setup()
        {
            _game = new Game2();
            _game.Setup();
        }

        [UnityTest]
        [TestCase(WeaponType.Guitar, WeaponType.Drum, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Drum, WeaponType.Flute, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Flute, WeaponType.Sousaphone, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Sousaphone, WeaponType.Guitar, ExpectedResult = (IEnumerator) null)]
        public IEnumerator TestPlayerCanSwitchInstrumentsForwardWhenUnlocked(WeaponType before, WeaponType expectedAfter)
        {
            // load our scene.
            yield return _game.WaitForSetupTestDataForScene("SampleScene");

            while (_game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType != before)
                _game.Player.WeaponWheel.CurrentlySelected = _game.Player.WeaponWheel.CurrentlySelected.Next;

            _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.Unlocked = true;
            _game.Player.WeaponWheel.CurrentlySelected.Next.WeaponWheelPlaceholderData.Unlocked = true;

            yield return null;
            
            _game.Press(Keyboard.current.eKey);

            yield return null;

            _game.Release(Keyboard.current.eKey);

            WeaponType newlySelected = _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;
            
            Assert.AreEqual(expectedAfter, newlySelected);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerCanSwitchInstrumentsBackwardWhenUnlocked(WeaponType before, WeaponType expectedAfter)
        {
            yield return new WaitForSeconds(1);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerCannotSwitchInstrumentsWhenLocked()
        {
            yield return new WaitForSeconds(1);
        }
        


        [TearDown]
        public void Teardown()
        {
            _game.TearDown();
        }

    }
}