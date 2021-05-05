using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1:1 for each animator (1 instance of this class for each animation)
public class AnimationColleague : MonoBehaviour
{
    private Animator animator;
    private bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isEnabled) {
            GameObject.Find("Slider").GetComponent<Slider>().value = 1 - (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
        }
    }

    public void setAnimator(Animator animator) {
        this.animator = animator;
    }
    
    public void Enable()
    {
        this.isEnabled = true;
        AnimationMediator.PlayEvent += ResumeHandler;
        AnimationMediator.PauseEvent += PauseHandler;
    }
    
    public void Disable()
    {
        AnimationMediator.PlayEvent -= ResumeHandler;
        AnimationMediator.PauseEvent -= PauseHandler;
    }

    public void PauseHandler()
    {
        this.animator.enabled = false;
    }

    public void ResumeHandler()
    {
        this.animator.enabled = true;
    }
}
