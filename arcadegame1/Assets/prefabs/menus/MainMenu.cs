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
            //GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            //gameManager.loadCurrentLevel();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            //menuManager.loadMenu(GameMenu.Instance,3f,true);
            LevelSelecterMenu.Instance.setup();
            menuManager.loadMenu(LevelSelecterMenu.Instance, 1f, true);

        }

        public void playInfinite()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadInfinityMode();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance,1f, true);
        }

    }
}