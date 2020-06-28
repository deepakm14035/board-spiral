using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GenerateLevel levelGenerator;
    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
    }

    public void loadCurrentLevel() {
        JSONSaver jsonSaver = FindObjectOfType<JSONSaver>();
        SaveData saveData = new SaveData();
        saveData = jsonSaver.loadData(saveData);
        loadLevel(saveData.currentLevel);
    }

    public void loadNextLevel()
    {
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        SaveData saveData = new SaveData();
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        saveData = jsonSaver.loadData(saveData);
        saveData.currentLevel++;
        if (saveData.currentLevel >= levelGenerator.NoOfLevels())
            saveData.currentLevel = 0;
        jsonSaver.saveData(saveData);
        loadLevel(saveData.currentLevel);
    }

    public void loadLevel(int index) {
        levelGenerator.clearLevel();
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.resetPosition();
        levelGenerator.generateLevel(index);
    }
    
}
