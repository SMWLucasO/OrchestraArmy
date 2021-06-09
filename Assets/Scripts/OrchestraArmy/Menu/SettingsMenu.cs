using UnityEngine;

namespace OrchestraArmy.Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        public GameObject FullScreenCheckmark;

        public void FullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            FullScreenCheckmark.SetActive(Screen.fullScreen);
        }
    }
}