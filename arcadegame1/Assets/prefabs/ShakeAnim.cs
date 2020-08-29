using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnim : MonoBehaviour
{
    public Vector3[] m_waypoints;

    int m_currentWaypoint = 0;
    public float m_speed = 0.0002f;


    // Start is called before the first frame update
    void Start()
    {
        m_waypoints = new Vector3[3];
        m_waypoints[0] = new Vector3(transform.localScale.x + 0.08f, transform.localScale.y, 1f);
        m_waypoints[1] = transform.localScale;
        m_waypoints[2] = new Vector3(transform.localScale.x, transform.localScale.y + 0.08f, 1f);


    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.localScale, m_waypoints[m_currentWaypoint]) < 0.01f)
        {
            m_currentWaypoint++;
            m_currentWaypoint = m_currentWaypoint % m_waypoints.Length;
        }
        transform.localScale += Vector3.Normalize(m_waypoints[m_currentWaypoint] - transform.localScale) * m_speed * Time.deltaTime*0.05f;

    }
}
