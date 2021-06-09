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
        private SettingsData _data = new SettingsData();
        
        public GameObject SliderSound;
        public GameObject SliderGMusic;
        public GameObject SliderMMusic;
        public GameObject SliderBeats;

        private float _savedSound = 1.0f;
        private float _savedGMusic = 1.0f;
        private float _savedMMusic = 1.0f;
        private float _savedBeats = 1.0f;
        
        /// <summary>
        /// load the saved data;
        /// </summary>
        private void OnEnable()
        {
            _data = DataSaver.LoadData<SettingsData>("settingsData");
            
            SliderSound.GetComponent<Slider>().value = _data.Sound;
            SliderGMusic.GetComponent<Slider>().value = _data.GMusic;
            SliderMMusic.GetComponent<Slider>().value = _data.MMusic;
            SliderBeats.GetComponent<Slider>().value = _data.Beats;
        }

        /// <summary>
        /// save all slider values to _saved
        /// and write data to settings save file
        /// </summary>
        public void SaveSettings()
        {
            _data = DataSaver.LoadData<SettingsData>("settingsData");
            
            _savedSound = SliderSound.GetComponent<Slider>().value;
            _savedGMusic = SliderGMusic.GetComponent<Slider>().value;
            _savedMMusic = SliderMMusic.GetComponent<Slider>().value;
            _savedBeats = SliderBeats.GetComponent<Slider>().value;

            _data.Beats = _savedBeats;
            _data.Sound = _savedSound;
            _data.GMusic = _savedGMusic;
            _data.MMusic = _savedMMusic;
            
            DataSaver.SaveData(_data, "settingsData");
        }
        
        /// <summary>
        /// reset all volumes to max
        /// and write data to settings save file
        /// </summary>
        public void Undo()
        {
            _data = DataSaver.LoadData<SettingsData>("settingsData");
            
            _savedSound = 1.0f;
            _savedGMusic = 1.0f;
            _savedMMusic = 1.0f;
            _savedBeats = 1.0f;
            
            SliderSound.GetComponent<Slider>().value = 1.0f;
            SliderGMusic.GetComponent<Slider>().value = 1.0f;
            SliderMMusic.GetComponent<Slider>().value = 1.0f;
            SliderBeats.GetComponent<Slider>().value = 1.0f;
            
            _data.Beats = _savedBeats;
            _data.Sound = _savedSound;
            _data.GMusic = _savedGMusic;
            _data.MMusic = _savedMMusic;
            DataSaver.SaveData(_data, "settingsData");
        } 
    }
}