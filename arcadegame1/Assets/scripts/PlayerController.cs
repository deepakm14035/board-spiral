using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
//fix pixel length

public class PlayerController : MonoBehaviour
{
    public Transform pos1, pos2,curpos;
    public SpriteRenderer boardImage;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject pivotEffectPrefab;
    [SerializeField] private GameObject trailRenderer1;
    [SerializeField] private GameObject trailRenderer2;
    [SerializeField] private GameObject particlesRenderer1;
    [SerializeField] private GameObject particlesRenderer2;
    [SerializeField] private GameObject gameWinParticles;
    [SerializeField] private GameObject UIButtonLeft, UIButtonRight;
    [SerializeField] private GameObject partitionObj;

    GameManager _gameManager;
    MenuManager _menuManager;
    Tutorial _tutorial;

    float direction = -1f;
    UIEffects uiEffects;
    public bool allowMoving;
    public bool gameStarted = false;
    PlayerController1 pc;
    Collider2D collisionObj;
    GameObject mainCamera;
    bool gameComplete = false;
    Vector3 startPosition;

    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }

    private void Awake()
    {
        boardImage = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiEffects = GameObject.FindObjectOfType<UIEffects>();
        pc = GameObject.FindObjectOfType<PlayerController1>();
        setMoving(false);
        //allowMoving = true;
        mainCamera = Camera.main.gameObject.transform.parent.gameObject;
        updatePivot1(false);
        updatePivot2(false);
        if (SystemInfo.deviceType.Equals(DeviceType.Desktop))
        {
            UIButtonLeft.SetActive(false);
            UIButtonRight.SetActive(false);
        }

        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _menuManager = GameObject.FindObjectOfType<MenuManager>();
        _tutorial = GameObject.FindObjectOfType<Tutorial>();
        startPosition = transform.position;
        partitionObj.SetActive(false);
    }

    public void setPivots(bool pivot1, bool pivot2)
    {
        if(pivot1 && !pivot2)
            curpos = pos2;
        else if(!pivot1 && pivot2)
            curpos = pos1;
        updatePivot1(pivot1);
        updatePivot2(pivot2);
    }

    public void setMoving(bool moving) {
        allowMoving = moving;
        if (pc != null)
            pc.allowMoving = moving;
    }

    IEnumerator disablePartition() {
        yield return new WaitForSeconds(5f);
        partitionObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMoving&& gameStarted && !gameComplete)
            transform.RotateAround(curpos.position,Vector3.forward, direction * 5f*Time.deltaTime*RotSpeed);
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z))&& allowMoving && !gameComplete) {
            if(!SystemInfo.deviceType.Equals(DeviceType.Handheld))
            changePivot();
        }
        if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.X)) && allowMoving && !gameComplete)
        {
            if (!SystemInfo.deviceType.Equals(DeviceType.Handheld))
                changeDirection();
        }

    }

    public void resetPosition(bool resetRequired, bool startGame, bool allowMoving) {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        if(resetRequired)
            mainCamera.GetComponent<CameraFollow>().resetCamera();
        curpos = pos1;
        trailRenderer2.SetActive(true);
        particlesRenderer2.SetActive(true);
        gameStarted = startGame;
        gameComplete = false;

        StartCoroutine(allowMovementAfterDelay(allowMoving));
    }

    IEnumerator allowMovementAfterDelay(bool allowMoving) {
        yield return new WaitForSeconds(2f);
        //trailRenderer1.SetActive(true);
        //trailRenderer2.SetActive(true);
        setMoving(allowMoving);
    }

    public void changeDirection() {
        if (!allowMoving)
            return;
        direction *= -1f;
        _gameManager.reverseRotation();
    }

    public float getDirection()
    {
        return direction;
    }

    public void changePivot()
    {
        if (!allowMoving ||_gameManager.isPositionOutOfBounds(pos1 == curpos ? pos2.transform.position : pos1.transform.position))
            return;
        if (pos1 == curpos)
        {
            //curpos = pos2;
            setPivots(true, false);
        }
        else
        {
            //curpos = pos1;
            setPivots(false, true);
        }
        if (_tutorial.isPlaying() && collisionObj!=null && collisionObj.gameObject.tag.Equals("tutorial"))
        {
            GameObject.FindObjectOfType<Tutorial>().updateProgress(trailRenderer1.activeSelf?1:2, direction, transform.position);
        }
        /*if (!gameStarted)
        {
            gameStarted = true;
            setMoving(true);
            return;
        }*/
        Instantiate(pivotEffectPrefab,curpos.transform.position,Quaternion.identity);
        StartCoroutine(pivotChangeDelay());

    }

    void updatePivot1(bool val)
    {
        trailRenderer1.SetActive(val);
        particlesRenderer1.SetActive(val);
    }

    void updatePivot2(bool val)
    {
        trailRenderer2.SetActive(val);
        particlesRenderer2.SetActive(val);
    }

    IEnumerator pivotChangeDelay()
    {
        if (gameComplete || !gameStarted)
            yield return null;
        else
        {
            setMoving(false);
            yield return new WaitForSeconds(0.2f);
            setMoving(true);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("coll name - "+collision.gameObject.name);
        if(_tutorial.isPlaying() && collision.gameObject.tag.Equals("obstacle"))
        {
            setMoving(false);
            StartCoroutine( _tutorial.Replay());
            return;
        }
        if (collision.gameObject.tag.Equals("tutorial"))
        {
            collisionObj = collision;
            return;
        }
        if (collision.gameObject.tag.Equals("obstacle") && !gameComplete)
        {
            gameComplete = true;
            setMoving(false);
            mainCamera.GetComponent<Animator>().SetTrigger("shake");
            _gameManager.stopAllObstacles();
            bool isHighScore = false;
            if (_gameManager._isInfinityMode)
            {
                isHighScore= _gameManager.updateStats();
                _menuManager.populateStats(_gameManager);
                _gameManager.setGameStarted(false);
            }
            Debug.Log("ishighscore-"+isHighScore);
            if(!isHighScore)
                uiEffects.playLoseAnimation();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            uiEffects.disableObjects();
            setPivots(false, false);
            menuManager.loadMenu(LoseMenu.Instance, 2f, false);
            LoseMenu.Instance.GetComponent<Animator>().SetTrigger("open");
            
        }
        if (collision.gameObject.tag.Equals("finish") && !gameComplete)
        {
            gameComplete = true;
            uiEffects.playWinAnimation();
            setMoving(false);
            //if (pc != null)
            //    pc.allowMoving = false;
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            uiEffects.disableObjects();
            setPivots(false, false);
            Instantiate(gameWinParticles, curpos.transform.position+Vector3.up*10f,Quaternion.identity);
            _gameManager.updateProgressForWin();
            if(!_gameManager.isLastLevel())
                menuManager.loadMenu(WinMenu.Instance,1f, false);
            else
                menuManager.loadMenu(MainMenu.Instance, 1f, false);


        }
        


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit-"+collision.gameObject.name);
        if (collisionObj!=null && collisionObj.gameObject.tag.Equals("tutorial"))
            collisionObj = null;
    }

}
