using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu<GameMenu>
{
    [SerializeField]
    private Text _levelText;
    public void OnPause()
    {
        MenuManager menuManager = MenuManager.Instance;
        menuManager.loadMenu(PauseMenu.Instance);
        Time.timeScale = 0f;
    }

    public void OnEnable()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (_levelText)
            _levelText.text = gameManager._currentLevel + "";
    }
}
