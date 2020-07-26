using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//fix pixel length

public class PlayerController : MonoBehaviour
{
    public Transform pos1, pos2,curpos;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject pivotEffectPrefab;
    [SerializeField] private GameObject trailRenderer1;
    [SerializeField] private GameObject trailRenderer2;
    [SerializeField] private GameObject UIButtonLeft, UIButtonRight;

    GameManager _gameManager;

    float direction = -1f;
    UIEffects uiEffects;
    bool allowMoving;
    bool gameStarted = false;
    PlayerController1 pc;
    GameObject mainCamera;

    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        uiEffects = GameObject.FindObjectOfType<UIEffects>();
        pc = GameObject.FindObjectOfType<PlayerController1>();
        setMoving(false);
        allowMoving = true;
        mainCamera = Camera.main.gameObject.transform.parent.gameObject;
        trailRenderer1.SetActive(false);
        trailRenderer2.SetActive(false);
        if (SystemInfo.deviceType.Equals(DeviceType.Desktop))
        {
            UIButtonLeft.SetActive(false);
            UIButtonRight.SetActive(false);
        }

        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void setMoving(bool moving) {
        allowMoving = moving;
        if (pc != null)
            pc.allowMoving = moving;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMoving&& gameStarted)
            transform.RotateAround(curpos.position,Vector3.forward, direction * 5f*Time.deltaTime*RotSpeed);
        if (Input.GetButtonDown("Fire1")&& allowMoving) {
            if(!SystemInfo.deviceType.Equals(DeviceType.Handheld))
            changePivot();
        }
        if (Input.GetButtonDown("Fire2")&& allowMoving)
        {
            if (!SystemInfo.deviceType.Equals(DeviceType.Handheld))
                changeDirection();
        }

    }

    public void resetPosition() {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        mainCamera.GetComponent<CameraFollow>().resetCamera();
        curpos = pos1;
        trailRenderer2.SetActive(true);
        gameStarted = true;

        StartCoroutine(allowMovementAfterDelay());
    }

    IEnumerator allowMovementAfterDelay() {
        yield return new WaitForSeconds(1f);
        //trailRenderer1.SetActive(true);
        //trailRenderer2.SetActive(true);
        setMoving(true);
    }

    public void changeDirection() {
        Debug.Log("chn");
        if (!allowMoving)
            return;
        direction *= -1f;
    }

    public void changePivot()
    {
        if (!allowMoving ||_gameManager.isPositionOutOfBounds(pos1 == curpos ? pos2.transform.position : pos1.transform.position))
            return;
        if (pos1 == curpos)
        {
            curpos = pos2;
            trailRenderer1.SetActive(true);
            trailRenderer2.SetActive(false);
        }
        else
        {
            curpos = pos1;
            trailRenderer2.SetActive(true);
            trailRenderer1.SetActive(false);
        }
        if (!gameStarted)
        {
            gameStarted = true;
            setMoving(true);
            return;
        }
        Instantiate(pivotEffectPrefab,curpos.transform.position,Quaternion.identity);
        StartCoroutine(pivotChangeDelay());

    }

    IEnumerator pivotChangeDelay()
    {
        setMoving(false);
        yield return new WaitForSeconds(0.2f);
        setMoving(true);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("obstacle"))
        {
            mainCamera.GetComponent<Animator>().SetTrigger("shake");
            _gameManager.stopAllObstacles();
            uiEffects.playLoseAnimation();
            setMoving(false);
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            uiEffects.disableObjects();
            trailRenderer1.SetActive(false);
            trailRenderer2.SetActive(false);
            menuManager.loadMenu(LoseMenu.Instance,3f);
        }
        if (collision.gameObject.tag.Equals("finish"))
        {
            uiEffects.playWinAnimation();
            setMoving(false);
            if (pc != null)
                pc.allowMoving = false;
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            uiEffects.disableObjects();
            trailRenderer1.SetActive(false);
            trailRenderer2.SetActive(false);
            menuManager.loadMenu(WinMenu.Instance,3f);


        }
    }



}
