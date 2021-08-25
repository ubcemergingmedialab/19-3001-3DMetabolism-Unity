using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMaterial : StateMachineBehaviour
{
    public Color StartColor;
    public Color EndColor;
    public float animationLength = 2;
    private float currentTime;
    private float endTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = Time.time;
        endTime = currentTime + animationLength;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = Time.time;
        animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", Color.Lerp(StartColor, EndColor, (endTime - currentTime) / animationLength));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", EndColor);
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
