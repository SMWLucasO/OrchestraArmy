using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Menu
{
    public class MainMenu : MonoBehaviour
    {
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