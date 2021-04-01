using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Animator getCurrentAnimator()
    {
        string currentPath = "/Center/Network/DefaultPathway";
        GameObject anim1 = GameObject.Find(currentPath + "/Animation1Container");
        if (!anim1.gameObject.activeSelf) {
            GameObject anim2 = GameObject.Find(currentPath + "/Animation2Container");
            return anim2.gameObject.activeSelf ? GameObject.Find(currentPath + "/Animation2Container/animation_2").GetComponent<Animator>()
            : GameObject.Find(currentPath).GetComponent<Animator>();
        }

        return GameObject.Find(currentPath + "/Animation1Container/animation_1").GetComponent<Animator>();
    }

    public void Pause()
    {
        Animator anim = getCurrentAnimator();

        anim.enabled = false;
    }

    public void Resume()
    {
        Animator anim = getCurrentAnimator();

        anim.enabled = true;
    }

    public void Scrub(float multiplier)
    {
	// https://docs.unity3d.com/ScriptReference/AnimatorStateInfo-speedMultiplier.html
	// negative -> backwards, positive -> forwards, default = 1f
 	// anim.SetFloat("speedMultiplier", multiplier);
    }
}
