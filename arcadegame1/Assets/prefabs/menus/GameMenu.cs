using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : Menu<GameMenu>
{
    public void OnPause()
    {
        MenuManager menuManager = MenuManager.Instance;
        menuManager.loadMenu(PauseMenu.Instance);
        Time.timeScale = 0f;
    }
}
