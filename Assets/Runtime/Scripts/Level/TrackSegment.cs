using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private ObstacleSpawner[] obstacleSpawners;

    public Transform Start => start;
    public Transform End => end;

    public void SpawnObstacles()
    {
        foreach (var obstacleSpawner in obstacleSpawners)
        {
            obstacleSpawner.Spawn();
        }
    }
}
