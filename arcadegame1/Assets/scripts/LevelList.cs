using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "levelList", menuName = "levels/Create LevelList", order = 1)]
public class LevelList : ScriptableObject
{
    [SerializeField]
    private List<Level> levelList;
    public int noOfLevels => levelList.Count;

    // Start is called before the first frame update
    public Level getLevel(int index)
    {
        return levelList[index];
    }
}
