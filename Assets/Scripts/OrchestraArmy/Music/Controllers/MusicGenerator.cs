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

        public Tone Key = Tone.A;

        public List<AudioSource> Instruments;

        public List<AudioSource> InstrumentsFixedOnBeat;

        public List<AudioSource> InstrumentsFixedOffBeat;

        /// <summary>
        /// The current beat. (1,2,3,4)
        /// </summary>
        public static int CurrentBeat = 0;



        [SerializeField]
        [Range(0,100)]
        public int MasterVolume = 50;


        public RhythmController RhythmController;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            RhythmController = new RhythmController();
            StartCoroutine(BeatCheck());
        }


        /// <summary>
        /// Play a note for each instrument in the given list
        /// </summary>
        private void PlayAudio(List<AudioSource> instruments)
        {
            foreach(AudioSource instrument in instruments)
            {
                if(instrument != null)
                {
                    instrument.pitch = GetPitch(Tone.C, instrument.GetComponent<InstrumentData>().BaseTone);
                    instrument.Play();
                }
                    
            }
        }

        /// <summary>
        /// Play a note for each instrument in the instruments list, if the interval matches
        /// </summary>
        private void PlayAudio(Interval interval)
        {
            foreach(AudioSource instrument in Instruments)
            {
                if(instrument != null && instrument.GetComponent<InstrumentData>().Interval == interval)
                {
                    int random = (int)(Random.value * (100f/instrument.GetComponent<InstrumentData>().Chance));
                    if(random == 1 || instrument.GetComponent<InstrumentData>().Chance == 100)
                    {
                        instrument.pitch = GetPitch(GetRandomCompanyTone(), instrument.GetComponent<InstrumentData>().BaseTone);
                        instrument.Play();
                    }
                    
                }
            }
        }

        private float GetPitch(Tone tone, Tone offset = Tone.C)
        {
            return Mathf.Pow(2, ((int)tone - (int)offset)/12f);
        }

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


        public IEnumerator BeatCheck()
        {
            while (true)
            {
                int score = RhythmController.GetRhythmScore(BPM);
                            
                if (score >= 99 && CurrentBeat % 2 == 1 || score <= 1 && CurrentBeat % 2 == 0)
                {
                    CurrentBeat = CurrentBeat % 4 + 1;
                    
                    if (CurrentBeat % 2 == 1)
                    {
                        EventManager.Invoke(new OffBeatEvent());
                        PlayAudio(InstrumentsFixedOffBeat);
                    }
                    else
                    {
                        EventManager.Invoke(new EvenBeatEvent());
                        PlayAudio(InstrumentsFixedOnBeat);
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
    }
}
