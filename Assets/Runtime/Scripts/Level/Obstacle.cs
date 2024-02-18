using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private DecorationSpawner[] decorationSpawners;

    public void SpawnDecorations()
    {
        foreach (var decorationSpawner in decorationSpawners)
        {
            decorationSpawner.SpawnDecorations();
        }
    }
}
