using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelecterMenu : Menu<LevelSelecterMenu>
{
    [SerializeField]
    private GameObject _levelButtonPrefab;
    [SerializeField]
    private GameObject _levelListPanel;
    [SerializeField]
    private GameObject[] _levelListRows;
    [SerializeField]
    private Text _worldName;

    int currentWorldNo = 0;

    private int _buttonsPerRow=4;

    void generateLevelSelecter(int worldNo)
    {
        clearButtons();
        GenerateLevel levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        LevelList levels = levelGenerator.getWorldDetails(worldNo);
        _worldName.text = levels.getLevelsName();
        int rowNo = 0;
        for (int i = 0; i < levels.noOfLevels; i++){
            Debug.Log("rowno-"+rowNo);
            Button newButton = Instantiate(_levelButtonPrefab, _levelListRows[rowNo].transform).GetComponent<Button>();
            Debug.Log(newButton);
            newButton.GetComponentInChildren<Text>().text=""+ (i+1);
            addEventListener(newButton, worldNo, i, gameManager.getLevelProgress(worldNo, i));
            if (gameManager.getLevelProgress(worldNo, i) == 2)
                newButton.gameObject.GetComponent<Image>().color = new Color32(50,190,88,255);
            if (gameManager.getLevelProgress(worldNo, i) == 0)
            {
                newButton.gameObject.GetComponent<Image>().color = new Color32(190, 50, 88, 255);
                newButton.gameObject.GetComponentInChildren<Text>().color = new Color32(160, 160, 160, 255);
            }
            if (i == (rowNo * _buttonsPerRow + (_buttonsPerRow-1)))
                rowNo++;
        }
        currentWorldNo = worldNo;
    }

    void addEventListener(Button button, int worldNo, int i, int type) {
        //if(type==1||type==2)
            button.onClick.AddListener(delegate {
                GameObject.FindObjectOfType<MenuManager>().loadMenu(GameMenu.Instance);
                Debug.Log("w" + worldNo + ", i-" + i);
                int i1 = i;
                GameObject.FindObjectOfType<GameManager>().startLevel(worldNo, i1);
            });

    }

    void clearButtons()
    {
        for (int i = 0; i < _levelListRows.Length; i++)
        {
            int len = _levelListRows[i].GetComponentsInChildren<Button>().Length;
            for (int j = 0; j < len; j++)
            {
                GameObject.Destroy(_levelListRows[i].GetComponentsInChildren<Button>()[j].gameObject);
            }
        }
    }

    public void nextList()
    {
        if (checkWorldNo(currentWorldNo+1))
            currentWorldNo++;
        Debug.Log("currworl-" + currentWorldNo);
        generateLevelSelecter(currentWorldNo);
    }
    public void prevList()
    {
        if (checkWorldNo(currentWorldNo - 1))
            currentWorldNo--;
        generateLevelSelecter(currentWorldNo);
    }

    bool checkWorldNo(int num) {
        GenerateLevel levelGenerator = GameObject.FindObjectOfType<GenerateLevel>();
        Debug.Log(num < 0 || num >= levelGenerator.getNoOfWorlds());
        if (num < 0 || num >= levelGenerator.getNoOfWorlds())
            return false;
        return true;

    }
    // Start is called before the first frame update
    public void setup()
    {
        generateLevelSelecter(0);
    }

}
