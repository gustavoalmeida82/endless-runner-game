using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAnimationState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       var clips = animator.GetNextAnimatorClipInfo(layerIndex);
       var player = animator.transform.parent.GetComponent<PlayerController>();
       if (clips.Length > 0 && player != null)
       {
            var rollClipInfo = clips[0];
            var multiplier = rollClipInfo.clip.length / player.RollDuration;
            animator.SetFloat(PlayerAnimationConstants.RollMultiplier, multiplier);
       }
    }
}
