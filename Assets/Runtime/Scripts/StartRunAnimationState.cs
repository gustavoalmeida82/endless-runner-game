using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunAnimationState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.parent.TryGetComponent<PlayerController>(out var player))
        {
            player.enabled = true;
        }
    }
}
