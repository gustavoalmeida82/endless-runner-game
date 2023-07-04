using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimationState : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       var clips = animator.GetNextAnimatorClipInfo(layerIndex);
       var player = animator.transform.parent.GetComponent<PlayerController>();
       if (clips.Length > 0 && player != null)
       {
            var jumpClipInfo = clips[0];
            var multiplier = jumpClipInfo.clip.length / player.JumpDuration;
            animator.SetFloat(PlayerAnimationConstants.JumpMultiplier, multiplier);
       }
    }
}
