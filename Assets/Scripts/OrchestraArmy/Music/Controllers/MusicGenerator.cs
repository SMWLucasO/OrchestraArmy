using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;
using OrchestraArmy.Music.Instruments;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Rhythm;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Event.Events.Level;
using OrchestraArmy.SaveData;

namespace OrchestraArmy.Music.Controllers
{
    public class MusicGenerator : MonoBehaviour, IListener<InitiatedCombatEvent>, 
    IListener<PlayerDeathEvent>, IListener<LeaveCombatEvent>, IListener<EnteredNewLevelEvent>
    {
        /// <summary>
        /// The BPM for the game.
        /// </summary>
        public int BPM
        {
            get => _BPM;
            set
            {
                _newBPM = value;
            }
        }

        /// <summary>
        /// Temporary store the changed BPM, will be used to change BPM at the first beat to prevent stuttering
        /// </summary>
        [SerializeField]
        private int _newBPM = 120;
        
        /// <summary>
        /// Value for BPM property
        /// </summary>
        [Range(1, 140)]
        private int _BPM = 120;

        /// <summary>
        /// The scale for the music harmony.
        /// </summary>
        public Scale Scale = Scale.NaturalMinor;

        /// <summary>
        /// The key for the music; the base tone.
        /// </summary>
        public Tone Key = Tone.A;

        /// <summary>
        /// Boolean determining whether we are currently in battle mode.
        /// </summary>
        private bool _inBattle = false;
        
        /// <summary>
        /// Boolean determining whether the death event is occuring at this point in time.
        /// </summary>
        private bool _deathEvent = false;

        /// <summary>
        /// Instruments that should play.
        /// </summary>
        public List<AudioSource> Instruments;

        /// <summary>
        /// Instruments that should play in battle.
        /// </summary>
        public List<AudioSource> InstrumentsBattleOnly;
        
        /// <summary>
        /// Instruments that should play when death occurs.
        /// </summary>
        public List<AudioSource> InstrumentsDeath;

        /// <summary>
        /// Instruments that should play on beat.
        /// </summary>
        public List<AudioSource> InstrumentsFixedOnBeat;

        /// <summary>
        /// Instruments that should play off beat.
        /// </summary>
        public List<AudioSource> InstrumentsFixedOffBeat;

        /// <summary>
        /// The current beat. (1,2,3,4)
        /// </summary>
        public static int CurrentBeat = 0;

        /// <summary>
        /// The volume for the instruments.
        /// </summary>
        private float _instrumentsVolume = .8f;

        /// <summary>
        /// The user given volume for the instruments.
        /// </summary>
        [Range(0,1)]
        private float _userInstrumentsVolume = 1f;

        /// <summary>
        /// The volume for the beat instruments.
        /// </summary>
        private float _beatVolume = .9f;

        /// <summary>
        /// The user given volume for the beat instruments.
        /// </summary>
        [Range(0,1)]
        private float _userBeatVolume = 1f;

        /// <summary>
        /// The RhythmController
        /// </summary>
        public RhythmController RhythmController;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            RhythmController = new RhythmController();
            BPM = 100;
            RhythmController.SetStopwatch();
            _instrumentsVolume = .8f;
            _beatVolume = .9f;
            _userInstrumentsVolume = 1f;
            _userBeatVolume = 1f;
            _inBattle = false;
            
            EventManager.Bind<InitiatedCombatEvent>(this);
            EventManager.Bind<LeaveCombatEvent>(this);
            EventManager.Bind<PlayerDeathEvent>(this);
            EventManager.Bind<EnteredNewLevelEvent>(this);
            
            StartCoroutine(BeatCheck());
            
            
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            EventManager.Unbind<InitiatedCombatEvent>(this);
            EventManager.Unbind<LeaveCombatEvent>(this);
            EventManager.Unbind<PlayerDeathEvent>(this);
            EventManager.Unbind<EnteredNewLevelEvent>(this);
        }

        /// <summary>
        /// Play a note for each instrument in the given beat list
        /// </summary>
        private void PlayBeatAudio(List<AudioSource> instruments)
        {
            foreach (AudioSource instrument in instruments)
            {
                if (instrument != null)
                {
                    instrument.pitch = GetPitch(Tone.C, instrument.GetComponent<InstrumentData>().BaseTone);
                    instrument.volume = instrument.GetComponent<InstrumentData>().SpecificVolume 
                    * _beatVolume * _userBeatVolume;
                    instrument.Play();
                }
            }
        }

        /// <summary>
        /// Play a note for each instrument in the given list
        /// </summary>
        private void PlayAudio(List<AudioSource> instruments, Interval interval)
        {
            foreach (AudioSource instrument in instruments)
            {
                if (instrument != null && instrument.GetComponent<InstrumentData>().Interval == interval)
                {
                    int random = (int)(Random.value * (100f/instrument.GetComponent<InstrumentData>().Chance));
                    if (random == 1 || instrument.GetComponent<InstrumentData>().Chance == 100)
                    {
                        instrument.pitch = GetPitch(GetRandomCompanyTone(), 
                        instrument.GetComponent<InstrumentData>().BaseTone);
                        instrument.volume = instrument.GetComponent<InstrumentData>().
                        SpecificVolume * _instrumentsVolume * _userInstrumentsVolume;
                        instrument.Play();
                    }
                }
            }
        }


        /// <summary>
        /// Play a note for each instrument in the instruments list, if the interval matches
        /// </summary>
        private void PlayAudio(Interval interval)
        {
            if (_deathEvent)
            {
                PlayAudio(InstrumentsDeath, interval);
            }
            else
            {
                PlayAudio(Instruments, interval);

                if (_inBattle)
                {
                    PlayAudio(InstrumentsBattleOnly, interval);
                }
            }
        }

        /// <summary>
        /// Calculate the needed pitch for the tone.
        /// </summary>
        /// <param name="tone"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private float GetPitch(Tone tone, Tone offset = Tone.C) 
            => Mathf.Pow(2, ((int)tone - (int)offset)/12f);

        /// <summary>
        /// Get a random tone that fits in the current scale.
        /// </summary>
        /// <returns></returns>
        private Tone GetRandomCompanyTone()
        {
            int[] intervals = new int[3];
            
            if(Scale == Scale.NaturalMinor)
            {
                // Interval 0, 3, 7
                intervals = new int[]{0,3,7};
            }
            else if (Scale == Scale.Major)
            {
                // Interval 0, 5, 9
                intervals = new int[]{0,5,9};
            }
            
            int random = intervals[(int)(Random.value*2.99)];
            
            // Make sure the end value is not larger than 11
            return (Tone)((int)(Key + random) % 12);
        }
        
        /// <summary>
        /// Check the beat and performs all actions that match this beat.
        /// </summary>
        /// <returns></returns>
        public IEnumerator BeatCheck()
        {
            SettingsData data = DataSaver.LoadData<SettingsData>("settingsData");
            while (true)
            {

                if (data != null)
                {
                    _userBeatVolume = data.Beats;
                    _userInstrumentsVolume = data.GMusic;
                }

                int score = RhythmController.GetRhythmScore(BPM);

                //change BPM at first beat if needed
                if (score < 1 && _newBPM != _BPM)
                {
                    RhythmController.ResetStopWatch();
                    _BPM = _newBPM;
                }
                            
                if (score >= 95 && CurrentBeat % 2 == 1 || score <= 5 && CurrentBeat % 2 == 0)
                {
                    CurrentBeat = CurrentBeat % 4 + 1;
                    
                    if (CurrentBeat % 2 == 1)
                    {
                        EventManager.Invoke(new OffBeatEvent());
                        PlayBeatAudio(InstrumentsFixedOffBeat);
                    }
                    else
                    {
                        PlayBeatAudio(InstrumentsFixedOnBeat);
                    }

                    switch (CurrentBeat)
                    {
                        case 1:
                            PlayAudio(Interval.Wholes);
                            PlayAudio(Interval.Halves);
                            PlayAudio(Interval.Quarters);
                            break;
                        case 2:
                            PlayAudio(Interval.Quarters);
                            break;
                        case 3:
                            PlayAudio(Interval.Halves);
                            PlayAudio(Interval.Quarters);
                            break;
                        case 4:
                            PlayAudio(Interval.Quarters);
                            break;
                        default:
                            PlayAudio(Interval.Quarters);
                            break;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// Event for when combat is started
        /// </summary>
        public void OnEvent(InitiatedCombatEvent invokedEvent)
        {
            _inBattle = true;
            BPM = 110;
            _instrumentsVolume = .9f;
            _beatVolume = 1f;
        }

        /// <summary>
        /// Event for when combat is left
        /// </summary>
        public void OnEvent(LeaveCombatEvent invokedEvent)
        {
            ResetMusic();
        }

        public void ResetMusic()
        {
            _inBattle = false;
            BPM = 100;
            _instrumentsVolume = .8f;
            _beatVolume = .9f;
        }

        /// <summary>
        /// Event for when the player dies
        /// </summary>
        public void OnEvent(PlayerDeathEvent invokedEvent)
        {
            _deathEvent = true;
            ResetMusic();
        }

        public void OnEvent(EnteredNewLevelEvent invokedEvent)
        {
            _deathEvent = false;
        }
    }
}
