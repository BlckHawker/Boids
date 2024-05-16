using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    private Obstacle obstaclePrefab;
    [NonSerialized]
    public List<Obstacle> ObstacleList = new List<Obstacle>();

    public void CreateObstacle()
    {

    }
}
