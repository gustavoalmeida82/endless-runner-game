using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float armZ;

    private void LateUpdate()
    {
        var targetPosition = transform.position;
        targetPosition.z = player.transform.position.z - armZ;
        transform.position = targetPosition;
    }
}
