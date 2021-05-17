using System;
using System.Collections;
using NUnit.Framework;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
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
        public IEnumerator TestPlayerCanSwitchInstrumentsForwardWhenUnlocked(WeaponType before,
            WeaponType expectedAfter)
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
        public IEnumerator TestPlayerCanSwitchInstrumentsBackwardWhenUnlocked(WeaponType before,
            WeaponType expectedAfter)
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
            // due to testing, all weapons might be unlocked.
            LockAllUnlockableInstruments();
            
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

        [UnityTest]
        public IEnumerator TestPlayerWeaponSelectionForwardFlow()
        {
            ApplyToAllInstruments(
                (placeholder) => placeholder.WeaponWheelPlaceholderData.Unlocked = true
            );

            WeaponType[] weaponTypes = new[]
            {
                WeaponType.Guitar,
                WeaponType.Drum,
                WeaponType.Flute,
                WeaponType.Sousaphone
            };

         
            foreach (WeaponType weaponType in weaponTypes)
            {
                _game.Press(Keyboard.current.eKey);
                Assert.AreEqual(
                    weaponType,
                    _game.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                );
                yield return null;
                _game.Release(Keyboard.current.eKey);
            }
            
            
            ApplyToAllInstruments(
                (placeholder) =>
                {
                    if (placeholder.WeaponWheelPlaceholderData.WeaponType != WeaponType.Guitar)
                        placeholder.WeaponWheelPlaceholderData.Unlocked = false;
                });
        }
        
        [UnityTest]
        public IEnumerator TestPlayerWeaponSelectionBackwardFlow()
        {
            ApplyToAllInstruments(
                (placeholder) => placeholder.WeaponWheelPlaceholderData.Unlocked = true
            );

            WeaponType[] weaponTypes = new[]
            {
                WeaponType.Guitar,
                WeaponType.Sousaphone,
                WeaponType.Flute,
                WeaponType.Drum,
                WeaponType.Guitar
            };

         
            foreach (WeaponType weaponType in weaponTypes)
            {
                _game.Press(Keyboard.current.qKey);
                Assert.AreEqual(
                    weaponType,
                    _game.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
                );
                yield return null;
                _game.Release(Keyboard.current.qKey);
            }

            LockAllUnlockableInstruments();

        }

        [UnityTest]
        public IEnumerator TestPlayerPressBothQAndEDoesNothing()
        {
            WeaponType previous = _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;

            _game.Press(Keyboard.current.qKey);
            _game.Press(Keyboard.current.eKey);

            yield return null;

            _game.Release(Keyboard.current.qKey);
            _game.Release(Keyboard.current.eKey);

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

        // specific helpers

        private void ApplyToAllInstruments(Action<WeaponWheelPlaceholder> appliableFunc)
        {
            WeaponWheelPlaceholder placeholder = _game.Player.WeaponWheel.CurrentlySelected;
            bool first = true;
            while (placeholder.WeaponWheelPlaceholderData.WeaponType != WeaponType.Guitar || first)
            {
                appliableFunc(placeholder);
                placeholder = placeholder.Next;
                
                if (placeholder.WeaponWheelPlaceholderData.WeaponType == WeaponType.Guitar)
                    first = false;
            }
        }
        
        private void LockAllUnlockableInstruments()
            => ApplyToAllInstruments(
                (placeholder) =>
                {
                    if (placeholder.WeaponWheelPlaceholderData.WeaponType != WeaponType.Guitar)
                        placeholder.WeaponWheelPlaceholderData.Unlocked = false;
                });
        
    }
}