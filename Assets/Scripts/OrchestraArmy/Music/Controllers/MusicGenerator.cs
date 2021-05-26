using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;
using OrchestraArmy.Music.Instruments;

namespace OrchestraArmy.Music.Controllers
{
    public class MusicGenerator : MonoBehaviour
    {
        [SerializeField]
        [Range(1,140)]
        public int BPM = 120;

        public Scale Scale = Scale.Major;

        public Key Key = Key.C;

        public TimeSignature TimeSignature = TimeSignature.CommonTime;

        public bool[] OnBeats;

        public List<IInstrument> InstrumentsRandom;

        public List<IInstrument> InstrumentsFixedOnBeat;

        public List<IInstrument> InstrumentsFixedOffBeat;



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
            RhythmController.ChangeBPMImmediately(BPM);
        }

        // Update is called once per frame
        void Update()
        {
            // Keep BPM up to date
            //RhythmController.ChangeBPMImmediately(BPM);
        }
    }
}
