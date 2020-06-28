using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField]
    private LevelList _levels;
    [SerializeField]
    private GameObject[] obstacles;
    [SerializeField]
    private GameObject finishPoint;

    public int NoOfLevels() {
        return _levels.noOfLevels;
    }

    public void generateLevel(int index) {
        Level level = _levels.getLevel(index);
        Debug.Log("generating - "+level.obstacles.Length);
        Instantiate(finishPoint,level.finishPosition,Quaternion.identity);
        for (int i =0; i < level.obstacles.Length; i++) {
            GameObject obstacle = Instantiate(obstacles[level.obstacles[i].obstacleID], level.obstacles[i].position, level.obstacles[i].rotation);
            obstacle.transform.localScale = level.obstacles[i].scale;
        }
    }

    public void clearLevel() {
        GameObject finishObj = GameObject.FindGameObjectWithTag("finish");
        GameObject.Destroy(finishObj);
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        for(int i=0;i<obstacles.Length;i++) GameObject.Destroy(obstacles[i]);
        
    }
}
