using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrub : MonoBehaviour
{
    string CURRENT_PATH = "/Center/Network/DefaultPathway";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator anim = getCurrentAnimator();

        GameObject.Find(CURRENT_PATH + "/Canvas/Slider").GetComponent<Slider>().value = 1 - (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
    }
    
    Animator getCurrentAnimator()
    {
        GameObject anim1 = GameObject.Find(CURRENT_PATH + "/Animation1Container");
        if (!anim1.gameObject.activeSelf) {
            GameObject anim2 = GameObject.Find(CURRENT_PATH + "/Animation2Container");
            return anim2.gameObject.activeSelf ? GameObject.Find(CURRENT_PATH + "/Animation2Container/animation_2").GetComponent<Animator>()
            : GameObject.Find(CURRENT_PATH).GetComponent<Animator>();
        }

        return GameObject.Find(CURRENT_PATH + "/Animation1Container/animation_1").GetComponent<Animator>();
    }
}
