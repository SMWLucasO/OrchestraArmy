using System;
using System.Collections;
using NUnit.Framework;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using Tests.PlayTests.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayTests
{
    public class WeaponSelectionTest
    {
        
        private Image[] _weaponWheelIconPlaceholders;
        
        private Game _game;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            _game = new Game();
            yield return _game.TestSetup("GameScene");
            
            _weaponWheelIconPlaceholders = GameObject.FindWithTag("UI:WeaponWheel:ImagePlaceholders")
                .GetComponentsInChildren<Image>();
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

            AssertPlaceholderSprite();
            
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

            AssertPlaceholderSprite();

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
            ApplyToAllInstrumentPlaceholders(
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
            
            
            ApplyToAllInstrumentPlaceholders(
                (placeholder) =>
                {
                    if (placeholder.WeaponWheelPlaceholderData.WeaponType != WeaponType.Guitar)
                        placeholder.WeaponWheelPlaceholderData.Unlocked = false;
                });
        }
        
        [UnityTest]
        public IEnumerator TestPlayerWeaponSelectionBackwardFlow()
        {
            ApplyToAllInstrumentPlaceholders(
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
        public IEnumerator TestPlayerPressBothQAndEWeaponSelectionDoesNothing()
        {
            WeaponType previous = _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType;

            _game.PressAndRelease(Keyboard.current.qKey);
            _game.PressAndRelease(Keyboard.current.eKey);
            yield return null;

            Assert.AreEqual(
                previous,
                _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType
            );
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return _game.TestTearDown("GameScene");
        }

        // specific helpers

        /// <summary>
        /// Assertion method for whether the icons are correct for the currently selected instrument and its next/prev.
        /// </summary>
        private void AssertPlaceholderSprite()
        {
            Assert.AreEqual(
                _game.Player.WeaponWheel.CurrentlySelected.Previous.WeaponWheelPlaceholderData.WeaponPlaceholderIcon,
                _weaponWheelIconPlaceholders[0].sprite
            );

            Assert.AreEqual(
                _game.Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponPlaceholderIcon,
                _weaponWheelIconPlaceholders[1].sprite
            );

            Assert.AreEqual(
                _game.Player.WeaponWheel.CurrentlySelected.Next.WeaponWheelPlaceholderData.WeaponPlaceholderIcon,
                _weaponWheelIconPlaceholders[2].sprite
            );
        }
        
        /// <summary>
        /// Apply something to all instrument placeholders.
        /// </summary>
        /// <param name="appliableFunc"></param>
        private void ApplyToAllInstrumentPlaceholders(Action<WeaponWheelPlaceholder> appliableFunc)
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
        
        /// <summary>
        /// Lock all unlockable(everything except the guitar) instruments
        /// </summary>
        private void LockAllUnlockableInstruments()
            => ApplyToAllInstrumentPlaceholders(
                (placeholder) =>
                {
                    if (placeholder.WeaponWheelPlaceholderData.WeaponType != WeaponType.Guitar)
                        placeholder.WeaponWheelPlaceholderData.Unlocked = false;
                });
        
    }
}