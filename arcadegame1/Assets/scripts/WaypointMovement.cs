using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    public Vector3[] m_waypoints;

    int m_currentWaypoint=0;
    public float m_speed = 8f;

    public bool m_stopMoving = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_stopMoving)
        {
            if (Vector3.Distance(transform.position, m_waypoints[m_currentWaypoint]) < 0.2f)
            {
                m_currentWaypoint++;
                m_currentWaypoint = m_currentWaypoint % m_waypoints.Length;
            }
            else
                transform.position += Vector3.Normalize(m_waypoints[m_currentWaypoint] - transform.position) * m_speed * Time.deltaTime;
        }
    }
}
