using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;

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

        [SerializeField]
        [Range(0,100)]
        public int MasterVolume = 50;

        private RhythmController _rhythmController;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            _rhythmController = new RhythmController();
            _rhythmController.ChangeBPMImmediately(BPM);
        }

        // Update is called once per frame
        void Update()
        {
            // Keep BPM up to date
            _rhythmController.ChangeBPMImmediately(BPM);
        }
    }
}
