using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : Menu<LoseMenu>
{
    public Text bestScore;
    public Text currentScore;
    public GameObject bestScorePanel;
    public GameObject currentScorePanel;
    public Text coinsText;

    private void OnEnable()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager._isInfinityMode)
        {
            coinsText.gameObject.SetActive(true);
            currentScore.gameObject.SetActive(true);
            bestScore.gameObject.SetActive(true);
            coinsText.text = gameManager.getCoins() + "";
            currentScore.text = Mathf.RoundToInt(gameManager._score) + "";
            bestScore.text = gameManager.maxScore + "";
        }
        else
        {
            coinsText.gameObject.SetActive(false);
            currentScore.gameObject.SetActive(false);
            bestScore.gameObject.SetActive(false);

        }
    }

    private void Start()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        coinsText.text = gameManager.getCoins() + "";
        currentScore.text = Mathf.RoundToInt(gameManager._score) + "";
        bestScore.text = gameManager.maxScore + "";
    }

    public void Replay() {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (!gameManager._isInfinityMode)
            gameManager.loadCurrentLevel();
        else
            gameManager.loadInfinityMode(false);
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
        menuManager.goBack(true);

    }
}
