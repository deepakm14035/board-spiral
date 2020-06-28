using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//fix pixel length

public class PlayerController : MonoBehaviour
{
    public Transform pos1, pos2,curpos;
    public float movespeed, rotSpeed;
    public GameObject pivotEffectPrefab;
    public GameObject trailRenderer1;
    public GameObject trailRenderer2;

    float direction = -1f;
    UIEffects uiEffects;
    bool allowMoving;
    bool gameStarted = false;
    PlayerController1 pc;
    GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        uiEffects = GameObject.FindObjectOfType<UIEffects>();
        pc = GameObject.FindObjectOfType<PlayerController1>();
        setMoving(false);
        allowMoving = true;
        mainCamera = Camera.main.gameObject.transform.parent.gameObject;
        trailRenderer1.SetActive(true);
        trailRenderer2.SetActive(true);

    }

    public void setMoving(bool moving) {
        allowMoving = moving;
        if (pc != null)
            pc.allowMoving = moving;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.up*Time.deltaTime*movespeed;
        if(allowMoving&& gameStarted)
            transform.RotateAround(curpos.position,Vector3.forward, direction * 5f*Time.deltaTime*rotSpeed);
        if (Input.GetButtonDown("Fire1")&& allowMoving) {
            changePivot();
        }
        if (Input.GetButtonDown("Fire2")&& allowMoving)
        {
            changeDirection();
        }
        //Debug.Log(transform.rotation.eulerAngles);

    }

    public void resetPosition() {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        mainCamera.GetComponent<CameraFollow>().resetCamera();
        curpos = pos2;
        StartCoroutine(allowMovementAfterDelay());
    }

    IEnumerator allowMovementAfterDelay() {
        yield return new WaitForSeconds(1f);
        trailRenderer1.SetActive(true);
        trailRenderer2.SetActive(true);
        setMoving(true);
    }

    public void changeDirection() {
        if (!allowMoving)
            return;
        direction *= -1f;
    }

    public void changePivot()
    {
        if (!allowMoving)
            return;
        if (pos1 == curpos)
            curpos = pos2;
        else
            curpos = pos1;
        if (!gameStarted)
        {
            gameStarted = true;
            setMoving(true);
            return;
        }
        Instantiate(pivotEffectPrefab,curpos.transform.position,Quaternion.identity);
        StartCoroutine(pivotChange());

    }

    IEnumerator pivotChange()
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
            uiEffects.playLoseAnimation();
            setMoving(false);
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            uiEffects.disableObjects();
            trailRenderer1.SetActive(false);
            trailRenderer2.SetActive(false);
            menuManager.loadMenu(LoseMenu.Instance);
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
