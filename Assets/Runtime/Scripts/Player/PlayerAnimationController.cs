using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
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
        animator.SetBool("IsJumping", _player.IsJumping);
        animator.SetBool("IsRolling", _player.IsRolling);
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
