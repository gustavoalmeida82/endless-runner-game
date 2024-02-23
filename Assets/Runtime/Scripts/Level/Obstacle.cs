using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private DecorationSpawner[] decorationSpawners;

    private List<ObstacleDecoration> obstacleDecorations = new();

    public void SpawnDecorations()
    {
        foreach (var decorationSpawner in decorationSpawners)
        {
            decorationSpawner.SpawnDecorations();
            var obstacleDecoration = decorationSpawner.CurrentDecoration.GetComponent<ObstacleDecoration>();
            if (obstacleDecoration != null)
            {
                obstacleDecorations.Add(obstacleDecoration);
            }
        }
    }

    public void PlayCollisionFeedback(Collider col)
    {
        var decorationHit = FindDecorationForCollider(col);
        if (decorationHit != null)
        {
            decorationHit.PlayCollisionFeedback();
        }
    }

    private ObstacleDecoration FindDecorationForCollider(Collider col)
    {
        var minDistX = Mathf.Infinity;
        ObstacleDecoration minDistDecoration = null;

        foreach (var decoration in obstacleDecorations)
        {
            var decorationPosX = decoration.transform.position.x;
            var colliderPosX = GetComponent<Collider>().bounds.center.x;
            var distX = Mathf.Abs(decorationPosX - colliderPosX);
            if (distX < minDistX)
            {
                minDistX = distX;
                minDistDecoration = decoration;
            }
        }

        return minDistDecoration;
    }
}
