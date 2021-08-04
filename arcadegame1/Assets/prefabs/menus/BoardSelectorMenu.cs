using MenuManagement.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class BoardSelectorMenu : Menu<BoardSelectorMenu>
    {
        [SerializeField]
        private GameObject[] rows;
        [SerializeField]
        private GameObject lockIcon;
        [SerializeField]
        private GameObject buyButton;
        [SerializeField]
        private GameObject constraintPanel;
        [SerializeField]
        private Text minScoreText;
        [SerializeField]
        private Text title;
        [SerializeField]
        ScrollRect scrollRect;
        public int boardsPerRow = 2;
        int selectedBoard;
        int selectedBG;
        public Text totalCoins;
        GameObject[] buttons;
        GameManager gameManager;

        bool isBoardView;

        void clearRows() {
            for(int i = 0; i < rows.Length; i++)
            {
                Transform[] transforms = rows[i].GetComponentsInChildren<Transform>();
                for (int j = 0; j < transforms.Length; j++)
                    if (transforms[j] != null && j > 0)
                        GameObject.Destroy(transforms[j].gameObject);
            }
        }
        
        public void setup(bool setupBoards)
        {
            clearRows();
            isBoardView = setupBoards;
            scrollRect.normalizedPosition = new Vector2(0, 1);
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();
            if (!setupBoards)
            {
                setupBackgrounds();
                title.text = "BUY PATTERN";
                return;
            }
            title.text = "BUY BOARD";
            SaveData playerInfo = gameManager.getPlayerData(true);
            GameObject[] boardList = gameManager._BoardList;
            buttons = new GameObject[boardList.Length];
            for (int i = 0; i < boardList.Length; i++)
            {
                GameObject board = Instantiate(boardList[i],rows[i/boardsPerRow].transform);
                board.transform.localScale = new Vector3(0.3f,0.3f,1f);
                board.name = "board" + i;
                GameObject button = null;
                if (!boardQualified(boardList[i].GetComponent<Board>(), playerInfo.maxScore))
                {
                    hideBoard(board);
                    lockBoard(board, i);
                    playerInfo.purchasedBoards[i] = 0;
                }
                else if (boardQualified(boardList[i].GetComponent<Board>(), playerInfo.maxScore) && playerInfo.purchasedBoards[i] == 0)
                    playerInfo.purchasedBoards[i] = 1;

                if (playerInfo.purchasedBoards[i] != 0 || playerInfo.selectedBoard == i)
                {
                    button = Instantiate(buyButton, board.transform);
                    addEventListener(button.GetComponent<Button>(), i, playerInfo.purchasedBoards[i]);
                    if (playerInfo.purchasedBoards[i] == 2)
                        button.GetComponentInChildren<Text>().text = "USE";
                    else
                        button.GetComponentInChildren<Text>().text = boardList[i].GetComponent<Board>().cost + "";

                }
                //setButtonColor(button);
                //button.transform.parent = board.transform;
                // button.GetComponent<Image>().color = new Color32(255,255,255,255);
                // button.GetComponentInChildren<Image>().color = new Color32(220,60,60,255);
                if (playerInfo.selectedBoard == i)
                    button.SetActive(false);
                Debug.Log("board id - "+i+", status - "+ playerInfo.purchasedBoards[i]);
                if (playerInfo.purchasedBoards[i] == 2 && playerInfo.selectedBoard == i)
                    selectedBoard = i;
                buttons[i] = button;
            }
        }

        public void setupBackgrounds()
        {
            SaveData playerInfo = gameManager.getPlayerData(true);
            GameObject[] bgList = gameManager._BackgroundList;
            buttons = new GameObject[bgList.Length];
            for (int i = 0; i < bgList.Length; i++)
            {
                GameObject bg = Instantiate(bgList[i], rows[i / boardsPerRow].transform);
                bg.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                bg.name = "bg" + i;
                GameObject button = null;
                if (!boardQualified(bgList[i].GetComponent<Board>(), playerInfo.maxScore))
                {
                    hideBoard(bg);
                    lockBoard(bg, i);
                    playerInfo.purchasedBackgrounds[i] = 0;
                }
                else if (boardQualified(bgList[i].GetComponent<Board>(), playerInfo.maxScore) && playerInfo.purchasedBackgrounds[i] == 0)
                    playerInfo.purchasedBackgrounds[i] = 1;

                if (playerInfo.purchasedBackgrounds[i] != 0 || playerInfo.selectedBackground == i)
                {
                    button = Instantiate(buyButton, bg.transform);
                    addEventListener(button.GetComponent<Button>(), i, playerInfo.purchasedBackgrounds[i]);
                    if (playerInfo.purchasedBackgrounds[i] == 2)
                        button.GetComponentInChildren<Text>().text = "USE";
                    else
                        button.GetComponentInChildren<Text>().text = bgList[i].GetComponent<Board>().cost + "";

                }
                //setButtonColor(button);
                //button.transform.parent = board.transform;
                // button.GetComponent<Image>().color = new Color32(255,255,255,255);
                // button.GetComponentInChildren<Image>().color = new Color32(220,60,60,255);
                if (playerInfo.selectedBackground == i)
                    button.SetActive(false);
                if (playerInfo.purchasedBackgrounds[i] == 2 && playerInfo.selectedBackground == i)
                    selectedBG = i;
                buttons[i] = button;
            }
        }

        void addEventListener(Button button, int i, int state)
        {
            Debug.Log("adding event - "+state);
            if (state == 1)
            {
                button.onClick.AddListener(delegate
                {
                    Debug.Log("buying..");
                    if (GameObject.FindObjectOfType<GameManager>().buyBoard(i))
                    {
                        Debug.Log("purchased");
                        if(GameObject.Find("lock" + i))
                            GameObject.Find("lock" + i).SetActive(false);
                        button.GetComponentInChildren<Text>().text = "USE";
                        Debug.Log("button coin info - " + (button.gameObject.transform.Find("coinIcon") ==null));
                        button.gameObject.transform.Find("coinIcon").gameObject.SetActive(false);
                        addUseBoardListener(button, i);
                    }
                });
            }else if (state == 2)
            {
                button.GetComponentInChildren<Text>().text = "USE";
                button.gameObject.transform.Find("coinIcon").gameObject.SetActive(false);
                addUseBoardListener(button, i);
            }
        }

        bool boardQualified(Board b, int maxScore)
        {
            return b.minScore <= maxScore;
        }

        void addUseBoardListener(Button button, int i) {
            button.onClick.AddListener(delegate {
                GameManager gameManager = FindObjectOfType<GameManager>();
                SaveData playerInfo = gameManager.getPlayerData(true);
                int selectedObj = 0;
                if (isBoardView)
                    selectedObj = playerInfo.selectedBoard;
                else
                    selectedObj = playerInfo.selectedBackground;
                buttons[selectedObj].SetActive(true);
                buttons[selectedObj].GetComponentInChildren<Text>().text = "USE";
                addEventListener(buttons[selectedObj].GetComponent<Button>(), selectedObj, 2);
                if(isBoardView)
                    playerInfo.selectedBoard = i;
                else
                    playerInfo.selectedBackground = i;
                gameManager.savePlayerData();
                if (isBoardView)
                    reloadBoard();
                else
                    reloadBackground();
                button.gameObject.SetActive(false);
                //button.gameObject.transform.Find("coinImage").gameObject.SetActive(false);
            });
        }

        public void lockBoard(GameObject board, int i)
        {
            hideBoard(board);
            GameObject lockIconInstance = Instantiate(lockIcon,board.transform);
            lockIconInstance.name = "lock" + i;
            lockIconInstance.GetComponent<Button>().onClick.AddListener(delegate {
                StartCoroutine(showConstraint(board.GetComponent<Board>()));
            });
        }

        IEnumerator showConstraint(Board board) {
            if (!constraintPanel.activeSelf)
            {
                minScoreText.text = board.minScore + "";
                constraintPanel.SetActive(true);
               // MaskableGraphic[] arr = { constraintPanel.GetComponent<MaskableGraphic>() };
                //SceneTransitionUtil.fadeObjects(arr, 0.5f, 1f);
                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(1f);
                //SceneTransitionUtil.fadeObjects(arr, 2f, 0f);
                //yield return new WaitForSeconds(2f);
                constraintPanel.SetActive(false);
            }
        }

        public void hideBoard(GameObject board)
        {
            Image[] images = board.GetComponentsInChildren<Image>();
            for(int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color32(255,255,255,122);
            }
        }

        public void reloadBoard()
        {
            gameManager.updatePlayerBoard();
        }
        public void reloadBackground()
        {
            gameManager.updatePlayerBG();
        }
        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

    }
}