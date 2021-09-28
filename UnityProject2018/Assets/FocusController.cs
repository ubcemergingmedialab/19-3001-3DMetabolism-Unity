using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusController : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider defaultCenter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// changes the focus to the aggregate view of the highlighted Pathways using GetHighlightedRenderers and CenterCamera functions
    public void UpdateFocus() {
        List<Bounds> boundsList = HighlightController.Instance.GetHighlightedBounds();
        if (boundsList.Count == 0){
            Debug.Log(" no PW highlighted defaulting to defaultCenter");
            GetComponent<ClickListen>().ColliderCenterCamera(defaultCenter);
            return;
       }
        Bounds bounds = GetComponent<ClickListen>().BoundsEncapsulate(boundsList);
        
        GetComponent<ClickListen>().CenterCamera(bounds);
        
    //  RENDER VERSION OF UPDATE FOCUS
    //    List<Renderer> renderers  = HighlightController.Instance.GetHighlightedRenderers();
    //    if (renderers.Count == 0){
    //        Debug.Log(" Render List is EMPTY!");
    //        renderers.Add(defaultCenter.GetComponent<Renderer>());
    //    } 
    
    //    GetComponent<ClickListen>().CenterCamera(renderers);
    }
}