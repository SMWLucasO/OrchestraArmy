using System;
using OrchestraArmy.SaveData;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.UI;


namespace OrchestraArmy.Menu
{
    public class GameSettings : MonoBehaviour
    {
        private SettingsData Data = new SettingsData();
        
        public Texture2D[] CursorSprite;
        public GameObject[] ShowCursor;
        public GameObject[] ShowDifficulty;
        public GameObject SliderDifficulty;
        public GameObject SliderMouse;
        
        private int _savedDifficulty = 1;
        private int _savedCursor = 0;
        private int _tempDifficulty = 1;
        private int _tempCursor = 0;

        /// <summary>
        /// load the saved data on activation
        /// </summary>
        private void OnEnable()
        {
            Data = DataSaver.LoadData<SettingsData>("settingsData");
            
            SliderMouse.GetComponent<Scrollbar>().value = Data.Mouse * (1.0f/3.0f);
            SliderDifficulty.GetComponent<Scrollbar>().value = Data.Difficulty * 0.5f;
        }
        
        /// <summary>
        /// temporarily save difficulty to _tempDifficulty
        /// and change UI
        /// </summary>
        public void SetDifficulty()
        {
            float sliderValue = SliderDifficulty.GetComponent<Scrollbar>().value;
            // set state name on screen
            if (sliderValue < 0.5f)
            {
                ShowDifficulty[0].SetActive(true);
                ShowDifficulty[1].SetActive(false);
                ShowDifficulty[2].SetActive(false);
                _tempDifficulty = 0;
            }
            else if (sliderValue < 1.0f)
            {
                ShowDifficulty[0].SetActive(false);
                ShowDifficulty[1].SetActive(true);
                ShowDifficulty[2].SetActive(false);
                _tempDifficulty = 1;
            }
            else
            {
                ShowDifficulty[0].SetActive(false);
                ShowDifficulty[1].SetActive(false);
                ShowDifficulty[2].SetActive(true);
                _tempDifficulty = 2;
            }
        }
        
        /// <summary>
        /// temporarily save cursor to _tempCursor
        /// and change UI
        /// </summary>
        public void SetCursor()
        {
            float sliderValue = SliderMouse.GetComponent<Scrollbar>().value;
            int state = (int) Mathf.Floor(sliderValue * 4.0f);
            state = state == 4 ? 3 : state;
            _tempCursor = state;
            // set show cursor on screen
            foreach (GameObject v in ShowCursor)
            {
                v.SetActive(false);
            }
            
            ShowCursor[state].SetActive(true);
            
            Cursor.SetCursor(CursorSprite[state],Vector2.zero,CursorMode.ForceSoftware);
        }
        
        /// <summary>
        /// save all _temp variables to _saved
        /// and write data to settings save file
        /// </summary>
        public void SaveSettings()
        {
            Data = DataSaver.LoadData<SettingsData>("settingsData");
            
            _savedCursor = _tempCursor;
            _savedDifficulty = _tempDifficulty;
            Cursor.SetCursor(CursorSprite[_savedCursor],Vector2.zero,CursorMode.ForceSoftware);

            Data.Mouse = _savedCursor;
            Data.Difficulty = _savedDifficulty;

            DataSaver.SaveData(Data, "settingsData");
        }
        
        /// <summary>
        /// undo all _temp variables
        /// and reset sliders and ui to _saved values
        /// </summary>
        public void Undo()
        {
            Data = DataSaver.LoadData<SettingsData>("settingsData");
            
            SliderDifficulty.GetComponent<Scrollbar>().value = _savedDifficulty * 0.5f;
            SliderMouse.GetComponent<Scrollbar>().value = _savedCursor * (1.0f/3.0f);
            
            ShowDifficulty[0].SetActive(_savedDifficulty==0);
            ShowDifficulty[1].SetActive(_savedDifficulty==1);
            ShowDifficulty[2].SetActive(_savedDifficulty==2);

            
            Cursor.SetCursor(CursorSprite[_savedCursor],Vector2.zero,CursorMode.ForceSoftware);

            Data.Mouse = _savedCursor;
            Data.Difficulty = _savedDifficulty;
            
            DataSaver.SaveData(Data, "settingsData");
        } 
    }
}