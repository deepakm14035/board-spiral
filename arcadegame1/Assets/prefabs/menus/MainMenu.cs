using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        public void play()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadCurrentLevel();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance);
        }
        
        public void playInfinite()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadInfinityMode();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance);
        }
    }
}