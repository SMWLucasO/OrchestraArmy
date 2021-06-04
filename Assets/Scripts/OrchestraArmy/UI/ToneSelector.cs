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
    /// <summary>
    /// Note type.
    /// 
    /// Base = note that is striped (C4, A5, ect)
    /// Lower = line up
    /// Higher = line down
    /// </summary>
    enum NoteType
     {
         Base,
         Lower,
         Higher
     };
    
    /// <summary>
    /// Used to display the current tone on the UI
    /// </summary>
    public class ToneSelector : MonoBehaviour, IListener<ToneChangedEvent>
    {
        /// <summary>
        /// The Note object in the UI that will be moved up/down
        /// </summary>
        public GameObject Note;
        
        /// <summary>
        /// Base note texture
        /// </summary>
        public Texture BaseNoteTexture;
        
        /// <summary>
        /// Note texture
        /// </summary>
        public Texture NoteTexture;

        private float _currentHeight = 0;
        
        /// <summary>
        /// Note location and type for tone. (y coord, NoteType) 
        /// </summary>
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

        /// <summary>
        /// Event to handle tone change
        /// </summary>
        public void OnEvent(ToneChangedEvent invokedEvent)
        {
            var noteInfo = _toneMap[invokedEvent.Tone];
            
            if (noteInfo.Item2 == NoteType.Higher)
                Note.transform.rotation = Quaternion.Euler(180, 180, 0);
            else
                Note.transform.rotation = Quaternion.Euler(0, 0, 0);

            var position = Note.transform.localPosition;
            Note.transform.localPosition = new Vector3(position.x, position.y - _currentHeight + noteInfo.Item1, position.z);
            var texture = noteInfo.Item2 == NoteType.Base ? BaseNoteTexture : NoteTexture;
            Note.GetComponent<RawImage>().texture = texture;

            _currentHeight = noteInfo.Item1;
        }
    }
}