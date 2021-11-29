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
    public AnimationDescriptionPresenter presenter;

    private Coroutine animationRoutine;

    void Start()
    {
        //animationRoutine = StartCoroutine("PlayAnimations");
        presenter = FindObjectOfType<AnimationDescriptionPresenter>();
    }

    private IEnumerator PlayAnimations()
    {
        foreach(AnimationDescription animation in animations)
        {
            Dictionary<string, string> animationDefinition;
            animationDefinition = DefineAnimation(animation);
            AnimateGameObject(animationDefinition);
            if(presenter != null)
            {
                presenter.HighlightStep(int.Parse(animation.name));
            }
            yield return new WaitForSeconds(waitTime);
            ResetGameObject(animationDefinition);
        }
        yield return new WaitForSeconds(waitTime);
        animationRoutine = StartCoroutine("PlayAnimations");
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

    private void AnimateGameObject(Dictionary<string, string> animationDefinition)
    {
        Animator gameObjectAnimator;
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject curGO = GameObject.Find(animationStep.Key);
            if(curGO != null)
            {
                gameObjectAnimator = curGO.GetComponent<Animator>();
                if(gameObjectAnimator != null)
                {
                    Debug.Log(curGO.name + " " + animationStep.Value);
                    gameObjectAnimator.Play(animationStep.Value);
                } else
                {
                    Debug.Log("didnt find animator in " + curGO.name);
                }
            } else
            {
                Debug.Log("gameobject not found");
            }
        }
    }

    private void ResetGameObject(Dictionary<string, string> animationDefinition)
    {
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject curGO = GameObject.Find(animationStep.Key);
            if(curGO != null)
            {
                Animator gameObjectAnimator = curGO.GetComponent<Animator>();
                if(gameObjectAnimator != null)
                {
                    gameObjectAnimator.Play("Idle");
                }
            }
        }
        /*
        foreach(Animator anim in transform.GetComponentsInChildren<Animator>(true))
        {
            if(anim.gameObject.activeSelf)
            {
                anim.Play("Idle");
            }
        }*/
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
