using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField]
    private GameObject backgroundImage;


    public int NoOfLevels() {
        return _levels.noOfLevels;
    }

    public void generateLevel(int worldNo, int index) {
        Level level = _levels.levelLists[worldNo].getLevel(index);
        Debug.Log("generating - "+level.obstacles.Length);
        for (int i =0; i < level.obstacles.Length; i++) {
            Vector3 position = level.obstacles[i].position;

            GameObject obstacle = Instantiate(obstacles[level.obstacles[i].obstacleID], position, level.obstacles[i].rotation);
            obstacle.transform.localScale = level.obstacles[i].scale;

            if (level.obstacles[i].path.Length > 0)
            {
                obstacle.AddComponent<WaypointMovement>();
                obstacle.GetComponent<WaypointMovement>().m_waypoints= level.obstacles[i].path;
                obstacle.GetComponent<WaypointMovement>().m_speed = level.obstacles[i].speed;
            }
        }
        createBoundaries(level.borders);
        generateBackground(level.borders);
        Instantiate(finishPoint, level.finishPosition, Quaternion.identity);
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.RotSpeed = level.playerRotationSpeed;
        PlayerController1 playerController1 = GameObject.FindObjectOfType<PlayerController1>();
        playerController1.MoveSpeed = level.playerMovementSpeed;
    }

    public GameObject generateLevel(int index, float offset)
    {
        GameObject parentObj = new GameObject();
        parentObj.name = "index " + index;
        Level level = _levels.getLevel(index);
        Debug.Log("[generateLevel] - " + level.obstacles.Length);
        for (int i = 0; i < level.obstacles.Length; i++)
        {
            Vector3 position = level.obstacles[i].position + Vector3.up*offset;

            GameObject obstacle = Instantiate(obstacles[level.obstacles[i].obstacleID], position, level.obstacles[i].rotation);
            obstacle.transform.localScale = level.obstacles[i].scale;

            if (level.obstacles[i].path.Length > 0)
            {
                obstacle.AddComponent<WaypointMovement>();
                obstacle.GetComponent<WaypointMovement>().m_waypoints = level.obstacles[i].path;
                for(int w=0;w< level.obstacles[i].path.Length; w++)
                {
                    obstacle.GetComponent<WaypointMovement>().m_waypoints[w] += Vector3.up * offset;
                    Debug.Log(obstacle.GetComponent<WaypointMovement>().m_waypoints[w]+", "+ Vector3.up * offset);
                }
                obstacle.GetComponent<WaypointMovement>().m_speed = level.obstacles[i].speed;
            }
            obstacle.transform.parent = parentObj.transform;
        }
        Debug.Log("backgr - "+(level.borders + new Vector4(0, offset, 0f, 0f)));
        generateBackground(level.borders+new Vector4(0,offset+5f,0f,0f));
        return parentObj;
    }

    private void generateBackground(Vector4 area)
    {
        bool emptySpacePresent = true;
        area.x -= 10f;
        area.y -= 10f;
        area.z += 20f;
        area.w += 20f;

        List<Transform> positionsList = new List<Transform>();
        while (emptySpacePresent)
        {
            Vector3 newPos;
            float scale = Random.Range(5f,20f);
            newPos = new Vector3(Random.Range(area.x,area.x+area.z), Random.Range(area.y, area.y + area.w),0f);
            GameObject newImage = Instantiate(backgroundImage, newPos, Quaternion.identity);
            newImage.transform.localScale = new Vector3(scale,scale,1f);
            newImage.GetComponent<BackgroundImage>().m_spinningClockwise = Random.Range(0f, 1f) > 0.5f;
            PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
            newImage.GetComponent<BackgroundImage>().m_rotSpeed = Random.Range((playerController.RotSpeed/1.5f)-5f, (playerController.RotSpeed / 1.5f)+5f);
            int tries = 0;
            while (!isValidPosition(newImage, positionsList))
            {
                newPos = new Vector3(Random.Range(area.x, area.x + area.z), Random.Range(area.y, area.y + area.w),0f);
                newImage.transform.position = newPos;
                if (tries > 20)
                    break;
                tries++;
            }
            if (tries > 20)
            {
                emptySpacePresent = false;
                GameObject.Destroy(newImage);
            }
            else positionsList.Add(newImage.transform);
        }
    }

    bool isValidPosition(GameObject newPos, List<Transform> positionsList)
    {
        for(int i = 0; i < positionsList.Count; i++)
        {
            if (Vector3.Distance(newPos.transform.position, positionsList[i].transform.position) - newPos.transform.localScale.x/2 - positionsList[i].localScale.x/2 < 0f)
                return false;
        }
        return true;
    }

    public void createBoundaries(Vector4 rect) {
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
        BackgroundImage[] images = GameObject.FindObjectsOfType<BackgroundImage>();
        for (int i = 0; i < images.Length; i++) GameObject.Destroy(images[i].gameObject);

    }

    public float getLevelHeight(int levelNo)
    {
        return _levels.getLevel(levelNo).borders.w;
    }

    public bool isMovingLevel(int i)
    {
        if (_levels.getLevel(i).playerMovementSpeed > 1f || _levels.getLevel(i).allowedInInfinity)
            return true;
        return false;
    }

    public bool isMoving(int world, int i)
    {
        return world!=0;
        
    }

    public bool isNonMovingLevel(int i)
    {
        if (_levels.getLevel(i).allowedInInfinity)
            return true;
        return false;
    }

    public LevelList getWorldDetails(int worldNo)
    {
        return _levels.levelLists[worldNo];
    }
    public int getNoOfWorlds()
    {
        return _levels.levelLists.Capacity;
    }
    public int getNoOfLevels(int worldNo)
    {
        return _levels.levelLists[worldNo].noOfLevels;
    }
}
