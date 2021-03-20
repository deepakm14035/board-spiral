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
    public Material[] _BoardTrailList;
    public GameObject[] _BackgroundList;
    public GameObject[] _BackgroundPrefabList;

    public float _score = 0;
    public int _currentLevel;
    public int _currentWorld;
    public int maxScore;
    float _startHeight = 0f;
    public GameObject MovingIndicator;
    SaveData loadedData;

    float _nextHeight = 25f;
    float _lookAheadConst = 50f;
    public bool _isInfinityMode = false;
    bool gameStarted = false;
    bool isFirstGame = false;
    bool isPlayingTutorial = false;
    int _coinCount;

    float _incrementAfterDistance = 100f;
    float _lastIncrementHeight=0f;
    float _noOfIncrements = 1f;
    float increment = 1.3f;

    void Start()
    {
        _levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerController1 = GameObject.FindObjectOfType<PlayerController1>();
        getPlayerData(true);
        updatePlayerBoard();
        updatePlayerBG();
        MovingIndicator.SetActive(false);
        AdManager.instance.loadBanner();
        AdManager.instance.loadFullScreenAd();
        AdManager.instance.showBanner();
    }

    public void updatePlayerBoard()
    {
        Color newColor = _BoardList[loadedData.selectedBoard].GetComponent<Image>().color;
        _playerController.boardImage.sprite = _BoardList[loadedData.selectedBoard].GetComponent<Image>().sprite;
        _playerController.boardImage.color = newColor;
        TrailRenderer[] trails = _playerController.gameObject.GetComponentsInChildren<TrailRenderer>(true);
        for(int i = 0; i < trails.Length; i++)
        {
            trails[i].material = _BoardTrailList[loadedData.selectedBoard];
            Debug.Log("trail name - "+ trails[i].gameObject.name+", "+ _BoardList[loadedData.selectedBoard].GetComponent<Image>().color);
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(newColor, 0.0f), new GradientColorKey(newColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0f, 1.0f) }
            );
            trails[i].colorGradient = gradient;
        }

        ParticleSystem[] ps = _playerController.gameObject.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < ps.Length; i++)
            ps[i].startColor = newColor;
    }
    public void updatePlayerBG()
    {
        Debug.Log(_BackgroundList[loadedData.selectedBackground]);
        _levelGenerator.backgroundImage = _BackgroundPrefabList[loadedData.selectedBackground];
    }

    public void loadInfinityMode(bool resetRequired)
    {
        _isInfinityMode = true;
        gameStarted = true;
        isPlayingTutorial = false;
        _nextHeight = 15f;
        _lastIncrementHeight = 0f;
        _levelGenerator.clearLevel();
        _levelGenerator.createBoundaries(new Vector4(-15f,-15f,30f,10000f), true);
        _playerController.resetPosition(resetRequired, true,true);
        if (getPlayerData(false).gamesPlayed == 0)
        {
            StartCoroutine(playTutorial());
            isFirstGame = true;
            isPlayingTutorial = true;
        }else
            generateInfinityObstacles();
        _playerController1.MoveSpeed = 6f;
        _playerController.RotSpeed = 35f;
        GameMenu.Instance.setScoreVisibility(true);
        _score = 0;
        _noOfIncrements = 1;
        _coinCount = 0;
        Time.timeScale=1f;
        GameMenu.Instance.setCoins(_coinCount + "");
        AdManager.instance.hideBanner();
        AdManager.instance.loadFullScreenAd();
    }

    void generateInfinityObstacles()
    {
        while (_nextHeight - _playerController.gameObject.transform.position.y < 200f)
        {
            int randomLevel = getRandomLevel();
            if (!_levelGenerator.isMovingLevel(randomLevel))
                continue;
            //if (_levelGenerator.isNonMovingLevel(randomLevel))
                //_nextHeight += 10f;
            Debug.Log("[generateInfinityObstacles] - " + randomLevel+", "+_nextHeight);
            GameObject level =  _levelGenerator.generateLevel(randomLevel, _nextHeight, _startHeight);
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
        AdManager.instance.hideBanner();
        AdManager.instance.loadFullScreenAd();
        StartCoroutine(loadLevel(_currentWorld, _currentLevel));
    }

    public void loadNextLevel()
    {
        _currentLevel++;
        AdManager.instance.loadFullScreenAd();
        startLevel( _currentWorld, _currentLevel);
    }

    public void startLevel(int worldNo, int index) {
        _currentLevel = index;
        _currentWorld = worldNo;
        _isInfinityMode = false;
        AdManager.instance.hideBanner();
        AdManager.instance.loadFullScreenAd();
        StartCoroutine(loadLevel(worldNo,index));
    }

    IEnumerator loadLevel(int worldNo, int index) {
        isPlayingTutorial = false;
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
        isPlayingTutorial = false;
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
        
        if(value==2 && saveData.worlds[worldNo].levelList.scores[level] != 2)
        {
            saveData.completedLevels++;
            Leaderboard.unlockAchievement(3, saveData.completedLevels);
        }
        saveData.worlds[worldNo].levelList.scores[level] = value;
        if(saveData.worlds[worldNo].levelList.scores.Length > level + 1)
            saveData.worlds[worldNo].levelList.scores[level+1] = 1;
        jsonSaver.saveData(saveData);
        getPlayerData(true);
    }

    public int getLevelProgress(int worldNo, int level)
    {
        SaveData saveData = getPlayerData(false);
        //Debug.Log("w no - "+worldNo+", "+level);
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
            Instantiate(_newBestScorePS, Camera.main.transform.position,Quaternion.identity);
            isHighScore = true;
            Leaderboard.addScoreToLeaderboard(Mathf.RoundToInt(_score));
            Leaderboard.unlockAchievement(1,_score);
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

        if(saveData.gamesPlayed%8==0 && saveData.gamesPlayed > 0)
        {
            AdManager.instance.showFullScreenAd();
        }
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

    public bool buyBoard(int i)
    {
        getPlayerData(true);
        int currCoins = loadedData.totalCoins;
        Debug.Log("cutting");
        if (currCoins < _BoardList[i].GetComponent<Board>().cost)
            return false;
        loadedData.totalCoins -= _BoardList[i].GetComponent<Board>().cost;
        loadedData.purchasedBoards[i] = 2;
        int count = -1;
        for (int b = 0; b < loadedData.purchasedBoards.Length; b++)
            if (loadedData.purchasedBoards[b] == 2 || b == loadedData.selectedBoard)
                count++;
        Leaderboard.unlockAchievement(1, count);

        JSONSaver jsonSaver = GameObject.FindObjectOfType<JSONSaver>();
        jsonSaver.saveData(loadedData);
        Debug.Log("cutted");
        return true;
    }

    private void Update()
    {
        if (_isInfinityMode && gameStarted && !isPlayingTutorial)
        {
            if (_nextHeight - _playerController.gameObject.transform.position.y < _lookAheadConst)
                generateInfinityObstacles();
            if (_playerController.gameObject.transform.position.y - _lastIncrementHeight > _incrementAfterDistance)
            {
                _lastIncrementHeight = _playerController.gameObject.transform.position.y;
                //_playerController.RotSpeed *= _rotationIncrement;
                //_playerController1.MoveSpeed *= _speedIncrement;
                _noOfIncrements *= increment;
                Time.timeScale *= 1.1f;

            }
            //if (_playerController.gameObject.transform.position.y < 75f)
                _score += Time.deltaTime * _noOfIncrements;
        }
        else if (_isInfinityMode && gameStarted && isPlayingTutorial)
        {
            _score = 0f;
            _startHeight= _playerController.gameObject.transform.position.y+20f;
            _lastIncrementHeight = _playerController.gameObject.transform.position.y;
            _nextHeight = _playerController.gameObject.transform.position.y+30f;
        }

        GameMenu.Instance.setScore(Mathf.Round(_score) + "");
    }

    

}
