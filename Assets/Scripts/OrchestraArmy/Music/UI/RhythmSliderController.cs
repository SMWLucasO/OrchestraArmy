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
        /// The controller for the rhythm
        /// </summary>
        private RhythmController _rhythmController;

        /// <summary>
        /// The UI slider
        /// </summary>
        public Slider _rhythmSlider;

        // Start is called before the first frame update
        void Start()
        {
            _rhythmController = new RhythmController();
            // set max value to 100
            _rhythmSlider.maxValue = 100; 
        }

        // Update is called once per frame
        void Update()
        {
            // update value of slider each frame
            _rhythmSlider.value = _rhythmController.RhythmScore(); 
        }
    }
}
