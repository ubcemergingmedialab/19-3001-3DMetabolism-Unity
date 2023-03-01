using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseAnimateMaterial : StateMachineBehaviour
{
    private Color StartColor;
    public Color EndColor;
    public float animationLength = 2;
    private float currentTime;
    private bool isFoward;

    private float usedAnimationLength;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        StartColor = animator.GetComponent<Renderer>().material.GetColor("_WiggleColor");
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
                animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", Color.Lerp(StartColor, EndColor, currentTime / usedAnimationLength));
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
            animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", Color.Lerp(EndColor, StartColor, currentTime / usedAnimationLength));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //NO-OP: The text is reset via trigger in ResetColor.
    }
}
