using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animations;
    private float waitTime = 1f;

    void Start()
    {
        StartCoroutine("PlayAnimations");
    }

    private IEnumerator PlayAnimations()
    {
        foreach(AnimationDescription animation in animations)
        {
            Dictionary<string, string> animationDefinition;
            animationDefinition = DefineAnimation(animation);
            AnimateGameObject(animationDefinition);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Dictionary<string, string> DefineAnimation(AnimationDescription animation) 
    {
        Dictionary<string, string> animationDefinition;
        List<string> objectsToAnimate;
        List<string> triggersToSet;
        objectsToAnimate = animation.AnimatedObjects;
        triggersToSet = animation.TriggerToSet;
        animationDefinition = objectsToAnimate.Zip(triggersToSet, (k, v) => new { k, v })
            .ToDictionary(x => x.k, x => x.v);
        return animationDefinition;
    }

    private void AnimateGameObject(Dictionary<string, string> animationDefintion)
    {
        foreach (KeyValuePair<string, string> animationStep in animationDefintion)
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag(animationStep.Key);
            Animator gameObjectAnimator = gameObject.GetComponent<Animator>();
            gameObjectAnimator.SetTrigger(animationStep.Value);
        }
    }
}
