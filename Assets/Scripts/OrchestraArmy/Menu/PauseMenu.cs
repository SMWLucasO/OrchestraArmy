using OrchestraArmy.Keybindings;
using OrchestraArmy.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrchestraArmy.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseScreen;
        public GameObject MusicTimer;
        
        /// <summary>
        /// reactivate the time
        /// timeScale for movement and MusicTimer for the music
        /// </summary>
        public void Continue()
        {
            MusicTimer.GetComponent<RhythmSliderController>().GetRhythmController().PauseTimer();
            Time.timeScale = 1;
        }

        /// <summary>
        /// reactivate the time
        /// and go back to the menu scene
        /// </summary>
        public void QuitToMenu()
        {
            Continue();
            SceneManager.LoadScene(0);
        }
        
        /// <summary>
        /// pause and unpause game using the 'esc' key
        /// </summary>
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