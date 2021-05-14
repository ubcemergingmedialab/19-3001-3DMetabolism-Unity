using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animations;

    private float waitTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("AnimationRoutine");
        // Inside Coroutine:
        //      read next animation from list
        //      fetch corresponding gameobject
        //      set corresponding trigger on gameobject
        //      wait an amount of time set by either technician or animation clip length
    }

    //Coroutine
    IEnumerator AnimationRoutine()
    {
        List<string> animatedObjects;
        List<string> triggerToSet;
      
        foreach(AnimationDescription animation in animations)
        {
            animatedObjects = animation.AnimatedObjects;
            triggerToSet = animation.TriggerToSet;
            var dic = animatedObjects.Zip(triggerToSet, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);

            foreach(KeyValuePair<string, string> gameObjectTrigger in dic)
            {
                //Debug.Log("-------------------");
                //Debug.Log(string.Join(", ", gameObjectTrigger.Key));
                //Debug.Log(string.Join(", ", gameObjectTrigger.Value));
                //Debug.Log("-------------------");
                GameObject gameObject = GameObject.FindGameObjectWithTag(gameObjectTrigger.Key);
                Debug.Log(string.Join("",gameObject.name));
                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetTrigger(gameObjectTrigger.Value);
                //WaitForAnimation(animator.GetCurrentAnimatorClipInfo(0));
            }
        yield return new WaitForSeconds(waitTime);
       
        }
    }

    private IEnumerator WaitForAnimation ( Animation animation)
    {
        do
        {
            yield return null;
        } while ( animation.isPlaying);
    }
    // Update is called once per frame
    void Update()
    {
       // StartCoroutine("AnimationRoutine");
    }

    //Define Coroutine that lasts as long as an animation (or decide what the interval should be)
}
