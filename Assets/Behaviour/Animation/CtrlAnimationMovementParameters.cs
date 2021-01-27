using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlAnimationMovementParameters : StateMachineBehaviour
{
    public float MoveSpeed = 1f;
    public Vector2 MoveDirection = new Vector2(0,0);
    public bool Crouched = false;

    public void Recalculate()
    {
        if (_animator != null) _animator.SetTrigger("Recalculate");
    }

    Animator _animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator;
        LocalInfo.ctrlAnimSpeedSingleton = this;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", MoveSpeed);
        animator.SetFloat("Y_Velocity", MoveDirection.y);
        animator.SetFloat("X_Velocity", MoveDirection.x);
        animator.SetBool("Crouched", Crouched);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LocalInfo.ctrlAnimSpeedSingleton = null;
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
