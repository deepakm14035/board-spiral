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
    [SerializeField]
    private Material borderMaterial;

    public int NoOfLevels() {
        return _levels.noOfLevels;
    }

    public void generateLevel(int index) {
        Level level = _levels.getLevel(9);
        Debug.Log("generating - "+level.obstacles.Length);
        Instantiate(finishPoint,level.finishPosition,Quaternion.identity);
        for (int i =0; i < level.obstacles.Length; i++) {
            GameObject obstacle = Instantiate(obstacles[level.obstacles[i].obstacleID], level.obstacles[i].position, level.obstacles[i].rotation);
            obstacle.transform.localScale = level.obstacles[i].scale;

            if (level.obstacles[i].path.Length > 0)
            {
                obstacle.AddComponent<WaypointMovement>();
                obstacle.GetComponent<WaypointMovement>().m_waypoints= level.obstacles[i].path;
                obstacle.GetComponent<WaypointMovement>().m_speed = level.obstacles[i].speed;
            }
        }

        createBoundaries(level.borders);
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.RotSpeed = level.playerRotationSpeed;
        PlayerController1 playerController1 = GameObject.FindObjectOfType<PlayerController1>();
        playerController1.MoveSpeed = level.playerMovementSpeed;

    }

    private void createBoundaries(Vector4 rect) {
        GameObject lineObj = new GameObject();
        lineObj.tag = "borders";
        LineRenderer lines =  lineObj.AddComponent<LineRenderer>();
        lines.positionCount = 4;
        lines.SetPosition(0,new Vector3(rect.x,rect.y,1f));
        lines.SetPosition(1, new Vector3(rect.x, rect.y + rect.w, 1f));
        lines.SetPosition(2, new Vector3(rect.x + rect.z, rect.y + rect.w, 1f));
        lines.SetPosition(3, new Vector3(rect.x + rect.z, rect.y, 1f));
        lines.material = borderMaterial;
        lines.loop = true;

    }

    public void clearLevel() {
        GameObject finishObj = GameObject.FindGameObjectWithTag("finish");
        GameObject.Destroy(finishObj);
        GameObject boundaryObj = GameObject.FindGameObjectWithTag("borders");
        GameObject.Destroy(boundaryObj);

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        for(int i=0;i<obstacles.Length;i++) GameObject.Destroy(obstacles[i]);
        
    }
}
