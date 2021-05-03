using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    None,
    Forward,
    Backward,
    Outward,
    Inward
}

public class GuidedTourManager : MonoBehaviour {
    private static GuidedTourManager _instance;
    public static GuidedTourManager Instance
    {
        get { return _instance; }
    }

    public SceneData[] sceneDataArray;

    /// APP STATE EVENTS
    public delegate void DefaultStateHandler();
    public static event DefaultStateHandler DefaultState;

    public delegate void Initialize();
    public static event Initialize InitializeEvent;

    public delegate void VisitPrevious(SceneData sceneData);
    public static event VisitPrevious VisitPreviousEvent;

    public delegate void VisitNext(SceneData sceneData);
    public static event VisitNext VisitNextEvent;

    public delegate void ZoomIn(SceneData sceneData);
    public static event ZoomIn ZoomInEvent;

    public delegate void ZoomOut(SceneData sceneData);
    public static event ZoomOut ZoomOutEvent;

    public delegate void ZoomedOutHandler();
    public static event ZoomedOutHandler ZoomedOut;

    public delegate void DuringTransition();
    public static event DuringTransition DuringTransitionEvent;

    public delegate void Skip(SceneData sceneData);
    public static event Skip SkipEvent;

    // VISUALS-SPECIFIC EVENTS (called in special cirumstances)

    public delegate void EnableBoundariesHandler(string[] names);
    public static event EnableBoundariesHandler EnableBoundaries;

    public delegate void SetRenderTextureHandler(string name);
    public static event SetRenderTextureHandler SetRenderTexture;

    public delegate void DisableRenderTextureHandler();
    public static event DisableRenderTextureHandler DisableRenderTexture;


    Vector3 adjustedCameraPosition;
    int currentSceneNumber; // the current scene destination number
    static bool isDuringTransition;
    TransitionType currentTransitionType;
    string currentAnimationClipName;
    float currentAnimationClipLength;
    float distanceFromAdjustedCameraPositionThreshold;
    Coroutine afterAnimationCoroutine;
    bool afterAnimationCoroutineIsRunning;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        currentSceneNumber = 1;
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        distanceFromAdjustedCameraPositionThreshold = 0.2f;
        afterAnimationCoroutineIsRunning = false;

        InitializeEvent?.Invoke();
    }

    // Returns the current scene number
    public int CurrentSceneNumber
    {
        get { return currentSceneNumber; }
        set { currentSceneNumber = value; }
    }

    public bool GetIsDuringTransition()
    {
        return isDuringTransition;
    }

    public TransitionType GetCurrentTransitionType()
    {
        return currentTransitionType;
    }

    public string CurrentAnimationClipName
    {
        get { return currentAnimationClipName; }
        set { currentAnimationClipName = value; }
    }

    //public void SetCurrentAnimationClipLength(float clipLength)
    //{
    //    currentAnimationClipLength = clipLength;
    //}

    // Adjusts all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). TransitionToAnotherScene() will handle the actual animation
    public void VisitPreviousScene()
    {
        Debug.Log("Before decrement: " + currentSceneNumber);
        if (currentSceneNumber > 1)
        {
            currentSceneNumber -= 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Backward;
            string[] startingnodes = sceneDataArray[currentSceneNumber - 1].startingNodes;
            string[] endingNodes = sceneDataArray[currentSceneNumber - 1].endingNodes;
            //call animateInactive on starting nodes, call animateActive on ending nodes
            VisitPreviousEvent?.Invoke(sceneDataArray[currentSceneNumber -1]);

            PlayTransition();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). PlayTransition() will handle the actual animation
    public float VisitNextScene()
    {
        Debug.Log("Before increment: " + currentSceneNumber);
        if (currentSceneNumber < sceneDataArray.Length)
        {
            currentSceneNumber += 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Forward;
            Debug.Log(sceneDataArray[currentSceneNumber-1].name);

            string[] startingnodes = sceneDataArray[currentSceneNumber - 1].startingNodes;
            string[] endingNodes = sceneDataArray[currentSceneNumber - 1].endingNodes;
            // call animateInactive on starting nodes, call animateInactive on ending nodes 

            VisitNextEvent?.Invoke(sceneDataArray[currentSceneNumber - 1]);
            PlayTransition();

            return currentAnimationClipLength;
        }
        return 0f;
    }

    public void ZoomInToCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Inward;
        //currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].ZoomInAnimationClipName;
        //currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].ZoomInAnimationClipLength;


        ZoomInEvent?.Invoke(sceneDataArray[currentSceneNumber - 1]);
        
        PlayTransition();
    }

    public void ZoomOutFromCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Outward;
       // currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].ZoomOutAnimationClipName;
       // currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].ZoomOutAnimationClipLength;
        

        ZoomOutEvent?.Invoke(sceneDataArray[currentSceneNumber - 1]);

        PlayTransition();
    }

    // Checks whether the skull needs to be adjusted first. Then, plays the appropriate transition animation clip.
    void PlayTransition()
    {
        //AdjustSkullPositionIfPastThreshold();

        if (!string.IsNullOrEmpty(currentAnimationClipName))
        {
            animator.Play(currentAnimationClipName, 0, 0f);
            DuringTransitionEvent?.Invoke();   
        }

        afterAnimationCoroutine = StartCoroutine(AfterAnimation());
    }

    void AdjustSkullPositionIfPastThreshold()
    {
        Vector3 currentCameraPosition = mainCamera.transform.position;
        if (Vector3.Distance(currentCameraPosition, adjustedCameraPosition) > distanceFromAdjustedCameraPositionThreshold)
        {
            Vector3 offset = new Vector3(currentCameraPosition.x - adjustedCameraPosition.x, currentCameraPosition.y - adjustedCameraPosition.y, currentCameraPosition.z - adjustedCameraPosition.z);
            headContainer.transform.position += offset;
            adjustedCameraPosition = mainCamera.transform.position;
        }
    }

    IEnumerator AfterAnimation()
    {
        afterAnimationCoroutineIsRunning = true;
        yield return new WaitForSeconds(currentAnimationClipLength);
        isDuringTransition = false;
        if (currentTransitionType == TransitionType.Outward)
        {
            //EnableBoundaries?.Invoke(sceneDataArray[currentSceneNumber - 1].boundaries);
        }
        ChangeButtonStatesAfterAnimationCompleted();
        currentTransitionType = TransitionType.None;
        currentAnimationClipName = "";
        currentAnimationClipLength = 0;
        afterAnimationCoroutineIsRunning = false;
    }

    void ChangeButtonStatesAfterAnimationCompleted()
    {
        if (currentTransitionType == TransitionType.Forward || currentTransitionType == TransitionType.Backward || currentTransitionType == TransitionType.Inward)
        {
            DefaultState?.Invoke();
        }
        else if (currentTransitionType == TransitionType.Outward)
        {
            ZoomedOut?.Invoke();
        }
    }

    public void SkipTransition()
    {
        if (afterAnimationCoroutineIsRunning)
        {
            StopCoroutine(afterAnimationCoroutine); /// you need this because you don't want this effect to take place unintentionally
            afterAnimationCoroutineIsRunning = false;
        }
        animator.Play(currentAnimationClipName, -1, 1);
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        currentAnimationClipName = "";
        currentAnimationClipLength = 0;
        DefaultState?.Invoke();

        SkipEvent?.Invoke(sceneDataArray[currentSceneNumber - 1]);
    }
}
