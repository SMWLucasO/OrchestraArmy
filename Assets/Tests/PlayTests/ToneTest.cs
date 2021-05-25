using System.Collections;
using NUnit.Framework;
using OrchestraArmy.Entity;
using OrchestraArmy.Enum;
using Tests.PlayTests.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class ToneTest
    {
        private Game _game;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            _game = new Game();
            yield return _game.TestSetup("SampleScene");
        }

        [UnityTest]
        public IEnumerator TestDigitKeysMapToTones()
        {
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit1Key, Tone.C);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit2Key, Tone.D);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit3Key, Tone.E);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit4Key, Tone.F);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit5Key, Tone.G);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit6Key, Tone.A);
            yield return AssertToneIsSelectedAfterKeyPress(Keyboard.current.digit7Key, Tone.B);
        }

        [UnityTest]
        public IEnumerator TestScrollUpShouldChangeTone()
        {
            Assert.AreEqual(Tone.C, _game.Player.ToneController.CurrentTone);
            _game.Set(Mouse.current.scroll.y, 1);
            yield return null;
            Assert.AreEqual(Tone.D, _game.Player.ToneController.CurrentTone);
        }

        [UnityTest]
        public IEnumerator TestScrollDownShouldChangeTone()
        {
            Assert.AreEqual(Tone.C, _game.Player.ToneController.CurrentTone);
            _game.Set(Mouse.current.scroll.y, -1);
            yield return null;
            Assert.AreEqual(Tone.B, _game.Player.ToneController.CurrentTone);
        }

        private IEnumerator AssertToneIsSelectedAfterKeyPress(KeyControl key, Tone tone)
        {
            _game.Press(key);
            yield return null;
            _game.Release(key);

            Assert.AreEqual(tone, _game.Player.ToneController.CurrentTone);
        }
        
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return _game.TestTearDown("SampleScene");
        }
    }
}