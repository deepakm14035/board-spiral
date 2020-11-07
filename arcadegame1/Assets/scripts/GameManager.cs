using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GenerateLevel _levelGenerator;
    LineRenderer _boundaries;

    PlayerController _playerController;
    PlayerController1 _playerController1;
    [SerializeField]
    private GameObject _HowToPlayPanel1;
    [SerializeField]
    private GameObject _HowToPlayPanel2;
    [SerializeField]
    private GameObject _newBestScorePS;
    public GameObject[] _BoardList;

    public float _score = 0;
    public int _currentLevel;
    public int _currentWorld;
    public int maxScore;
    public GameObject MovingIndicator;
    SaveData loadedData;

    int _numLevelsCrossed;
    float _nextHeight = 25f;
    float _lookAheadConst = 50f;
    public bool _isInfinityMode = false;
    bool gameStarted = false;
    int _coinCount;

    float _speedIncrement = 1.1f;
    float _rotationIncrement = 1.1f;
    float _incrementAfterDistance = 200f;
    float _lastIncrementHeight=0f;
    int _noOfIncrements = 1;

    void Start()
    {
        _levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerController1 = GameObject.FindObjectOfType<PlayerController1>();
        getPlayerData(true);
        updatePlayerBoard();
        MovingIndicator.SetActive(false);
    }

    public void updatePlayerBoard()
    {
        _playerController.boardImage.sprite = _BoardList[loadedData.selectedBoard].GetComponent<Image>().sprite;
    }

    public void loadInfinityMode(bool resetRequired)
    {
        _isInfinityMode = true;
        gameStarted = true;
        _nextHeight = 15f;
        _lastIncrementHeight = 0f;
        _numLevelsCrossed = 2;
        _levelGenerator.clearLevel();
        _levelGenerator.createBoundaries(new Vector4(-15f,-15f,30f,10000f), true);
        _playerController.resetPosition(resetRequired, true,true);
        if (getPlayerData(false).gamesPlayed == 0)
        {
            StartCoroutine(playTutorial());
            _nextHeight = 100f;
        }
        generateInfinityObstacles();
        _playerController1.MoveSpeed = 6f;
        _playerController.RotSpeed = 35f;
        GameMenu.Instance.setScoreVisibility(true);
        _score = 0;
        _noOfIncrements = 1;
        _coinCount = 0;
        
    }

    void generateInfinityObstacles()
    {
        _numLevelsCrossed++;
        while (_nextHeight - _playerController.gameObject.transform.position.y < 200f)
        {
            int randomLevel = getRandomLevel();
            if (!_levelGenerator.isMovingLevel(randomLevel))
                continue;
            //if (_levelGenerator.isNonMovingLevel(randomLevel))
                //_nextHeight += 10f;
            Debug.Log("[generateInfinityObstacles] - " + randomLevel+", "+_nextHeight);
            GameObject level =  _levelGenerator.generateLevel(randomLevel, _nextHeight);
            //level.transform.localScale = new Vector3(1f,1.5f,1f);
            _nextHeight += _levelGenerator.getLevelHeight(randomLevel) ;
            if (!_levelGenerator.isNonMovingLevel(randomLevel))
                _nextHeight -= 10f;
        }
    }

    int getRandomLevel()
    {
        int lowerLimit = 0;
        //int upperLimit = _numLevelsCrossed / 2;
        int upperLimit = _levelGenerator.NoOfLevels();
        float newLevel = Mathf.FloorToInt(Random.Range(0, Mathf.Pow( _levelGenerator.NoOfLevels(),2)+100));
        newLevel = _levelGenerator.NoOfLevels() - Mathf.Pow(newLevel, 0.5f) +1;
        //Debug.Log("[getRandomLevel] - "+ Mathf.FloorToInt(newLevel));
        //        return Random.Range(lowerLimit, upperLimit);
        return Mathf.Clamp( Mathf.FloorToInt(newLevel), 1, _levelGenerator.NoOfLevels()-1);
    }

    public void loadCurrentLevel() {
        Time.timeScale = 1f;
        StartCoroutine(loadLevel(_currentWorld, _currentLevel));
    }

    public void loadNextLevel()
    {
        _currentLevel++;
        startLevel( _currentWorld, _currentLevel);
    }

    public void startLevel(int worldNo, int index) {
        _currentLevel = index;
        _currentWorld = worldNo;
        _isInfinityMode = false;
        StartCoroutine(loadLevel(worldNo,index));
    }

    IEnumerator loadLevel(int worldNo, int index) {
        yield return new WaitForSeconds(1f);//wait for menu transitions
        _levelGenerator.clearLevel();
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.resetPosition(true, true,true);
        _levelGenerator.generateLevel(worldNo, index);
        _boundaries = GameObject.FindGameObjectWithTag("borders").GetComponent<LineRenderer>();
        if (index == 0)
        {
            StartCoroutine(playTutorial());
        }
        if (_levelGenerator.isMoving(worldNo, index))
            StartCoroutine(playMovingIndicator());
        Debug.Log("boundaries-"+ _boundaries);
    }

    IEnumerator playTutorial()
    {
        //_playerController1.allowMoving = false;
        //gameStarted = false;
        yield return new WaitForSeconds(2f);
        _HowToPlayPanel1.SetActive(true);
        yield return new WaitForSeconds(6f);
        _HowToPlayPanel1.SetActive(false);
        yield return new WaitForSeconds(2f);
        _HowToPlayPanel2.SetActive(true);
        yield return new WaitForSeconds(6f);
        _HowToPlayPanel2.SetActive(false);
        yield return null;
        //gameStarted = true;
        //_playerController.resetPosition(true, true);
        //_playerController1.allowMoving = true;

    }

    IEnumerator playMovingIndicator()
    {
        yield return new WaitForSeconds(2f);
        MovingIndicator.SetActive(true);
        yield return new WaitForSeconds(.6f);
        MovingIndicator.SetActive(false);
        yield return new WaitForSeconds(.6f);
        MovingIndicator.SetActive(true);
        yield return new WaitForSeconds(.6f);
        MovingIndicator.SetActive(false);
        yield return null;
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

    public void updateProgressForWin()
    {
        updateProgress(_currentWorld, _currentLevel,2);

    }

    public void updateProgress(int worldNo, int level,int value)
    {
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        SaveData saveData = getPlayerData(false);
        saveData.worlds[worldNo].levelList.scores[level] = value;
        if(saveData.worlds[worldNo].levelList.scores.Length > level + 1)
            saveData.worlds[worldNo].levelList.scores[level+1] = 1;
        jsonSaver.saveData(saveData);
        getPlayerData(true);
    }

    public int getLevelProgress(int worldNo, int level)
    {
        SaveData saveData = getPlayerData(false);
        Debug.Log("w no - "+worldNo+", "+level);
        return saveData.worlds[worldNo].levelList.scores[level];
    }

    public bool isLastLevel()
    {
        if (_currentLevel == _levelGenerator.getNoOfLevels(_currentWorld) - 1)
            return true;
        return false;
        
    }

    public void addCoin()
    {
        _coinCount++;
        GameMenu.Instance.setCoins(_coinCount + "");
    }

    public bool updateStats()
    {
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        SaveData saveData = getPlayerData(false);
        bool isHighScore = false;
        if (saveData.maxScore < _score)
        {
            saveData.maxScore = Mathf.RoundToInt(_score);
            Instantiate(_newBestScorePS, _playerController.gameObject.transform.position+new Vector3(0f,0f,10f),Quaternion.identity);
            isHighScore = true;
        }
        maxScore = saveData.maxScore;
        saveData.totalCoins += _coinCount;
        saveData.gamesPlayed++;
        saveData.infinityAverage = (saveData.infinityAverage * (saveData.gamesPlayed-1) + _score) / saveData.gamesPlayed;

        for (int i = saveData.pastScores.Length-1; i >0; i--)
        {
            saveData.pastScores[i] = saveData.pastScores[i-1];
           
        }
        saveData.pastScores[0] = Mathf.RoundToInt(_score);

        jsonSaver.saveData(saveData);
        getPlayerData(true);
        return isHighScore;
    }

    public int getCoins()
    {
        SaveData saveData = getPlayerData(true);
        return saveData.totalCoins;
    }

    public SaveData getPlayerData(bool reload)
    {
        if (reload || loadedData==null)
        {
            JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
            if(_levelGenerator==null)
                _levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
            SaveData saveData = new SaveData(_levelGenerator);
            loadedData = jsonSaver.loadData(saveData, _levelGenerator);
        }
        return loadedData;
    }
    public void savePlayerData()
    {
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        jsonSaver.saveData(loadedData);
    }

    public void setGameStarted(bool started)
    {
        gameStarted = started;
    }

    private void Update()
    {
        if (_isInfinityMode && gameStarted)
        {
            if (_nextHeight - _playerController.gameObject.transform.position.y < _lookAheadConst)
                generateInfinityObstacles();
            if (_playerController.gameObject.transform.position.y - _lastIncrementHeight > _incrementAfterDistance)
            {
                _lastIncrementHeight = _playerController.gameObject.transform.position.y;
                //_playerController.RotSpeed *= _rotationIncrement;
                //_playerController1.MoveSpeed *= _speedIncrement;
                _noOfIncrements++;
                Time.timeScale *= 1.05f;

            }
            _score += Time.deltaTime * _noOfIncrements;
            GameMenu.Instance.setScore(Mathf.Round(_score)+"");
        }
    }

    public bool buyBoard(int i)
    {
        getPlayerData(true);
        int currCoins = loadedData.totalCoins;
        Debug.Log("cutting");
        if (currCoins < _BoardList[i].GetComponent<Board>().cost)
            return false;
        loadedData.totalCoins -= _BoardList[i].GetComponent<Board>().cost;
        loadedData.purchasedBoards[i] = 2;
        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        jsonSaver.saveData(loadedData);
        Debug.Log("cutted");
        return true;
    }

}
