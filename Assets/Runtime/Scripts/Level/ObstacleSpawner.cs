using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstaclesPrefabs;

    private Obstacle currentObstacle;

    public void Spawn()
    {
        var index = Random.Range(0, obstaclesPrefabs.Length);
        currentObstacle = Instantiate(obstaclesPrefabs[index], transform);
        currentObstacle.transform.localPosition = Vector3.zero;
        currentObstacle.transform.rotation = Quaternion.identity;
    }
}
