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

    public void CreateObstacle(Vector3 position)
    {
        Obstacle prefab = Instantiate(obstaclePrefab, position, Quaternion.identity);
        ObstacleList.Add(prefab);
    }

    public void ClearObstacles()
    {
        foreach (Obstacle obstacle in ObstacleList)
            Destroy(obstacle.gameObject);
        ObstacleList.Clear();
    }
}
