using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameAnimationState : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = animator.transform.parent.GetComponent<PlayerController>();
        player.enabled = true;
    }
}