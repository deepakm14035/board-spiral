using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu<GameMenu>
{
    [SerializeField]
    private Text _levelText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _coinsText;

    public void OnPause()
    {
        MenuManager menuManager = MenuManager.Instance;
        menuManager.loadMenu(PauseMenu.Instance);
        Time.timeScale = 0f;
    }

    public void OnEnable()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager._isInfinityMode)
            _levelText.gameObject.transform.parent.gameObject.SetActive(false);
        else
            _levelText.gameObject.transform.parent.gameObject.SetActive(true);
        if (_levelText && !gameManager._isInfinityMode)
            _levelText.text = gameManager._currentLevel + "";
    }

    public void setScore(string score) {
        _scoreText.text = score;
    }
    public void setCoins(string coins)
    {
        _coinsText.text = coins;
    }
    public void setScoreVisibility(bool isVisible)
    {
        _scoreText.gameObject.transform.parent.gameObject.SetActive(isVisible);
    }
}
