using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAnimationState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = animator.transform.parent.GetComponent<PlayerController>();
        var clips = animator.GetNextAnimatorClipInfo(layerIndex);
        if (player != null && clips.Length > 0)
        {
            var rollClipInfo = clips[0];
            var multiplier = rollClipInfo.clip.length / player.RollDuration;
            animator.SetFloat("RollMultiplier", multiplier);
        }
    }
}
