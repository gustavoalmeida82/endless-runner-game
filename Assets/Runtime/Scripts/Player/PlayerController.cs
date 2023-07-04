using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;

    [Header("Ground Movement")]
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float laneWidth = 4;

    [Header("Jump")]
    [SerializeField] private float jumpMaxHeight = 4;
    [SerializeField] private float jumpMaxDistance = 4;
    [SerializeField] private float jumpAbortSpeed = 3;

    [Header("Roll")]
    [SerializeField] private float rollMaxDistance = 4;

    [Header("Colliders")]
    [SerializeField] private Collider regularCollider;
    [SerializeField] private Collider rollCollider;

    public float RightLaneBound => _initialPosition.x + laneWidth;
    public float LeftLaneBound => _initialPosition.x - laneWidth;
    public float JumpDuration => jumpMaxDistance / forwardSpeed;
    public float RollDuration => rollMaxDistance / forwardSpeed;
    private bool OnGround => transform.position.y == _initialPosition.y;
    private bool CanJump => !IsJumping && OnGround;
    private bool CanRoll => !IsRolling;

    public bool IsJumping { get; private set; }
    public bool IsRolling { get; private set; }
    public bool IsDead { get; private set; }

    private Vector3 _initialPosition;
    private float _targetPositionX;
    private float _jumpStartZ;
    private float _rollStartZ;

    private void Awake()
    {
        _initialPosition = transform.position;
        StopJump();
        StopRoll();
    }

    private void Update()
    {
        ProcessInput();

        var position = transform.position;
        position.x = ProcessLaneMovement();
        position.z = ProcessForwardMovement();
        position.y = ProcessJumpMovement();

        ProcessRollMovement();

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

        if (Input.GetKeyDown(KeyCode.W) && CanJump)
        {
            StartJump();
        }

        if (Input.GetKeyDown(KeyCode.S) && CanRoll)
        {
            StartRoll();
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
                StopJump();
            }
        }

        if ((IsRolling && !OnGround) || (IsDead && !OnGround))
        {
            var currentHeight = transform.position.y - _initialPosition.y;
            deltaY = Mathf.Max(0, currentHeight - jumpAbortSpeed * Time.deltaTime);
        }

        return _initialPosition.y + deltaY;
    }

    private void StartJump()
    {
        IsJumping = true;
        _jumpStartZ = transform.position.z;
        StopRoll();
    }

    private void StopJump()
    {
        IsJumping = false;
    }

    private void ProcessRollMovement()
    {
        if (IsRolling)
        {
            var rollCurrentProgress = transform.position.z - _rollStartZ;
            var rollPercent = rollCurrentProgress / rollMaxDistance;

            if (rollPercent >= 1)
            {
                StopRoll();
            }
        }
    }

    private void StartRoll()
    {
        IsRolling = true;
        _rollStartZ = transform.position.z;
        regularCollider.enabled = false;
        rollCollider.enabled = true;
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
        //TODO: Fix animation bug when dying on air.
        forwardSpeed = 0;
        horizontalSpeed = 0;
        IsDead = true;
        StopJump();
        StopRoll();
        gameMode.OnGameOver();
    }
}
