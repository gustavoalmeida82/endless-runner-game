using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 4.0f;
    [SerializeField] private float forwardSpeed = 10.0f;
    [SerializeField] private float laneDistanceX = 4.0f;
    
    [Header("Jump")] 
    [SerializeField] private float jumpDistanceZ = 4;
    [SerializeField] private float jumpHeightY = 2;
    [SerializeField] private float jumpAbortSpeed = 3;

    [Header("Roll")] 
    [SerializeField] private BoxCollider regularCollider;
    [SerializeField] private BoxCollider rollCollider;
    [SerializeField] private float rollDistanceZ = 4;

    private Vector3 _initialPosition;
    private float _targetPositionX;
    private float _jumpStartZ;
    private float _rollStartZ;
    
    private float LaneBoundRight => _initialPosition.x + laneDistanceX;
    private float LaneBoundLeft => _initialPosition.x - laneDistanceX;
    public float JumpDuration => jumpDistanceZ / forwardSpeed;
    public float RollDuration => rollDistanceZ / forwardSpeed;
    public bool IsJumping { get; private set; }
    public bool IsRolling { get; private set; }
    private bool IsGrounded => Mathf.Approximately(transform.position.y,_initialPosition.y);
    private bool WasJumpAborted => !IsJumping && !IsGrounded;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        ProcessInput();
        ProcessRollMovement();
        
        var position = transform.position;

        position.x = ProcessLaneMovement();
        position.y = ProcessJumpMovement();
        position.z = ProcessForwardMovement();
        
        transform.position = position;
    }

    private float ProcessLaneMovement()
    {
        return Mathf.MoveTowards(transform.position.x, _targetPositionX, horizontalSpeed * Time.deltaTime);
    }

    private float ProcessForwardMovement()
    {
        return transform.position.z + (forwardSpeed * Time.deltaTime);
    }

    private float ProcessJumpMovement()
    {
        var deltaY = 0f;

        if (IsJumping)
        {
            var currentJumpProgress = transform.position.z - _jumpStartZ;
            var jumpPercent = currentJumpProgress / jumpDistanceZ;

            if (jumpPercent >= 1)
            {
                StopJump();
            }
            else
            {
                deltaY = Mathf.Sin(Mathf.PI * jumpPercent) * jumpHeightY;
            }
        }

        if (WasJumpAborted)
        {
            deltaY = Mathf.MoveTowards(transform.position.y, _initialPosition.y, jumpAbortSpeed * Time.deltaTime);
        }

        return _initialPosition.y + deltaY;
    }

    private void ProcessRollMovement()
    {
        if (IsRolling)
        {
            var currentRollProgress = transform.position.z - _rollStartZ;
            var rollPercent = currentRollProgress / rollDistanceZ;

            if (rollPercent >= 1)
            {
                StopRoll();
            }
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _targetPositionX = transform.position.x + laneDistanceX;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _targetPositionX = transform.position.x - laneDistanceX;
        }

        if (Input.GetKeyDown(KeyCode.W) && !IsJumping)
        {
            StartJump();
        }

        if (Input.GetKeyDown(KeyCode.S) && !IsRolling)
        {
            StartRoll();
        }
        
        _targetPositionX = Mathf.Clamp(_targetPositionX, LaneBoundLeft, LaneBoundRight);
    }

    private void StartJump()
    {
        StopRoll();
        IsJumping = true;
        _jumpStartZ = transform.position.z;
    }
    
    private void StopJump()
    {
        IsJumping = false;
    }

    private void StartRoll()
    {
        StopJump();
        IsRolling = true;
        _rollStartZ = transform.position.z;
        rollCollider.enabled = true;
        regularCollider.enabled = false;
    }
    
    private void StopRoll()
    {
        IsRolling = false;
        rollCollider.enabled = false;
        regularCollider.enabled = true;
    }

    public void Die()
    {
        forwardSpeed = 0;
        horizontalSpeed = 0;
        StopJump();
        StopRoll();
    }
}
