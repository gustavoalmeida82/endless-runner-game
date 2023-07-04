using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (animator.transform.parent.TryGetComponent<PlayerController>(out var player))
       {
            player.enabled = false;
       }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(PlayerAnimationConstants.StartGameTrigger);
        }
    }
}
