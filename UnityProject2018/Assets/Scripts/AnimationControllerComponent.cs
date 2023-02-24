using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 2023-2-15
/// Given a list of AnimationDescriptions, it will loop through each AD, and play the animation.  For example:
/// if a AnimationDescription has an Animated Object of 'glucose-6-phosphate' and the trigger is 'GreyToWhite',
/// it will look for that node of 'glucose-6-phosphate' and trigger 'GreyToWhite'.
/// - Also does color highlighting of the materials AnimationDescriptionPresenter is present.
/// </summary>
public class AnimationControllerComponent : MonoBehaviour
{
    public List<AnimationDescription> animations;
    public float waitTime = 1f;
    public float resetTime = 0.1f;
    public AnimationDescription resetAnimation;
    public AnimationDescriptionPresenter presenter;

    // singleton instantiation of AnimationControllerComponent
    private static AnimationControllerComponent _instance;
    public static AnimationControllerComponent Instance
    {
        get { return _instance; }
    }

    private Coroutine animationRoutine;

    /// <summary>
    /// Singleton!
    /// </summary>
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    /// <summary>
    ///Assigns Presenter (optional)
    ///Manually start coroutine here for testing
    /// </summary>
    void Start()
    {
        //animationRoutine = StartCoroutine("PlayAnimations");
        if (presenter == null)
        {
            presenter = FindObjectOfType<AnimationDescriptionSliderPresenter>();
        }

        //For testing
        //StartCoroutine(PlayAnimations());
    }

    /// <summary>
    /// Animates each ordered item from start to finish.
    /// -Stops existing animations / coroutines
    /// -Pulses the start and end node before it loops through
    /// </summary>
    /// <param name="list">list of ordered scriptable objects to be animated
    public void AnimateSearchResults(List<ScriptableObject> list)
    {
        foreach(ScriptableObject so in list)
        {
            AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();

            //add it
            animations.Add(ad);
        }
        //TODO STOP EXISTING ANIMATIONS
        //???

        //TODO DO STUFF
        //Create new AD and add that to the list of animations

        //TODO
        //start the loop
        StartCoroutine(PlayAnimations());
    }

    /// <summary>
    /// Goes through each AD, looks for it in the scene, and triggers it.  It also plays reset animation if presented.
    /// When finished, it will restart from the first animation.
    /// </summary>
    private IEnumerator PlayAnimations()
    {
        foreach (AnimationDescription animation in animations)
        {
            Dictionary<string, string> animationDefinition;
            animationDefinition = DefineAnimation(animation);
            AnimateGameObject(animationDefinition);
            if (presenter != null)
            {
                presenter.HighlightStep(int.Parse(animation.name));
            }
            yield return new WaitForSeconds(waitTime);
            ResetGameObject(animationDefinition);
        }
        yield return new WaitForSeconds(waitTime);
        animationRoutine = StartCoroutine("PlayAnimations");
    }

    /// <summary>
    /// Given a single animation description, it will return a key value pair of it's name and trigger
    /// </summary>
    /// <param name="animation">the Animation Description Scriptable Object
    /// <returns> A dictionary<string, string> of all the AD element name and it's triggers </returns>
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

    /// <summary>
    /// Given a key value pair of a animation definition, it will look for the animator of the key,
    /// and then play the trigger of the animation.  For ex: if key is 'glucose-6-phosphate' and is found,
    /// it will attempt to play 'GreyToWhite' trigger (whatever the value is).
    /// </summary>
    /// <param name="animationDefinition">string key valye pair of the animation definition
    private void AnimateGameObject(Dictionary<string, string> animationDefinition)
    {
        Animator gameObjectAnimator;
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject curGO = GameObject.Find(animationStep.Key);
            if (curGO != null)
            {
                gameObjectAnimator = curGO.GetComponent<Animator>();
                if (gameObjectAnimator != null)
                {
                    Debug.Log(curGO.name + " " + animationStep.Value);
                    gameObjectAnimator.Play(animationStep.Value);
                }
                else
                {
                    Debug.Log("didnt find animator in " + curGO.name);
                }
            }
            else
            {
                Debug.Log("gameobject not found");
            }
        }
    }

    /// <summary>
    /// Given a key value pair of a animation definition, it will look for the animator of the key,
    /// and then play the trigger of the animation, which is coded to 'Idle'.  For ex: if key is 'glucose-6-phosphate' and is found,
    /// it will attempt to play 'Idle' trigger.
    /// </summary>
    /// <param name="animationDefinition">string key valye pair of the animation definition
    private void ResetGameObject(Dictionary<string, string> animationDefinition)
    {
        foreach (KeyValuePair<string, string> animationStep in animationDefinition)
        {
            GameObject curGO = GameObject.Find(animationStep.Key);
            if (curGO != null)
            {
                Animator gameObjectAnimator = curGO.GetComponent<Animator>();
                if (gameObjectAnimator != null)
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

    /// <summary>
    /// Plays reset animation (if applicable), then loads the next set of animations (ex: a different pathway),
    /// pauses based on resetTime, then plays the new list of animations.
    /// </summary>
    /// <param name="newAnimation">List of AnimationDescription scriptable objects.
    public IEnumerator ChangeAnimation(List<AnimationDescription> newAnimation)
    {
        Dictionary<string, string> reset = DefineAnimation(resetAnimation);
        AnimateGameObject(reset);
        yield return new WaitForSeconds(resetTime);
        ResetGameObject(reset);
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }
        this.animations = newAnimation;
        animationRoutine = StartCoroutine("PlayAnimations");
    }
}
