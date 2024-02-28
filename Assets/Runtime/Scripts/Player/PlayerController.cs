using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerAudioController playerAudioController;
    
    [Header("Ground")]
    [SerializeField] private float horizontalSpeed = 15;
    [SerializeField] private float laneDistanceX = 4;

    [Header("Jump")]
    [SerializeField] private float jumpDistanceZ = 5;
    [SerializeField] private float jumpHeightY = 2;
    [SerializeField] private float jumpLerpSpeed = 10;

    [Header("Roll")]
    [SerializeField] private float rollDistanceZ = 5;
    [SerializeField] private Collider regularCollider;
    [SerializeField] private Collider rollCollider;

    private Vector3 _initialPosition;
    private float _targetPositionX;
    private float _rollStartZ;
    private float _jumpStartZ;
    private bool _isDead;

    public bool IsJumping { get; private set; }
    public bool IsRolling { get; private set; }
    public float ForwardSpeed { get; set; }

    public float JumpDuration => jumpDistanceZ / ForwardSpeed;
    public float RollDuration => rollDistanceZ / ForwardSpeed;
    private float LeftLaneX => _initialPosition.x - laneDistanceX;
    private float RightLaneX => _initialPosition.x + laneDistanceX;

    private bool CanJump => !IsJumping;
    private bool CanRoll => !IsRolling;
    private bool IsGrounded => Mathf.Approximately(transform.position.y, _initialPosition.y);
    public float TravelledDistance => Vector3.Distance(transform.position, _initialPosition);

    private void Awake()
    {
        _initialPosition = transform.position;
        StopRoll();
    }

    private void Update()
    {
        ProcessInput();

        var position = transform.position;

        position.x = ProcessLaneMovement();
        position.y = ProcessJump();
        position.z = ProcessForwardMovement();
        ProcessRoll();

        transform.position = position;
    }

    private void ProcessInput()
    {
        if (_isDead) return;
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _targetPositionX += laneDistanceX;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _targetPositionX -= laneDistanceX;
        }
        
        if (Input.GetKeyDown(KeyCode.W) && CanJump)
        {
            StartJump();
        }
        
        if (Input.GetKeyDown(KeyCode.S) && CanRoll)
        {
            StartRoll();
        }

        _targetPositionX = Mathf.Clamp(_targetPositionX, LeftLaneX, RightLaneX);
    }

    private float ProcessLaneMovement()
    {
        return Mathf.Lerp(transform.position.x, _targetPositionX, Time.deltaTime * horizontalSpeed);
    }

    private float ProcessForwardMovement()
    {
        return transform.position.z + ForwardSpeed * Time.deltaTime;
    }

    private void StartJump()
    {
        IsJumping = true;
        _jumpStartZ = transform.position.z;
        playerAudioController.PlayJumpSound();
        
        StopRoll();
    }

    private void StopJump()
    {
        IsJumping = false;
    }

    private float ProcessJump()
    {
        float deltaY = 0;
        
        if (IsJumping)
        {
            var jumpCurrentProgress = transform.position.z - _jumpStartZ;
            var jumpPercent = jumpCurrentProgress / jumpDistanceZ;
            if (jumpPercent >= 1)
            {
                StopJump();
            }
            else
            {
                deltaY = Mathf.Sin(Mathf.PI * jumpPercent) * jumpHeightY;
            }
        }

        if (!IsJumping && !IsGrounded)
        {
            deltaY = Mathf.MoveTowards(transform.position.y, _initialPosition.y, jumpLerpSpeed * Time.deltaTime);
        }
        
        return _initialPosition.y + deltaY;
    }

    private void ProcessRoll()
    {
        if (!IsRolling) return;
        
        var percent = (transform.position.z - _rollStartZ) / rollDistanceZ;
        if (percent >= 1)
        {
            StopRoll();
        }
    }

    private void StartRoll()
    {
        _rollStartZ = transform.position.z;
        IsRolling = true;
        regularCollider.enabled = false;
        rollCollider.enabled = true;
        playerAudioController.PlayRollSound();

        StopJump();
    }

    private void StopRoll()
    {
        IsRolling = false;
        regularCollider.enabled = true;
        rollCollider.enabled = false;
    }

    public void Die()
    {
        _isDead = true;
        horizontalSpeed = 0;
        StopRoll();
        StopJump();
        regularCollider.enabled = false;
        rollCollider.enabled = false;
    }
}
