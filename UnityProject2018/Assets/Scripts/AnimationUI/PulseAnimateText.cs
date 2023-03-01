using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PulseAnimateText : StateMachineBehaviour
{
    private Color StartColor;
    public Color EndColor;
    public float animationLength = 2;
    private float currentTime;
    private bool isFoward;
    private TextMeshPro tmp;

    private float usedAnimationLength;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //finds the children that matches the string below, and gets the TextMeshPro component.
        //we can safely assume it has it (no need for null check).
        tmp = animator.transform.Find("NodeTemplate/Label").GetComponent<TextMeshPro>();
        currentTime = 0;
        StartColor = tmp.color;
        isFoward = true;

        //divide by two because we split the animation into two parts.  1st is to transition to targeted color, 2nd is to come back.
        usedAnimationLength = animationLength / 2;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //when we are in the first half of animation, lerp towards color.
        if (isFoward)
        {
            if (currentTime <= usedAnimationLength)
            {
                currentTime += Time.deltaTime;
                tmp.color = Color.Lerp(StartColor, EndColor, currentTime / usedAnimationLength);
            }
            else
            {
                isFoward = false;
                currentTime = 0;
            }
        }
        //when we are in the last half of animation, lerp towards original color.
        else
        {
            currentTime += Time.deltaTime;
            tmp.color = Color.Lerp(EndColor, StartColor, currentTime / usedAnimationLength);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tmp.color = StartColor;
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
