using UnityEngine;

public class PickupLineSpawner : MonoBehaviour
{
    [SerializeField] private Pickup pickupPrefab;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private int spaceBetweenPickups = 2;

    public void SpawnPickupLine(Vector3[] skipPositions)
    {
        var currentSpawnPosition = start.position;
        while (currentSpawnPosition.z < end.position.z)
        {
            if (!ShouldSkipPosition(currentSpawnPosition, skipPositions))
            {
                var pickup = Instantiate(pickupPrefab, currentSpawnPosition, Quaternion.identity, transform);
            }
            currentSpawnPosition.z += spaceBetweenPickups;
        }
    }

    private bool ShouldSkipPosition(Vector3 currentSpawnPosition, Vector3[] skipPositions)
    {
        foreach (var skipPosition in skipPositions)
        {
            var skipStart = skipPosition.z - spaceBetweenPickups * 0.5f;
            var skipEnd = skipPosition.z + spaceBetweenPickups * 0.5f;

            if (currentSpawnPosition.z >= skipStart && currentSpawnPosition.z <= skipEnd)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        var currentSpawnPosition = start.position;
        while (currentSpawnPosition.z < end.position.z)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(currentSpawnPosition, Vector3.one);
            currentSpawnPosition.z += spaceBetweenPickups;
        }
    }
}
