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
        private DoublyLinkedList<Tone> _toneList = new DoublyLinkedList<Tone>(new []
        {
            Tone.C,
            Tone.D,
            Tone.E,
            Tone.F,
            Tone.G,
            Tone.A,
            Tone.B
        });

        private DoublyLinkedListNode<Tone> _current;
        public Tone CurrentTone { get => _current.Data; }
        private float _lastChanged = 0;

        private void Initialize()
        {
            _current = _toneList.Start;
            
            EventManager.Invoke(new ToneChangedEvent()
            {
                Tone = CurrentTone
            });
        }
        
        private void SetCurrent()
        {
            var scrollValue = Mouse.current.scroll.y.ReadValue();
            
            if (scrollValue != 0)
            {
                var previousTone = CurrentTone;

                if (scrollValue < 0)
                    _current = _current.Next;
                else if (scrollValue > 0)
                    _current = _current.Previous;
            }
            else
            {
                var keyboard = Keyboard.current;
                
                switch (true)
                {
                    case keyboard
                        break;
                    
                }
            }
        }
        
        public void HandleTone()
        {
            if (_current == null)
            {
                Initialize();
            }
            
            if (Time.time - 0.2 < _lastChanged)
            {
                return;
            }

            var scrollValue = Mouse.current.scroll.y.ReadValue();
            

            // if (CurrentTone != previousTone)
            // {
            //     EventManager.Invoke(new ToneChangedEvent()
            //     {
            //         Tone = CurrentTone
            //     });
            //
            //     _lastChanged = Time.time;
            // }
            //    
        }
    }
}