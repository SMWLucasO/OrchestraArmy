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
            yield return _game.TestSetup("SampleScene");
            
            _weaponWheelIconPlaceholders = GameObject.FindWithTag("UI:WeaponWheel:ImagePlaceholders")
                .GetComponentsInChildren<Image>();
        }

        [UnityTest]
        public IEnumerator TestPlayerPressBothQAndEWeaponSelectionDoesNothing()
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
        
    }
}