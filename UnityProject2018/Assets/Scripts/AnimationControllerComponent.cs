using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

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
    private List<ScriptedAnimation> scriptedAnimations;

    public Material scriptedAnimationMaterial;


    public float waitTime = 0.25f;
    public float resetTime = 0.1f;
    public int timesToPulseStartAndEndNodes = 1;
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
    /// Stops animation coroutine, tells the nodes/edges to play the 'Reset' trigger, then clears the queue from animations
    /// </summary>
    public void StopAllAnimations()
    {
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);

            foreach (AnimationDescription animation in animations)
            {
                foreach (string name in animation.AnimatedObjects)
                {
                    //TODO generally don't want to call GO.find.  Better to cache the GO somewhere everytime we load.
                    //in this case it might be fine because it generally only gets called once? (To stop a specific animation).
                    GameObject curGO = GameObject.Find(name);
                    if (curGO != null)
                    {
                        ScriptedAnimation[] allScriptedAnimations = curGO.GetComponentsInChildren<ScriptedAnimation>();

                        for (int i = 0; i < allScriptedAnimations.Length; i++)
                        {
                            allScriptedAnimations[i].StopAnimating();
                        }
                    }
                }
            }
            animations.Clear();
            scriptedAnimations.Clear();
        }
    }

    /// <summary>
    /// Casts into a NodeSO object to get the label (name of the game object) and the trigger (name of the animation trigger)
    /// </summary>
    /// <param name="ad">The animation description to define the set of animations.
    /// <param name="so">The scriptable object from the search result.
    /// <param name="triggerName">The name of the trigger/animation clip.
    private void AddNodeToAnimationDescription(AnimationDescription ad, ScriptableObject so, string triggerName)
    {
        NodeSO nodeSo = (NodeSO)so;
        ad.AnimatedObjects.Add(nodeSo.Label);
        ad.TriggerToSet.Add(triggerName);
    }

    /// <summary>
    /// Casts into a EdgeSO object to get the Enzyme (name of the game object) and the trigger (name of the animation trigger)
    /// </summary>
    /// <param name="ad">The animation description to define the set of animations.
    /// <param name="so">The scriptable object from the search result.
    /// <param name="triggerName">The name of the trigger/animation clip.
    private void AddEdgeToAnimationDescription(AnimationDescription ad, ScriptableObject so, string triggerName)
    {
        EdgeSO edgeSo = (EdgeSO)so;
        ad.AnimatedObjects.Add(edgeSo.Enzyme);
        ad.TriggerToSet.Add(triggerName);
    }

    /// <summary>
    /// Animates each ordered item from start to finish.
    /// -Stops existing animation coroutine
    /// -Pulses the start and end node before it loops through
    /// </summary>
    /// <param name="list">list of ordered scriptable objects to be animated
    public void AnimateSearchResults(List<ScriptableObject> list)
    {
        StopAllAnimations(); // Stop animations differently later


        //'Pulse' the starting and end nodes 'x' times.
        //for (int i = 0; i < timesToPulseStartAndEndNodes; i++)
        //{
        //    AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();
        //    ad.AnimatedObjects = new List<string>();
        //    ad.TriggerToSet = new List<string>();

        //    ScriptableObject startNodeSO = list[0];
        //    ScriptableObject endNodeSO = list[list.Count - 1];

        //    AddNodeToAnimationDescription(ad, startNodeSO, "Flash");
        //    AddNodeToAnimationDescription(ad, endNodeSO, "Flash");
        //    animations.Add(ad);
        //}

        //Go through each node and edge and pulse 1x
        foreach (ScriptableObject so in list)
        {
            AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();
            ad.AnimatedObjects = new List<string>();
            ad.TriggerToSet = new List<string>();

            //add this node to the pathway if it is a node
            if (so.GetType() == typeof(NodeSO))
            {
                AddNodeToAnimationDescription(ad, so, "Pulse");
            }
            else if (so.GetType() == typeof(EdgeSO))
            {
                AddEdgeToAnimationDescription(ad, so, "Pulse");
            }
            else
            {
                Debug.LogWarning("We cannot add a SO that is neither a node or a edge");
            }

            

            //add it
            animations.Add(ad);
        }
                
        //Start the animation
        animationRoutine = StartCoroutine("PlayAnimations");
    }

    /// <summary>
    /// Goes through each AD, looks for it in the scene, and triggers it.  It also plays reset animation if presented.
    /// When finished, it will restart from the first animation.
    /// </summary>
    private IEnumerator PlayAnimations()
    {
        // Find all objects

        scriptedAnimations = new List<ScriptedAnimation>();

        for (int i = 0; i < animations.Count; i++)
        {
            for (int j = 0; j < animations[i].AnimatedObjects.Count; j++)
            {
                GameObject g = GameObject.Find(animations[i].AnimatedObjects[j]);

                if (g != null)
                {
                    ScriptedAnimation newAnimation = g.AddComponent<ScriptedAnimation>();
                    newAnimation.delay = ((i) + waitTime);
                    newAnimation.InitializeScriptedAnimation();
                    newAnimation.scriptedAnimationMaterial = scriptedAnimationMaterial;
                    scriptedAnimations.Add(newAnimation);
                    
                }

            }
        }

        for (int i = 0; i < scriptedAnimations.Count; i++)
        {
            scriptedAnimations[i].StartAnimating();
            //scriptedAnimations[i].gameObject.GetComponent<ScriptedAnimation>().StartCoroutine(scriptedAnimations[i].StartAnimating());
            //scriptedAnimations[i].ChangeColorByForce();
        }

        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(10);

        //foreach (AnimationDescription animation in animations)
        //{
        //    foreach(string s in animation.AnimatedObjects)
        //    {

        //    }

        //    Dictionary<string, string> animationDefinition;
        //    animationDefinition = DefineAnimation(animation);
        //    AnimateGameObject(animationDefinition);
        //    if (presenter != null)
        //    {
        //        presenter.HighlightStep(int.Parse(animation.name));
        //    }
        //    yield return new WaitForSeconds(waitTime);
        //    ResetGameObject(animationDefinition);
        //}
        //yield return new WaitForSeconds(waitTime);
        //animationRoutine = StartCoroutine("PlayAnimations");
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
