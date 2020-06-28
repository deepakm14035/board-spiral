using System.Collections;
using System.Collections.Generic;
using System;

namespace MenuManagement.Data
{
    [Serializable]
    public class SaveData
    {
        public static string defaultPlayerName = "Player";
        public string playerName;
        public int currentLevel;
        public string hashValue;

        public SaveData() {
            playerName = defaultPlayerName;
            currentLevel = 0;
        }

    }
}