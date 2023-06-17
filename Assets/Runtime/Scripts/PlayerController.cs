using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float laneWidth = 4;

    public float RightLaneBound => _initialPosition.x + laneWidth;
    public float LeftLaneBound => _initialPosition.x - laneWidth;

    private Vector3 _initialPosition;
    private float _targetPositionX;

    private void Awake() 
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        ProcessInput();

        var position = transform.position;
        position.x = ProcessLaneMovement();
        position.z = ProcessForwardMovement();

        transform.position = position;        
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _targetPositionX = transform.position.x + laneWidth;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _targetPositionX = transform.position.x - laneWidth;
        }

        _targetPositionX = Mathf.Clamp(_targetPositionX, LeftLaneBound, RightLaneBound);
    }

    private float ProcessLaneMovement()
    {
        return Mathf.MoveTowards(transform.position.x, _targetPositionX, horizontalSpeed * Time.deltaTime);
    }

    private float ProcessForwardMovement()
    {
        return transform.position.z + forwardSpeed * Time.deltaTime;
    }
}
