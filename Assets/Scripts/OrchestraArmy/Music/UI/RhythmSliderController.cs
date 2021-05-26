using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Music.Controllers;

namespace OrchestraArmy.Music.UI
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
        public Slider _rhythmSlider;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            // set max value to 100
            _rhythmSlider.maxValue = 100; 
        }

        // Update is called once per frame
        void Update()
        {
            // update value of slider each frame
            _rhythmSlider.value = MusicGenerator.RhythmController.GetRhythmScore();
        }
    }
}
