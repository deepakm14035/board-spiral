using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MenuManagement.Data
{
    [Serializable]
    public class SaveData
    {
        public static string defaultPlayerName = "Player";
        public string playerName;
        public int currentLevel;
        public bool isFirstTime;
        public string hashValue;
        public World[] worlds;
        public int maxScore;
        public int totalCoins;
        public int gamesPlayed;
        public float infinityAverage;
        public int[] pastScores;
        public int[] purchasedBoards;
        public int selectedBoard;
        public SaveData(GenerateLevel generateLevel) {
            playerName = defaultPlayerName;
            currentLevel = 0;
            isFirstTime = true;
            worlds = new World[generateLevel.getNoOfWorlds()];
            Debug.Log("worlds - "+ generateLevel.getNoOfWorlds());
            maxScore = 0;
            pastScores = new int[10];
            for (int i = 0; i < worlds.Length; i++)
            {
                //Debug.Log("i = "+i);
                worlds[i] = new World();
                worlds[i].levelList = new Levels();
                worlds[i].levelList.scores = new int[generateLevel.getNoOfLevels(i)];
                worlds[i].levelList.scores[0] = 1;
                //0 - locked
                //1 - can play
                //2 - completed
            }
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            purchasedBoards = new int[gameManager._BoardList.Length];
            for (int i = 0; i < purchasedBoards.Length; i++)
            {
                purchasedBoards[i] = 0;
                if (i == 0)
                    purchasedBoards[i] = 2;
                //0 - locked
                //1 - can buy
                //2 - bought
            }
            gamesPlayed = 0;
            infinityAverage = 0f;
            selectedBoard = 0;
        }

    }

    [Serializable]
    public class World {
        public Levels levelList;
    }
    [Serializable]
    public class Levels {
        public int[] scores;
    }
}