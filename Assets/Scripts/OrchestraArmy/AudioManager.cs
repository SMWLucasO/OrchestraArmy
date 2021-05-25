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
    /// <summary>
    /// Struct for the editor to set Audio for instruments
    /// </summary>
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
    
    /// <summary>
    /// Class that plays instrument audio
    /// </summary>
    public class AudioManager : MonoBehaviour, IListener<PlayerAttackEvent>, IListener<PlayerFiredAttackEvent>
    {
        /// <summary>
        /// InstrumentAudio collection
        /// </summary>
        public InstrumentAudio[] AttackSounds;
        
        /// <summary>
        /// AudioSource to play audio
        /// </summary>
        private AudioSource _audioSource;

        public void OnEnable()
        {
            EventManager.Bind<PlayerAttackEvent>(this);
            EventManager.Bind<PlayerFiredAttackEvent>(this);
            _audioSource = GetComponent<AudioSource>();
        }
        
        /// <summary>
        /// Event to handle player attack sounds
        /// </summary>
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

        public void OnEvent(PlayerFiredAttackEvent invokedEvent)
        {
            var instrumentAudio = AttackSounds.FirstOrDefault(s => s.Instrument == WeaponType.Drum);
            _audioSource.clip = instrumentAudio.C;
            _audioSource.Play();
        }
    }
}