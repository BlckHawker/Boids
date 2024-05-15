using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    private Obstacle obstaclePrefab;
    private List<Obstacle> obstacleList = new List<Obstacle>();

    public void CreateObstacle()
    {

    }
}
