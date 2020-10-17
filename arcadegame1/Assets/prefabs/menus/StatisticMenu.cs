using MenuManagement.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MenuManagement
{
    public class StatisticMenu : Menu<StatisticMenu>
    {
        [SerializeField]
        private Text gamesPlayed;
        [SerializeField] private Text bestScore;
        [SerializeField] private Text CoinsEarned;
        [SerializeField] private GameObject[] pastScores;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void loadData() {
            GameManager gameManager = FindObjectOfType<GameManager>();
            SaveData saveData = gameManager.getPlayerData(true);
            gamesPlayed.text = saveData.gamesPlayed + "";
            bestScore.text = saveData.maxScore + "";
            CoinsEarned.text = saveData.totalCoins + "";
            int[] pastScore = saveData.pastScores;
            for(int i = 0; i < pastScore.Length; i++)
            {
                pastScores[i].SetActive(true);
                pastScores[i].GetComponentInChildren<Text>().text = pastScore[i] + "";
                if (pastScore[i] == 0)
                {
                    pastScores[i].SetActive(false);
                }
                else if(pastScore[i]>0 && pastScore[i]<25)
                    pastScores[i].GetComponent<Image>().color = new Color32(40, 40, 200, 255);
                else if (pastScore[i] >=25 && pastScore[i] < 50)
                    pastScores[i].GetComponent<Image>().color = new Color32(75, 100, 175, 255);
                else if (pastScore[i] >= 50 && pastScore[i] < 100)
                    pastScores[i].GetComponent<Image>().color = new Color32(40, 190, 170, 255);
                else if (pastScore[i] >= 100 && pastScore[i] < 200)
                    pastScores[i].GetComponent<Image>().color = new Color32(190, 120, 40, 255);
                else if (pastScore[i] >= 200 && pastScore[i] < 500)
                    pastScores[i].GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                if(pastScore[i] >= 500)
                    pastScores[i].GetComponent<Image>().color = new Color32(230, 230, 0, 255);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}