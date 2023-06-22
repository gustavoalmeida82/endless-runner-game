using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        animator.SetBool(PlayerAnimationConstants.IsJumping, _player.IsJumping);
        animator.SetBool(PlayerAnimationConstants.IsRolling, _player.IsRolling);
    }

    public void Die()
    {
        animator.SetTrigger(PlayerAnimationConstants.DeathTrigger);
    }
}
