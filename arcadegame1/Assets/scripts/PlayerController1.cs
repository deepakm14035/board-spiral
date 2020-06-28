using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float movespeed;

    public bool allowMoving;
    // Start is called before the first frame update
    void Start()
    {
        allowMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMoving)
            transform.position += Vector3.up * Time.deltaTime * movespeed;
        


    }
}
