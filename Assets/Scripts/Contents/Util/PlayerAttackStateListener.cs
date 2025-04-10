using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateListener : StateMachineBehaviour
{
    private PlayerAnimationEventListener playerAnimationEventListener;

    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (stateInfo.normalizedTime >= 0.9f)
    //    {
    //        if (playerAnimationEventListener == null)
    //        {
    //            playerAnimationEventListener = animator.GetComponent<PlayerAnimationEventListener>();
    //        }
    //        playerAnimationEventListener.endAttackAnimationEvent?.Invoke();
    //    }
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerAnimationEventListener == null)
        {
            playerAnimationEventListener = animator.GetComponent<PlayerAnimationEventListener>();
        }

        playerAnimationEventListener.endAttackAnimationEvent?.Invoke();
        //if (stateInfo.normalizedTime >= 1f)
        //{
        //    if (playerAnimationEventListener == null)
        //    {
        //        playerAnimationEventListener = animator.GetComponent<PlayerAnimationEventListener>();
        //    }
        //    playerAnimationEventListener.endAttackAnimationEvent?.Invoke();
        //}
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
