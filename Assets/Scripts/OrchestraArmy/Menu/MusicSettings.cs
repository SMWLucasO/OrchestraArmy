using System;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;


namespace OrchestraArmy.Menu
{
    public class MusicSettings : MonoBehaviour
    {
        public GameObject sliderSound;
        public GameObject sliderGMusic;
        public GameObject sliderMMusic;
        public GameObject sliderBeats;

        private float savedSound = 1.0f;
        private float savedGMusic = 1.0f;
        private float savedMMusic = 1.0f;
        private float savedBeats = 1.0f;


        public void SetSound()
        {
            float sliderValue = sliderSound.GetComponent<Slider>().value;
            //todo: set sound volume
        }

        public void SetGMusic()
        {
            float sliderValue = sliderGMusic.GetComponent<Slider>().value;
            //todo: set game music volume
        }
        
        public void SetMMusic()
        {
            float sliderValue = sliderMMusic.GetComponent<Slider>().value;
            //todo: set menu music volume
        }
        
        public void SetBeats()
        {
            float sliderValue = sliderBeats.GetComponent<Slider>().value;
            //todo: set beats volume
        }

        public void SaveSettings()
        {
            //todo: link to music volume
        }
        
        /// <summary>
        /// reset all volumes to max
        /// </summary>
        public void Undo()
        {
            savedSound = 1.0f;
            savedGMusic = 1.0f;
            savedMMusic = 1.0f;
            savedBeats = 1.0f;
            
            sliderSound.GetComponent<Slider>().value = 1.0f;
            sliderGMusic.GetComponent<Slider>().value = 1.0f;
            sliderMMusic.GetComponent<Slider>().value = 1.0f;
            sliderBeats.GetComponent<Slider>().value = 1.0f;
            
            //todo: link to music volume
        } 
    }
}