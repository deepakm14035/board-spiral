using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenu : Menu<LoseMenu>
{
    public void Replay() {
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
        menuManager.goBack();
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.loadCurrentLevel();
    }
}
