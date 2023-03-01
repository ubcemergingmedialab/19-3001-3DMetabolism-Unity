using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetColor : StateMachineBehaviour
{
    //TODO buggy
    public Color resetColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Renderer>().material.SetColor("_WiggleColor", resetColor);
        
        //RESET NODE
        NodeTextDisplay.Instance.UpdateTextDisplay();

        Transform transform = animator.transform.Find("NodeTemplate/Label");

        if(null!= transform)
        {
            TextMeshPro tmp = transform.GetComponent<TextMeshPro>();
            if (null != tmp)
            {
                //TODO change color to ???
            }
        }
        //animator.transform.Find("NodeTemplate/Label").GetComponent<TextMeshPro>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
