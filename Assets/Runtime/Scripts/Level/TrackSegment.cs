using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackSegment : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [SerializeField] private ObstacleSpawner[] obstacleSpawners;
    [SerializeField] private DecorationSpawner decorationSpawner;
    [SerializeField] private PickupLineSpawner[] pickupLineSpawners;
    [SerializeField, Range(0, 1)] private float pickupSpawnChance = 0.2f;

    public Transform Start => start;
    public Transform End => end;

    public float Length => Vector3.Distance(End.position, Start.position);
    public float SqrLength => (End.position - Start.position).sqrMagnitude;

    public void SpawnObstacles()
    {
        foreach (var obstacleSpawner in obstacleSpawners)
        {
            obstacleSpawner.SpawnObstacle();
        }
    }

    public void SpawnDecorations()
    {
        decorationSpawner.SpawnDecorations();
    }

    public void SpawnPickups()
    {
        if (pickupLineSpawners.Length > 0 && Random.value <= pickupSpawnChance)
        {
            var pickupLineSpawner = pickupLineSpawners[Random.Range(0, pickupLineSpawners.Length)];
            pickupLineSpawner.SpawnPickupLine(GetSkipPositions());
        }
    }

    private Vector3[] GetSkipPositions()
    {
        var skipPositions = new Vector3[obstacleSpawners.Length];
        
        for (var i = 0; i < skipPositions.Length; i++)
        {
            skipPositions[i] = obstacleSpawners[i].transform.position;
        }

        return skipPositions;
    }
}
