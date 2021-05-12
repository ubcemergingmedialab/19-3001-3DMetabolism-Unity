using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animation;

    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        // Inside Coroutine:
        //      read next animation from list
        //      fetch corresponding gameobject
        //      set corresponding trigger on gameobject
        //      wait an amount of time set by either technician or animation clip length
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Define Coroutine that lasts as long as an animation (or decide what the interval should be)
}
