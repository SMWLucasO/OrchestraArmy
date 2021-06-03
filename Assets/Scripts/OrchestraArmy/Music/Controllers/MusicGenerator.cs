using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;
using OrchestraArmy.Music.Instruments;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Rhythm;
using System.Threading;


namespace OrchestraArmy.Music.Controllers
{
    public class MusicGenerator : MonoBehaviour
    {
        [SerializeField]
        [Range(1,140)]
        public int BPM = 120;

        public Scale Scale = Scale.NaturalMinor;

        public Key Key = Key.A;

        public TimeSignature TimeSignature = TimeSignature.CommonTime;

        public List<AudioSource> InstrumentsBass;
        public List<AudioSource> InstrumentsLoop;

        public List<AudioSource> InstrumentsFixedOnBeat;

        public List<AudioSource> InstrumentsFixedOffBeat;

        /// <summary>
        /// The current beat. (1,2,3,4)
        /// </summary>
        public static int CurrentBeat = 0;



        [SerializeField]
        [Range(0,100)]
        public int MasterVolume = 50;

        private int _prevRhythmScore = 0;
        private bool _toOnBeat = false;
        

        public RhythmController RhythmController;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            RhythmController = new RhythmController();
        }

        // Update is called once per frame
        void Update()
        {

            // Check if instrument should be played
            int currentRhythmScore = RhythmController.GetRhythmScore(BPM);

            if(_toOnBeat)
            {
                if(currentRhythmScore < _prevRhythmScore)
                {
                    PlayAudio(InstrumentsFixedOffBeat);
                    _toOnBeat = false;
                }
                
            }
            else
            {
                if(currentRhythmScore > _prevRhythmScore)
                {
                    PlayBass();
                    PlayAudio(InstrumentsFixedOnBeat);
                    _toOnBeat = true;
                }
            }
            _prevRhythmScore = currentRhythmScore;
        }

        /// <summary>
        /// Play a note for each instrument in the list
        /// </summary>
        private void PlayAudio(List<AudioSource> instruments)
        {
            foreach(AudioSource instrument in instruments)
            {
                instrument.Play();
            }
        }

        /// <summary>
        /// Play a note for each instrument in the bass instruments list
        /// </summary>
        private void PlayBass()
        {
            
            foreach(AudioSource instrument in InstrumentsBass)
            {
                
                instrument.pitch = GetPitch(Tone.C);
                instrument.Play();
            }
            foreach (AudioSource instrument in InstrumentsLoop)
            {

                instrument.pitch = GetPitch(Tone.C, instrument.GetComponent<InstrumentData>().BaseTone);
                Debug.Log((int)instrument.GetComponent<InstrumentData>().BaseTone);
                instrument.loop = true;
                instrument.Play();
            }
        }

        private float GetPitch(Tone tone, Tone offset = Tone.C)
        {
            return Mathf.Pow(2, ((int)tone - (int)offset)/12f);
        }

        public IEnumerator BeatCheck()
        {
            // To prevent nullreference exception
            if(RhythmController == null)
            {
                OnEnable();
            }

            while (true)
            {
                int score = RhythmController.GetRhythmScore(BPM);
                            
                if (score >= 99 && CurrentBeat % 2 == 1 || score <= 1 && CurrentBeat % 2 == 0)
                {
                    CurrentBeat = CurrentBeat % 4 + 1;
                    
                    if (CurrentBeat % 2 == 1)
                        EventManager.Invoke(new OffBeatEvent());
                    else
                        EventManager.Invoke(new EvenBeatEvent());
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
