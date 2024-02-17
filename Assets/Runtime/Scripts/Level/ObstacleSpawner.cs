using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstacles;

    private Obstacle currentObstacle;

    public void Spawn()
    {
        var index = Random.Range(0, obstacles.Length);
        currentObstacle = Instantiate(obstacles[index], transform);
        currentObstacle.transform.localPosition = Vector3.zero;
        currentObstacle.transform.rotation = Quaternion.identity;
    }
}
