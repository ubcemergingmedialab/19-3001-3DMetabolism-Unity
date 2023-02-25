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

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        StartColor = animator.GetComponent<Renderer>().material.GetColor("_WiggleColor");
        isFoward = true;
        animationLength /= 2;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFoward)
        {
            if (currentTime <= animationLength)
            {
                currentTime += Time.deltaTime;
                animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", Color.Lerp(StartColor, EndColor, currentTime / animationLength));
            }
            else
            {
                isFoward = false;
                currentTime = 0;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", Color.Lerp(EndColor, StartColor, currentTime / animationLength));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", StartColor);
    }
}
