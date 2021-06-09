using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using OrchestraArmy.Enum;
using OrchestraArmy.SaveData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public Texture2D[] CursorSprite;
        
        /// <summary>
        /// load the saved data on activation
        /// </summary>
        private void OnEnable()
        {
            SettingsData data = DataSaver.LoadData<SettingsData>("settingsData");
            try
            {
                Cursor.SetCursor(CursorSprite[data.Mouse],Vector2.zero,CursorMode.ForceSoftware);
            }
            catch (Exception e)
            {
                Debug.Log("no saved settings found\ngenerating new settings");
                DataSaver.SaveData(new SettingsData(), "settingsData");
            }
        }
        
        /// <summary>
        /// load the game scene
        /// </summary>
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }
        
        /// <summary>
        /// quit the game
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}