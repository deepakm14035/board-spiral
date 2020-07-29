using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GenerateLevel _levelGenerator;
    LineRenderer _boundaries;
    public int _currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        _levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
    }

    public void loadCurrentLevel() {
        JSONSaver jsonSaver = FindObjectOfType<JSONSaver>();
        SaveData saveData = new SaveData();
        saveData = jsonSaver.loadData(saveData);
        Time.timeScale = 1f;
        _currentLevel = saveData.currentLevel;
        loadLevel(saveData.currentLevel);
    }

    public void loadNextLevel()
    {
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        SaveData saveData = new SaveData();
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        saveData = jsonSaver.loadData(saveData);
        saveData.currentLevel++;
        if (saveData.currentLevel >= _levelGenerator.NoOfLevels())
            saveData.currentLevel = 0;
        jsonSaver.saveData(saveData);
        _currentLevel = saveData.currentLevel;
        loadLevel(saveData.currentLevel);

    }

    public void loadLevel(int index) {
        _levelGenerator.clearLevel();
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.resetPosition();
        _levelGenerator.generateLevel(index);
        _boundaries = GameObject.FindGameObjectWithTag("borders").GetComponent<LineRenderer>();
        Debug.Log("boundaries-"+ _boundaries);
    }

    public bool isPositionOutOfBounds(Vector3 position)
    {
        if (_boundaries == null)
            _boundaries = GameObject.FindGameObjectWithTag("borders").GetComponent<LineRenderer>();
        return position.x < _boundaries.GetPosition(0).x || position.y < _boundaries.GetPosition(0).y //left and lower boundaries
            || position.x > _boundaries.GetPosition(2).x || position.y > _boundaries.GetPosition(2).y;
    }

    public void stopAllObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        for(int i = 0; i < obstacles.Length; i++)
        {
            if (obstacles[i].GetComponent<WaypointMovement>())
                obstacles[i].GetComponent<WaypointMovement>().m_stopMoving = true; ;
        }
    }

    public void reverseRotation()
    {
        BackgroundImage[] images = GameObject.FindObjectsOfType<BackgroundImage>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].m_spinningClockwise = !images[i].m_spinningClockwise;
        }
    }
    
}
