using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        public Text coinsText;
        public Text topScore;
        private void Start()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            coinsText.text = gameManager.getCoins()+"";
            topScore.text = gameManager.getPlayerData(true).maxScore+"";
        }

        private void OnEnable()
        {
            StartCoroutine(loadStats());
        }

        IEnumerator loadStats()
        {
            yield return new WaitForEndOfFrame();
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            coinsText.text = gameManager.getCoins() + "";
            topScore.text = gameManager.getPlayerData(true).maxScore + "";
        }

        public void loadBoardSelector()
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(BoardSelectorMenu.Instance, 1f, false);
            BoardSelectorMenu.Instance.setup();
        }

        public void play()
        {
            //GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            //gameManager.loadCurrentLevel();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            //menuManager.loadMenu(GameMenu.Instance,3f,true);
            
            LevelSelecterMenu.Instance.setup();
            GetComponent<Animator>().SetTrigger("exit2");
            menuManager.loadMenu(LevelSelecterMenu.Instance, 0f, false);
        }
        public void playInfinite()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadInfinityMode(false);
            CameraFollow cf = FindObjectOfType<CameraFollow>();
            cf.startFollowing = true;
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance, 1f, false);
            GetComponent<Animator>().SetTrigger("exit");
        }

        public void loadStatistics()
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            StatisticMenu.Instance.loadData();
            menuManager.loadMenu(StatisticMenu.Instance, 0f, false);

        }
    }
}