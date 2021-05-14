using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animations;

    private float waitTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AnimationRoutine");
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
                Debug.Log(string.Join(", ", gameObjectTrigger.Key));
                Debug.Log(string.Join(", ", gameObjectTrigger.Value));
                Debug.Log("-------------------");
                GameObject gameObject = GameObject.Find(gameObjectTrigger.Key);
                (gameObject.GetComponent<Animator>()).SetTrigger(gameObjectTrigger.Key);
            }
       
        }
        yield return new WaitForSeconds(waitTime);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //Define Coroutine that lasts as long as an animation (or decide what the interval should be)
}
