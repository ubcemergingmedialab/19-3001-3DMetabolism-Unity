using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusController : MonoBehaviour
{
    private static FocusController _instance;
    public static FocusController Instance
    {
        get { return _instance; }
    }

    public Collider defaultCenter;
    public bool AutoCameraLock = false; // TO BE MADE PRIVATE, public only for demo purposes

    void Awake(){
        if (_instance != null && _instance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
        _instance = this;   
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// changes the focus to the aggregate view of the highlighted Pathways using GetHighlightedRenderers and CenterCamera functions
    public void UpdateFocus() {

        if (AutoCameraLock) {return;}
        List<Bounds> boundsList = HighlightService.Instance.GetHighlightedBounds();
        if (boundsList.Count == 0){
            Debug.Log(" no PW highlighted defaulting to defaultCenter");
            GetComponent<ClickListen>().ColliderCenterCamera(defaultCenter);
            return;
       }
        Bounds bounds = GetComponent<ClickListen>().BoundsEncapsulate(boundsList);
        
        GetComponent<ClickListen>().CenterCamera(bounds);

    }

    public void SetAutoLock(bool state){
        AutoCameraLock = state;
    }
}




    //  RENDER VERSION OF UPDATE FOCUS
    //    List<Renderer> renderers  = HighlightService.Instance.GetHighlightedRenderers();
    //    if (renderers.Count == 0){
    //        Debug.Log(" Render List is EMPTY!");
    //        renderers.Add(defaultCenter.GetComponent<Renderer>());
    //    } 
    
    //    GetComponent<ClickListen>().CenterCamera(renderers);