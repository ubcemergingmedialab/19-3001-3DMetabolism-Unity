using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// triggers the Automatic camera movement on to highlighted pathways.
/// Also has a camera lock ,where the automatic focus is disabled when locked
/// </summary>
public class FocusController : MonoBehaviour
{
    public Collider defaultCenter;
    private bool AutoCameraLock = false;

    //SINGLETON
    private static FocusController _instance;
    public static FocusController Instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Create the singleton instance. Hold only one active instance of this class
    /// </summary>
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

    //Create new UpdateFocus() method public that
    //takes in bounds (can come from the renderer component) then pass it in to the clicklisten. See UpdateFocus() for ex.
    //this will focus the camera to the resulting elements of the search results (rather than an entire pathway)
    public void UpdateFocus(List<Bounds> boundsList)
    {
        if (AutoCameraLock) { return; }
        if (boundsList.Count == 0)
        {
            Debug.Log(" no PW highlighted defaulting to defaultCenter");
            GetComponent<ClickListen>().ColliderCenterCamera(defaultCenter);
            return;
        }

        Bounds bounds = GetComponent<ClickListen>().BoundsEncapsulate(boundsList);
        GetComponent<ClickListen>().CenterCamera(bounds);
    }

    /// <summary>
    /// changes the focus to the aggregate view of the highlighted Pathways using GetHighlightedRenderers and CenterCamera functions
    /// </summary>
    public void UpdateFocus()
    {

        if (AutoCameraLock) { return; }
        List<Bounds> boundsList = HighlightService.Instance.GetHighlightedBounds();
        if (boundsList.Count == 0)
        {
            Debug.Log(" no PW highlighted defaulting to defaultCenter");
            GetComponent<ClickListen>().ColliderCenterCamera(defaultCenter);
            return;
        }
        Bounds bounds = BoundsEncapsulate(boundsList);
        //GetComponent<ClickListen>().CenterCamera(bounds);
        GetComponent<CameraController>().MoveCameraToTarget();
    }

    /// <summary>
    /// switches the boolean of the AutoCameraLock. ie locks and unlocks the camera movement
    /// </summary>
    public void SetAutoLock()
    {
        AutoCameraLock = (!AutoCameraLock);
    }

    /// <summary>   
    /// encapsulates all the input bounds into one 
    /// used when new pathways are highlighted and need to be in camera
    /// </summary>
    /// <param name="targetBoundsList"></param>
    /// <returns>encapsulated bounds of all the input bounds</returns>
    private Bounds BoundsEncapsulate(List<Bounds> targetBoundsList)
    {
        Bounds bounds = new Bounds();

        for (int index = 0; index < targetBoundsList.Count; index++)
        {
            if (index == 0)
            {
                bounds = targetBoundsList[index];
            }
            else
            {
                bounds.Encapsulate(targetBoundsList[index]);                        // encaplsulate all the bounds into one bound
            }
        }
        return bounds;
    }
}



//  RENDER VERSION OF UPDATE FOCUS
//    List<Renderer> renderers  = HighlightService.Instance.GetHighlightedRenderers();
//    if (renderers.Count == 0){
//        Debug.Log(" Render List is EMPTY!");
//        renderers.Add(defaultCenter.GetComponent<Renderer>());
//    } 

//    GetComponent<ClickListen>().CenterCamera(renderers);