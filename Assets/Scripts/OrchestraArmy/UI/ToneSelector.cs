using System.Collections.Generic;
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

        private Dictionary<Tone, float> _toneMap = new Dictionary<Tone, float>
        {
            [Tone.C] = 0,
            [Tone.D] = 10,
            [Tone.E] = 20,
            [Tone.F] = 30,
            [Tone.G] = 40,
            [Tone.A] = 50,
            [Tone.B] = 60
        };

        public void OnEvent(ToneChangedEvent invokedEvent)
        {
            var position = Note.transform.position;
            Note.transform.position = new Vector3(position.x, _toneMap[invokedEvent.Tone], position.y);;
        }
    }
}