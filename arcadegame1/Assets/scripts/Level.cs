using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    [Serializable]
    public class Obstacle
    {
        [SerializeField]
        public int obstacleID;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public Vector3 scale;
        [SerializeField]
        public Quaternion rotation;
    }
    [SerializeField]
    protected Obstacle[] _obstacles;
    [SerializeField]
    protected Vector3 _finishPosition;
    [SerializeField]
    protected Vector4 _borders;

    public Obstacle[] obstacles => _obstacles;
    public Vector3 finishPosition => _finishPosition;
    public Vector4 borders=> _borders;

}
