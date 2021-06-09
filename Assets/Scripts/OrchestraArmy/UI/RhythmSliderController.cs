using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Music.Controllers;

namespace OrchestraArmy.UI
{
    public class RhythmSliderController : MonoBehaviour
    {
        /// <summary>
        /// The controller for the music
        /// </summary>
        public MusicGenerator MusicGenerator;

        /// <summary>
        /// The UI slider
        /// </summary>
        public Slider RhythmSlider;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            // set max value to 100
            RhythmSlider.maxValue = 100;
        }

        // Update is called once per frame
        void Update()
        {
            // update value of slider each frame
            RhythmSlider.value = MusicGenerator.RhythmController.GetRhythmScore(MusicGenerator.BPM);
        }

        public RhythmController GetRhythmController()
        {
            return MusicGenerator.RhythmController;
        }
    }
}
