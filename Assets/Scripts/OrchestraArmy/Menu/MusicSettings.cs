using System;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using OrchestraArmy.SaveData;


namespace OrchestraArmy.Menu
{
    public class MusicSettings : MonoBehaviour
    {
        private SettingsData data = new SettingsData();
        
        public GameObject sliderSound;
        public GameObject sliderGMusic;
        public GameObject sliderMMusic;
        public GameObject sliderBeats;

        private float savedSound = 1.0f;
        private float savedGMusic = 1.0f;
        private float savedMMusic = 1.0f;
        private float savedBeats = 1.0f;
        
        /// <summary>
        /// load the saved data;
        /// </summary>
        private void OnEnable()
        {
            data = DataSaver.loadData<SettingsData>("settingsData");
            
            sliderSound.GetComponent<Slider>().value = data.sound;
            sliderGMusic.GetComponent<Slider>().value = data.gMusic;
            sliderMMusic.GetComponent<Slider>().value = data.mMusic;
            sliderBeats.GetComponent<Slider>().value = data.beats;
        }


        public void SetSound()
        {
            float sliderValue = sliderSound.GetComponent<Slider>().value;
        }

        public void SetGMusic()
        {
            float sliderValue = sliderGMusic.GetComponent<Slider>().value;
        }
        
        public void SetMMusic()
        {
            float sliderValue = sliderMMusic.GetComponent<Slider>().value;
        }
        
        public void SetBeats()
        {
            float sliderValue = sliderBeats.GetComponent<Slider>().value;
        }

        public void SaveSettings()
        {
            data = DataSaver.loadData<SettingsData>("settingsData");
            
            savedSound = sliderSound.GetComponent<Slider>().value;
            savedGMusic = sliderGMusic.GetComponent<Slider>().value;
            savedMMusic = sliderMMusic.GetComponent<Slider>().value;
            savedBeats = sliderBeats.GetComponent<Slider>().value;

            data.beats = savedBeats;
            data.sound = savedSound;
            data.gMusic = savedGMusic;
            data.mMusic = savedMMusic;
            
            DataSaver.saveData(data, "settingsData");
        }
        
        /// <summary>
        /// reset all volumes to max
        /// </summary>
        public void Undo()
        {
            data = DataSaver.loadData<SettingsData>("settingsData");
            
            savedSound = 1.0f;
            savedGMusic = 1.0f;
            savedMMusic = 1.0f;
            savedBeats = 1.0f;
            
            sliderSound.GetComponent<Slider>().value = 1.0f;
            sliderGMusic.GetComponent<Slider>().value = 1.0f;
            sliderMMusic.GetComponent<Slider>().value = 1.0f;
            sliderBeats.GetComponent<Slider>().value = 1.0f;
            
            data.beats = savedBeats;
            data.sound = savedSound;
            data.gMusic = savedGMusic;
            data.mMusic = savedMMusic;
            DataSaver.saveData(data, "settingsData");
        } 
    }
}