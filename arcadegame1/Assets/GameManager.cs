using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GenerateLevel _levelGenerator;
    LineRenderer _boundaries;

    PlayerController _playerController;
    PlayerController1 _playerController1;


    public int _currentLevel;
    int _numLevelsCrossed;
    float _nextHeight = 15f;
    float _lookAheadConst = 100f;
    bool _isInfinityMode = false;

    float _speedIncrement = 1.1f;
    float _rotationIncrement = 1.1f;
    float _incrementAfterDistance = 100f;
    float _lastIncrementHeight=0f;

    void Start()
    {
        _levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerController1 = GameObject.FindObjectOfType<PlayerController1>();
    }

    public void loadInfinityMode()
    {
        _isInfinityMode = true;
        _numLevelsCrossed = 2;
        _levelGenerator.createBoundaries(new Vector4(-15f,-15f,30f,10000f));
        generateInfinityObstacles();
        _playerController1.MoveSpeed = 8f;
        _playerController.RotSpeed = 35f;

    }

    void generateInfinityObstacles()
    {
        _numLevelsCrossed++;
        while (_nextHeight - _playerController.gameObject.transform.position.y < 100f)
        {
            int randomLevel = getRandomLevel();
            if (!_levelGenerator.isMovingLevel(randomLevel))
                continue;
            Debug.Log("level"+randomLevel+", "+_nextHeight+", upper-"+ _numLevelsCrossed / 2);
            GameObject level =  _levelGenerator.generateLevel(randomLevel, _nextHeight);
            //level.transform.localScale = new Vector3(1f,1.5f,1f);
            _nextHeight += _levelGenerator.getLevelHeight(randomLevel) ;
        }
    }

    int getRandomLevel()
    {
        int lowerLimit = 0;
        //int upperLimit = _numLevelsCrossed / 2;
        int upperLimit = _levelGenerator.NoOfLevels();

        return Random.Range(lowerLimit, upperLimit);
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
            if (GameObject.FindGameObjectWithTag("borders"))
                _boundaries = GameObject.FindGameObjectWithTag("borders").GetComponent<LineRenderer>();
            else return false;
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

    private void Update()
    {
        if (_isInfinityMode)
        {
            if (_nextHeight - _playerController.gameObject.transform.position.y < 30f)
                generateInfinityObstacles();
            if (_playerController.gameObject.transform.position.y - _lastIncrementHeight > _incrementAfterDistance)
            {
                _lastIncrementHeight = _playerController.gameObject.transform.position.y;
                _playerController.RotSpeed = _playerController.RotSpeed * _rotationIncrement;
                _playerController1.MoveSpeed = _playerController1.MoveSpeed * _speedIncrement;

            }
        }
    }

}
