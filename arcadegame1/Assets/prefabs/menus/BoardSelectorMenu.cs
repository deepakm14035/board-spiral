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
        public int boardsPerRow = 2;
        int selectedBoard;
        public Text totalCoins;
        GameObject[] buttons;
        GameManager gameManager;
        public void setup()
        {
            if(gameManager==null)
                gameManager= FindObjectOfType<GameManager>();
            SaveData playerInfo = gameManager.getPlayerData(true);
            GameObject[] boardList = gameManager._BoardList;
            buttons = new GameObject[boardList.Length];
            for (int i = 0; i < boardList.Length; i++)
            {
                GameObject board = Instantiate(boardList[i],rows[i/boardsPerRow].transform);
                board.transform.localScale = new Vector3(0.3f,0.3f,1f);
                board.name = "board" + i;
                GameObject button = null;
                if (playerInfo.purchasedBoards[i] == 1)
                    hideBoard(board);
                if (boardList[i].GetComponent<Board>().minScore > playerInfo.maxScore && playerInfo.purchasedBoards[i] != 2)
                    lockBoard(board, i);
                else
                    playerInfo.purchasedBoards[i] = 1;


                button = Instantiate(buyButton, board.transform);
                addEventListener(button.GetComponent<Button>(), i, playerInfo.purchasedBoards[i]);
                button.GetComponentInChildren<Text>().text = boardList[i].GetComponent<Board>().cost + "";
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
                button.gameObject.transform.Find("coinImage").gameObject.SetActive(false);
                addUseBoardListener(button, i);
            }
        }

        void addUseBoardListener(Button button, int i) {
            button.onClick.AddListener(delegate {
                GameManager gameManager = FindObjectOfType<GameManager>();
                SaveData playerInfo = gameManager.getPlayerData(true);
                buttons[playerInfo.selectedBoard].SetActive(true);
                playerInfo.selectedBoard = i;
                gameManager.savePlayerData();
                reloadBoard();
                button.gameObject.SetActive(false);
                //button.gameObject.transform.Find("coinImage").gameObject.SetActive(false);
            });
        }

        public void lockBoard(GameObject board, int i)
        {
            hideBoard(board);
            GameObject lockIconInstance = Instantiate(lockIcon,board.transform);
            lockIconInstance.name = "lock" + i;
        }

        public void hideBoard(GameObject board)
        {
            Image[] images = board.GetComponentsInChildren<Image>();
            for(int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color32(0,0,0,255);
            }
        }

        public void reloadBoard()
        {
            gameManager.updatePlayerBoard();
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

    }
}