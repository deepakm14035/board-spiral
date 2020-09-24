using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class WinMenu : Menu<WinMenu>
    {
        public void NextLevel()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadNextLevel();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance, 1f, true);
        }

        public void Replay()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadCurrentLevel();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance, 1f, true);
        }
    }
}