using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    public bool m_spinningClockwise = true;
    public float m_rotSpeed=5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_spinningClockwise)
        {
            transform.Rotate(0f,0f, m_rotSpeed * Time.deltaTime);
        }
        else
            transform.Rotate(0f, 0f, -m_rotSpeed*Time.deltaTime);
    }
}
