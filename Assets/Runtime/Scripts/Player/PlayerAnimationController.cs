using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        animator.SetBool(PlayerAnimationConstants.IsJumping, player.IsJumping);
        animator.SetBool(PlayerAnimationConstants.IsRolling, player.IsRolling);
    }

    public void Die()
    {
        animator.SetTrigger(PlayerAnimationConstants.DieTrigger);
    }

    public IEnumerator StartGame()
    {
        animator.SetTrigger(PlayerAnimationConstants.StartGameTrigger);
        
        //Espera entrar no estado de animação de início de jogo
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimationConstants.StartGameAnimationStateName))
        {
            yield return null;
        }
        
        //Esperar sair desse estado (espera a animação terminar de tocar)
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimationConstants.StartGameAnimationStateName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }
}
