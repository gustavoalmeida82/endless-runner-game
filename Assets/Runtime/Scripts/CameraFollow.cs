using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float armZLength = 7;

    private void LateUpdate() 
    {
        var targetPosition = transform.position;
        targetPosition.z = player.transform.position.z - armZLength;
        transform.position = targetPosition;
    }
}
