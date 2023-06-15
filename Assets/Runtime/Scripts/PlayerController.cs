using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private float forwardSpeed = 10;

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;
        }

        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    }
}
