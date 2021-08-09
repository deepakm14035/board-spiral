using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public struct Obstacle {
        int type;
        Vector3 scale;
    };

    public enum GameMode { InfinityMode,LevelMode, None};

    [SerializeField]
    private GameObject _HowToPlayPanel1;
    [SerializeField]
    private GameObject _HowToPlayPanel1Target;
    [SerializeField]
    private GameObject _HowToPlayPanel2;
    [SerializeField]
    private GameObject _GoodJobPanel2;
    [SerializeField]
    private GameObject _HowToPlayPanel2Target;
    [SerializeField]
    private Obstacle _HowToPlayPanel2Obstacle;
    [SerializeField]
    private GameObject _TutorialCompleteText;
    [SerializeField]
    private GameObject _targetArea;
    private Vector3 _lastPosition;
    private float _lastDirection;
    private int _lastActivePivot;

    PlayerController _playerController;
    PlayerController1 _playerController1;
    int tutorialProgress = -1;

    public bool isPlaying()
    {
        return tutorialProgress != -1;
    }

    public void resetTutorial()
    {
        tutorialProgress = -1;
        _HowToPlayPanel1.SetActive(false);
        _HowToPlayPanel1Target.SetActive(false);
        _GoodJobPanel2.SetActive(false);
        _HowToPlayPanel2.SetActive(false);
        _HowToPlayPanel2Target.SetActive(false);
        _TutorialCompleteText.SetActive(false);
    }

    public IEnumerator Replay()
    {
        Debug.Log("replaying");
        yield return new WaitForSeconds(1f);
        _playerController.gameObject.transform.position = _lastPosition;
        _playerController.setPivots(_lastActivePivot==1, _lastActivePivot!=1);
        if((_playerController.getDirection()>0f && _lastDirection<0f) ||
            (_playerController.getDirection() < 0f && _lastDirection > 0f))
            _playerController.changeDirection();
        _playerController.setMoving(true);
        yield return null;
    }

    public void setLastDirection(int dir)
    {
        _lastDirection = dir;
    }
    public void setLastPosition(Vector3 pos)
    {
        _lastPosition = pos;
    }

    public IEnumerator playTutorial(GameMode mode)
    {
        tutorialProgress = 0;
        _playerController1.allowMoving = false;
        //gameStarted = false;
        yield return new WaitForSeconds(2f);
        _HowToPlayPanel1.SetActive(true);
        _HowToPlayPanel1.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(5f);
        _HowToPlayPanel1.GetComponent<CanvasGroup>().alpha=0.5f;
        tutorialProgress = 1;
        _HowToPlayPanel1Target.SetActive(true);

        while (tutorialProgress == 1) yield return null;
        _HowToPlayPanel1.SetActive(false);

        _HowToPlayPanel1Target.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _GoodJobPanel2.SetActive(true);
        yield return new WaitForSeconds(2f);
        _GoodJobPanel2.SetActive(false);
        _HowToPlayPanel2.SetActive(true);
        _HowToPlayPanel2.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(5f);
        _HowToPlayPanel2.GetComponent<CanvasGroup>().alpha = 0.5f;
        _HowToPlayPanel2Target.SetActive(true);

        while (tutorialProgress == 2) yield return null;

        _HowToPlayPanel2.SetActive(false);
        //isPlayingTutorial = false;
        _TutorialCompleteText.SetActive(true);
        _HowToPlayPanel2Target.SetActive(false);
        yield return new WaitForSeconds(3f);
        _TutorialCompleteText.SetActive(false);
        //gameStarted = true;
        _playerController.resetPosition(true, true, true);
        //_playerController1.allowMoving = true;
        Debug.Log("Mode-"+mode.ToString());
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.tutorialComplete();
        tutorialProgress=-1;
        if (mode == GameMode.InfinityMode)
        {
            gameManager.loadInfinityMode(false);
        }
        else if(mode == GameMode.LevelMode)
        {
            //GameObject.FindObjectOfType<GameManager>().startLevel(0, 0);
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(WinMenu.Instance, 1f, false);
        }
        else
        {
            PauseMenu.Instance.GoHome();
        }
    }

    public void updateProgress(int pivot, float direction, Vector3 position)
    {
        tutorialProgress++;
        _lastActivePivot = pivot;
        _lastDirection = direction;
        _lastPosition = position;

    }
    // Start is called before the first frame update
    void Start()
    {
        tutorialProgress = -1;
        _HowToPlayPanel1.SetActive(false);
        _HowToPlayPanel1Target.SetActive(false);
        _HowToPlayPanel2.SetActive(false);
        _HowToPlayPanel2Target.SetActive(false);
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerController1 = GameObject.FindObjectOfType<PlayerController1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
