using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 4.0f;
    [SerializeField] private float forwardSpeed = 10.0f;
    [SerializeField] private float laneDistanceX = 4.0f;

    private Vector3 _initialPosition;
    private float _targetPositionX;

    private float LaneBoundRight => _initialPosition.x + laneDistanceX;
    private float LaneBoundLeft => _initialPosition.x - laneDistanceX;

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

    private float ProcessLaneMovement()
    {
        return Mathf.MoveTowards(transform.position.x, _targetPositionX, horizontalSpeed * Time.deltaTime);
    }

    private float ProcessForwardMovement()
    {
        return transform.position.z + (forwardSpeed * Time.deltaTime);
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
        
        _targetPositionX = Mathf.Clamp(_targetPositionX, LaneBoundLeft, LaneBoundRight);
    }
}
