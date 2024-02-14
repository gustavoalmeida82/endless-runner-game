using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 4.0f;
    [SerializeField] private float forwardSpeed = 10.0f;
    [SerializeField] private float laneDistanceX = 4.0f;
    
    [Header("Jump")] 
    [SerializeField] private float jumpDistanceZ = 4;
    [SerializeField] private float jumpHeightY = 2;

    private Vector3 _initialPosition;
    private float _targetPositionX;
    private float _jumpStartZ;
    
    private float LaneBoundRight => _initialPosition.x + laneDistanceX;
    private float LaneBoundLeft => _initialPosition.x - laneDistanceX;
    public float JumpDuration => jumpDistanceZ / forwardSpeed;
    
    public bool IsJumping { get; private set; }

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        ProcessInput();
        
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
                IsJumping = false;
            }
            else
            {
                deltaY = Mathf.Sin(Mathf.PI * jumpPercent) * jumpHeightY;
            }
        }

        return _initialPosition.y + deltaY;
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
            IsJumping = true;
            _jumpStartZ = transform.position.z;
        }
        
        _targetPositionX = Mathf.Clamp(_targetPositionX, LaneBoundLeft, LaneBoundRight);
    }
}
