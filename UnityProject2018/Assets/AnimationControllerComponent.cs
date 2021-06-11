using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animations;
    public float waitTime = 1f;
    public float resetTime = 0.1f;
    public AnimationDescription resetAnimation;

    private Coroutine animationRoutine;

    void Start()
    {
        //animationRoutine = StartCoroutine("PlayAnimations");
    }

    private IEnumerator PlayAnimations()
    {
        foreach(AnimationDescription animation in animations)
        {
            Dictionary<string, string> animationDefinition;
            animationDefinition = DefineAnimation(animation);
            AnimateGameObject(animationDefinition);
            yield return new WaitForSeconds(waitTime);
            ResetGameObject(animationDefinition);
        }
        yield return new WaitForSeconds(waitTime);
        animationRoutine = StartCoroutine("PlayAnimations");
    }

    private Dictionary<string, string> DefineAnimation(AnimationDescription animation) 
    {
        Debug.Log("Animation for :" + animation.name);
        Dictionary<string, string> animationDefinition;
        List<string> objectsToAnimate;
        List<string> triggersToSet;
        objectsToAnimate = animation.AnimatedObjects;
        triggersToSet = animation.TriggerToSet;
        animationDefinition = objectsToAnimate.Zip(triggersToSet, (k, v) => new { k, v })
            .ToDictionary(x => x.k, x => x.v);
        return animationDefinition;
    }

    private void AnimateGameObject(Dictionary<string, string> animationDefinition)
    {
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject gameObject = GameObject.Find(animationStep.Key);
            Debug.Log("Animating: " + gameObject.name + " " + animationStep.Value);
            Animator gameObjectAnimator = gameObject.GetComponent<Animator>();
            gameObjectAnimator.SetTrigger(animationStep.Value);
        }
    }

    private void ResetGameObject(Dictionary<string, string> animationDefinition)
    {
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject gameObject = GameObject.Find(animationStep.Key);
            if(gameObject != null)
            {
                Animator gameObjectAnimator = gameObject.GetComponent<Animator>();
                foreach (var param in gameObjectAnimator.parameters)
                {
                    if (param.type == AnimatorControllerParameterType.Trigger)
                    {
                        gameObjectAnimator.ResetTrigger(param.name);
                    }
                }
            }
        }
    }

    public IEnumerator ChangeAnimation(List<AnimationDescription> newAnimation)
    {
        Dictionary<string, string> reset = DefineAnimation(resetAnimation);
        AnimateGameObject(reset);
        yield return new WaitForSeconds(resetTime);
        ResetGameObject(reset);
        if(animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }
        this.animations = newAnimation;
        animationRoutine = StartCoroutine("PlayAnimations");
    }
}
