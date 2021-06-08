﻿using System;
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
            // set state name on screen
            if (sliderValue < 0.5f)
            {
                showDifficulty[0].SetActive(true);
                showDifficulty[1].SetActive(false);
                showDifficulty[2].SetActive(false);
                tempDifficulty = 0;
            }
            else if (sliderValue < 1.0f)
            {
                showDifficulty[0].SetActive(false);
                showDifficulty[1].SetActive(true);
                showDifficulty[2].SetActive(false);
                tempDifficulty = 1;
            }
            else
            {
                showDifficulty[0].SetActive(false);
                showDifficulty[1].SetActive(false);
                showDifficulty[2].SetActive(true);
                tempDifficulty = 2;
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
            savedDifficulty = tempDifficulty;
            Cursor.SetCursor(cursorSprite[savedCursor],Vector2.zero,CursorMode.ForceSoftware);
            //todo: link to damage and speed of enemies
        }

        public void Undo()
        {
            sliderDifficulty.GetComponent<Scrollbar>().value = savedDifficulty * 0.5f;
            sliderMouse.GetComponent<Scrollbar>().value = savedCursor * (1.0f/3.0f);
            Debug.Log("-------------");
            Debug.Log(savedCursor);
            
            if (savedDifficulty==0)
            {
                showDifficulty[0].SetActive(true);
                showDifficulty[1].SetActive(false);
                showDifficulty[2].SetActive(false);
            }
            else if (savedDifficulty==1)
            {
                showDifficulty[0].SetActive(false);
                showDifficulty[1].SetActive(true);
                showDifficulty[2].SetActive(false);
            }
            else
            {
                showDifficulty[0].SetActive(false);
                showDifficulty[1].SetActive(false);
                showDifficulty[2].SetActive(true);
            }
            
            Cursor.SetCursor(cursorSprite[savedCursor],Vector2.zero,CursorMode.ForceSoftware);
        } 
    }
}