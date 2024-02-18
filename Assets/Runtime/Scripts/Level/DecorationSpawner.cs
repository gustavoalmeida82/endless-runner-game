using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] decorationOptions;

    private GameObject currentDecoration;

    public void SpawnDecorations()
    {
        GameObject prefab = decorationOptions[Random.Range(0, decorationOptions.Length)];
        currentDecoration = Instantiate(prefab, transform);
        currentDecoration.transform.localPosition = Vector3.zero;
        currentDecoration.transform.rotation = Quaternion.identity;
    }
}
