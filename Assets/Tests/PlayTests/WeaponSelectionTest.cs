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
        private Game _game;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            _game = new Game();
            yield return _game.TestSetup("SampleScene");
        }

        [UnityTest]
        [TestCase(WeaponType.Guitar, WeaponType.Drum, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Drum, WeaponType.Flute, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Flute, WeaponType.Sousaphone, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Sousaphone, WeaponType.Guitar, ExpectedResult = (IEnumerator) null)]
        public IEnumerator TestPlayerCanSwitchInstrumentsForwardWhenUnlocked(WeaponType before, WeaponType expectedAfter)
        {
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
        [TestCase(WeaponType.Guitar, WeaponType.Sousaphone, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Sousaphone, WeaponType.Flute, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Flute, WeaponType.Drum, ExpectedResult = (IEnumerator) null)]
        [TestCase(WeaponType.Drum, WeaponType.Guitar, ExpectedResult = (IEnumerator) null)]
        public IEnumerator TestPlayerCanSwitchInstrumentsBackwardWhenUnlocked(WeaponType before, WeaponType expectedAfter)
        {
            while (_game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType != before)
                _game.Player.WeaponWheel.CurrentlySelected = _game.Player.WeaponWheel.CurrentlySelected.Next;

            _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.Unlocked = true;
            _game.Player.WeaponWheel.CurrentlySelected.Previous.WeaponWheelPlaceholderData.Unlocked = true;

            yield return null;
            
            _game.Press(Keyboard.current.qKey);

            yield return null;

            _game.Release(Keyboard.current.qKey);

            WeaponType newlySelected = _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;
            
            Assert.AreEqual(expectedAfter, newlySelected);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerCannotSwitchInstrumentsWhenLocked()
        {
            WeaponType previous = _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;
            
            _game.Press(Keyboard.current.qKey);

            yield return null;

            _game.Release(Keyboard.current.qKey);

            Assert.AreEqual(
                previous,
                _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                );

            _game.Press(Keyboard.current.qKey);

            yield return null;

            _game.Release(Keyboard.current.qKey);
            
            Assert.AreEqual(
                previous,
                _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
            );
            
        }
        
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return _game.TestTearDown("SampleScene");
        }
    }
}