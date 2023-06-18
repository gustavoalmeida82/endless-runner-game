using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float laneWidth = 4;
    [SerializeField] private float jumpMaxHeight = 4;
    [SerializeField] private float jumpMaxDistance = 4;

    public float RightLaneBound => _initialPosition.x + laneWidth;
    public float LeftLaneBound => _initialPosition.x - laneWidth;

    public bool IsJumping { get; private set; }

    private Vector3 _initialPosition;
    private float _targetPositionX;
    private float _jumpStartZ;

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
        position.y = ProcessJumpMovement();

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

        if (Input.GetKeyDown(KeyCode.W) && !IsJumping)
        {
           IsJumping = true;
           _jumpStartZ = transform.position.z;
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

    private float ProcessJumpMovement()
    {
        float deltaY = 0;

        if (IsJumping)
        {
            var jumpCurrentProgress = transform.position.z - _jumpStartZ;
            var jumpPercent = jumpCurrentProgress / jumpMaxDistance;

            if (jumpPercent < 1)
            {
                deltaY = Mathf.Sin(Mathf.PI * jumpPercent) * jumpMaxHeight;
            }
            else
            {
                IsJumping = false;
            }
        }

        return _initialPosition.y + deltaY;
    }

}
