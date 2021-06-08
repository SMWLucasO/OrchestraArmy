using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Enum;
using OrchestraArmy.Keybindings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseScreen;
        public void Continue()
        {
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
                Debug.Log("1");
                if (Time.timeScale != 0)
                {
                    Debug.Log("pause");
                    Time.timeScale = 0;
                    PauseScreen.SetActive(true);
                }
                else
                {
                    Debug.Log("unpause");
                    PauseScreen.SetActive(false);
                    Continue();
                }
            }
        }
    }
}