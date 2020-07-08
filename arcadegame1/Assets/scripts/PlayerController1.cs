using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private float _moveSpeed;

    public bool allowMoving;

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        allowMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMoving)
            transform.position += Vector3.up * Time.deltaTime * MoveSpeed;
        


    }
}
