using System;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.UI;


namespace OrchestraArmy.Menu
{
    public class GameSettings : MonoBehaviour
    {
        public Texture2D[] cursorSprite;
        public GameObject[] showCursor;
        public GameObject[] showDifficulty;
        public GameObject sliderDifficulty;
        public GameObject sliderMouse;
        
        private int savedDifficulty = 1;
        private int savedCursor = 0;
        private int tempDifficulty = 1;
        private int tempCursor = 0;
        


        public void SetDifficulty()
        {
            float sliderValue = sliderDifficulty.GetComponent<Scrollbar>().value;
            int state = (int) Mathf.Floor(sliderValue * 2.0f);
            state = state == 3 ? 2 : state;
            tempDifficulty = state;
            // set state name on screen
            switch (state)
            {
                case 0:
                    showDifficulty[0].SetActive(true);
                    showDifficulty[1].SetActive(false);
                    showDifficulty[2].SetActive(false);
                    break;
                case 1:
                    showDifficulty[0].SetActive(false);
                    showDifficulty[1].SetActive(true);
                    showDifficulty[2].SetActive(false);
                    break;
                case 2:
                    showDifficulty[0].SetActive(false);
                    showDifficulty[1].SetActive(false);
                    showDifficulty[2].SetActive(true);
                    break;
            }
        }

        public void SetCursor()
        {
            float sliderValue = sliderMouse.GetComponent<Scrollbar>().value;
            int state = (int) Mathf.Floor(sliderValue * 4.0f);
            state = state == 4 ? 3 : state;
            tempCursor = state;
            // set show cursor on screen
            foreach (GameObject v in showCursor)
            {
                v.SetActive(false);
            }
            showCursor[state].SetActive(true);
            
            Cursor.SetCursor(cursorSprite[state],Vector2.zero,CursorMode.ForceSoftware);
        }

        public void SaveSettings()
        {
            savedCursor = tempCursor;
            Cursor.SetCursor(cursorSprite[savedCursor],Vector2.zero,CursorMode.ForceSoftware);
            //todo: link to damage and speed of enemies
        }

        public void Undo()
        {
            sliderDifficulty.GetComponent<Scrollbar>().value = savedDifficulty/2;
            sliderDifficulty.GetComponent<Scrollbar>().value = savedCursor/4;
            
            showDifficulty[0].SetActive(false);
            showDifficulty[1].SetActive(true);
            showDifficulty[2].SetActive(false);
            
            Cursor.SetCursor(cursorSprite[0],Vector2.zero,CursorMode.ForceSoftware);
        } 
    }
}