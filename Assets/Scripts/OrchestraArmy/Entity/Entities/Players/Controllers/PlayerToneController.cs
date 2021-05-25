using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerToneController: IToneController
    {
        /// <summary>
        /// LinkedList of tones for easy looping
        /// </summary>
        private DoublyLoopedLinkedList<Tone> _toneList = new DoublyLoopedLinkedList<Tone>(new []
        {
            Tone.C,
            Tone.D,
            Tone.E,
            Tone.F,
            Tone.G,
            Tone.A,
            Tone.B
        });
        
        /// <summary>
        /// Current LinkedList node
        /// </summary>
        private DoublyLinkedListNode<Tone> _current;
        
        /// <summary>
        /// The currently selected tone
        /// </summary>
        public Tone CurrentTone { get; private set; }
        
        /// <summary>
        /// Last change made by mousewheel, used to debounce
        /// </summary>
        private float _lastChanged = 0;

        private void Initialize()
        {
            _current = _toneList.Start;
            
            EventManager.Invoke(new ToneChangedEvent()
            {
                Tone = CurrentTone
            });
        }
        
        /// <summary>
        /// Set the current tone based on user input
        /// </summary>
        private void SetCurrent()
        {
            var scrollValue = Mouse.current.scroll.y.ReadValue();
            
            if (scrollValue != 0)
            {
                //debounce scrolling to prevent switching to fast
                if (Time.time - 0.2 < _lastChanged)
                {
                    return;
                }
                
                if (scrollValue > 0)
                    _current = _current.Next;
                else if (scrollValue < 0)
                    _current = _current.Previous;

                CurrentTone = _current.Data;
                _lastChanged = Time.time;
            }
            else
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                    CurrentTone = Tone.C;
                else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                    CurrentTone = Tone.D;
                else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                    CurrentTone = Tone.E;
                else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                    CurrentTone = Tone.F;
                else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                    CurrentTone = Tone.G;
                else if (Keyboard.current.digit6Key.wasPressedThisFrame)
                    CurrentTone = Tone.A;
                else if (Keyboard.current.digit7Key.wasPressedThisFrame)
                    CurrentTone = Tone.B;

                while (_current.Data != CurrentTone)
                    _current = _current.Next;
            }
        }

        /// <summary>
        /// Handle tone updates
        /// </summary>
        public void HandleTone()
        {
            if (_current == null)
            {
                Initialize();
            }

            var previousTone = CurrentTone;
            
            SetCurrent();

            if (CurrentTone != previousTone)
            {
                EventManager.Invoke(new ToneChangedEvent()
                {
                    Tone = CurrentTone
                });
            }
        }
    }
}