using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Utils;
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

        public PlayerToneController()
        {
            _current = _toneList.Start;
        }
        
        public void HandleTone()
        {
            var scrollValue = Mouse.current.scroll.y.ReadValue();
            var previousTone = CurrentTone;

            if (scrollValue > 0)
                _current = _current.Next;
            else if (scrollValue < 0)
                _current = _current.Previous;
            
            if (CurrentTone != previousTone)
                EventManager.Invoke(new ToneChangedEvent()
                {
                    Tone = CurrentTone
                });
        }
    }
}