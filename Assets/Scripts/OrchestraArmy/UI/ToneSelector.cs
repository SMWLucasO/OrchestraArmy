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
    enum NoteType
     {
         Base,
         Lower,
         Higher
     };
    public class ToneSelector: MonoBehaviour, IListener<ToneChangedEvent>
    {
        
        public GameObject Note;
        public Texture BaseNoteTexture;
        public Texture NoteTexture;

        private Dictionary<Tone, (float, NoteType)> _toneMap = new Dictionary<Tone, (float, NoteType)>
        {
            [Tone.C] = (-20, NoteType.Base),
            [Tone.D] = (-14.2f, NoteType.Lower),
            [Tone.E] = (-8.4f, NoteType.Lower),
            [Tone.F] = (-2.6f, NoteType.Lower),
            [Tone.G] = (3.2f, NoteType.Lower),
            [Tone.A] = (9, NoteType.Lower),
            [Tone.B] = (-10f, NoteType.Higher) 
        };

        public void OnEnable()
        {
            EventManager.Bind<ToneChangedEvent>(this);
        }

        public void OnEvent(ToneChangedEvent invokedEvent)
        {
            var noteInfo = _toneMap[invokedEvent.Tone];
            
            if (noteInfo.Item2 == NoteType.Higher)
                Note.transform.rotation = Quaternion.Euler(180, 180, 0);
            else
                Note.transform.rotation = Quaternion.Euler(0, 0, 0);

            Note.transform.localPosition = new Vector3(0, noteInfo.Item1, 0);
            var texture = noteInfo.Item2 == NoteType.Base ? BaseNoteTexture : NoteTexture;
            Note.GetComponent<RawImage>().texture = texture;
        }
    }
}