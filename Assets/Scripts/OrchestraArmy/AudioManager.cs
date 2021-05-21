using System;
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

            switch (invokedEvent.Tone)
            {
                case Tone.C:
                    _audioSource.clip = instrumentAudio.C;
                    break;
                case Tone.D:
                    _audioSource.clip = instrumentAudio.D;
                    break;
                case Tone.E:
                    _audioSource.clip = instrumentAudio.E;
                    break;
                case Tone.F:
                    _audioSource.clip = instrumentAudio.F;
                    break;
                case Tone.G:
                    _audioSource.clip = instrumentAudio.G;
                    break;
                case Tone.A:
                    _audioSource.clip = instrumentAudio.A;
                    break;
                case Tone.B:
                    _audioSource.clip = instrumentAudio.B;
                    break;
            }
            
            _audioSource.Play();
        }
    }
}