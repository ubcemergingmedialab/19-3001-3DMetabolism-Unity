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
    private Material material;
    private TextMeshPro tmp;

    private float usedAnimationLength;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //material = animator.transform.Find("NodeTemplate/Label").GetComponent<Renderer>().material;
        tmp = animator.transform.Find("NodeTemplate/Label").GetComponent<TextMeshPro>();
        currentTime = 0;
        StartColor = tmp.color;
        isFoward = true;
        usedAnimationLength = animationLength / 2;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFoward)
        {
            if (currentTime <= usedAnimationLength)
            {
                currentTime += Time.deltaTime;
                //material.SetColor("Color", Color.Lerp(StartColor, EndColor, currentTime / animationLength));
                tmp.color = Color.Lerp(StartColor, EndColor, currentTime / usedAnimationLength);
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
            //material.SetColor("Color", Color.Lerp(EndColor, StartColor, currentTime / animationLength));
            tmp.color = Color.Lerp(EndColor, StartColor, currentTime / usedAnimationLength);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //material.SetColor("Color", StartColor);
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
