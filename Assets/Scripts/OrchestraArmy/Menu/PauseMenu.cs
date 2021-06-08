using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Enum;
using OrchestraArmy.Keybindings;
using OrchestraArmy.Music.Data;
using OrchestraArmy.Music.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseScreen;
        public GameObject MusicTimer;
        public void Continue()
        {
            MusicTimer.GetComponent<RhythmSliderController>().GetRhythmController().PauseTimer();
            Time.timeScale = 1;
        }

        public void Settings()
        {
            //todo: make settings menu reachable from ingame
            return;
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        
        private void Update()
        {
            //press esc key to pause and unpause game
            if (KeybindingManager.Instance.Keybindings["pause button"].wasPressedThisFrame)
            {
                if (Time.timeScale != 0)
                {
                    MusicTimer.GetComponent<RhythmSliderController>().GetRhythmController().PauseTimer();
                    Time.timeScale = 0;
                    PauseScreen.SetActive(true);
                }
                else
                {
                    PauseScreen.SetActive(false);
                    Continue();
                }
            }
        }
    }
}