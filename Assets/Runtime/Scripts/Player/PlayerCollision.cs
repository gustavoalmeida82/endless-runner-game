using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    
    private PlayerController player;
    private PlayerAnimationController animator;
    
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<PlayerAnimationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obstacle = other.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            player.Die();
            animator.Die();
            gameMode.OnGameOver();
        }
    }
}
