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
        public Texture2D[] cursorSprite;
        
        private void OnEnable()
        {
            SettingsData data = DataSaver.LoadData<SettingsData>("settingsData");
            if (data != null)
                Cursor.SetCursor(cursorSprite[data.mouse],Vector2.zero,CursorMode.ForceSoftware);
            else
                DataSaver.SaveData(new SettingsData(), "settingsData");
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}