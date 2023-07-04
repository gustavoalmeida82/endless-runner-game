using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerCollision : MonoBehaviour
{
    private PlayerController _player;
    private PlayerAnimationController _animation;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _animation = GetComponent<PlayerAnimationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out var obstacle))
        {
            _player.Die();
            _animation.Die();
        }
    }
}
