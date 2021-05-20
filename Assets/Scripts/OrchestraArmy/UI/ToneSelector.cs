using System;
using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrchestraArmy.UI
{
    public class ToneSelector: MonoBehaviour, IListener<ToneChangedEvent>
    {
        public GameObject Note;
        public GameObject Background;
        public Texture[] Notes;

        private Dictionary<Tone, (float, int)> _toneMap = new Dictionary<Tone, (float, int)>
        {
            [Tone.C] = (-20, 0),
            [Tone.D] = (-14.2f, 1),
            [Tone.E] = (-8.4f, 1),
            [Tone.F] = (-2.6f, 1),
            [Tone.G] = (3.2f, 1),
            [Tone.A] = (9, 1),
            [Tone.B] = (-10f, 1) 
        };

        public void Start()
        {
            EventManager.Bind<ToneChangedEvent>(this);
        }

        public void OnEvent(ToneChangedEvent invokedEvent)
        {
            if (invokedEvent.Tone == Tone.B)
                Note.transform.rotation = Quaternion.Euler(180, 180, 0);
            else
                Note.transform.rotation = Quaternion.Euler(0, 0, 0);
            
            var position = Note.transform.position;
            var noteInfo = _toneMap[invokedEvent.Tone];
            
            Note.transform.position = new Vector3(position.x, this.transform.position.y + noteInfo.Item1, position.y);
            Note.GetComponent<RawImage>().texture = Notes[noteInfo.Item2];
        }
    }
}