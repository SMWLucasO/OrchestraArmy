using System;
using System.ComponentModel;
using System.Linq;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy
{
    [Serializable]
    public struct InstrumentAudio
    {
        public WeaponType Instrument;
        public AudioClip C;
        public AudioClip D;
        public AudioClip E;
        public AudioClip F;
        public AudioClip G;
        public AudioClip A;
        public AudioClip B;
    }
    public class AudioManager: MonoBehaviour, IListener<PlayerAttackEvent>
    {
        public InstrumentAudio[] AttackSounds;
        private AudioSource _audioSource;

        public void OnEnable()
        {
            EventManager.Bind<PlayerAttackEvent>(this);
            _audioSource = GetComponent<AudioSource>();
        }

        public void OnEvent(PlayerAttackEvent invokedEvent)
        {
            var instrumentAudio = AttackSounds.FirstOrDefault(s => s.Instrument == invokedEvent.Instrument);

            if (instrumentAudio.C == null)
                return;

            _audioSource.clip = invokedEvent.Tone switch
            {
                Tone.C => instrumentAudio.C,
                Tone.D => instrumentAudio.D,
                Tone.E => instrumentAudio.E,
                Tone.F => instrumentAudio.F,
                Tone.G => instrumentAudio.G,
                Tone.A => instrumentAudio.A,
                Tone.B => instrumentAudio.B,
                _ => throw new InvalidEnumArgumentException()
            };
            
            _audioSource.Play();
        }
    }
}