using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// first point of contact between the buttons and the highlight Pipeline.
/// - Sends out the highligting message out to StatusController.
/// - gets the renderers/bounds of the highlighted pathways
/// - Sets the Camera focus to the highlighted pathways.
/// Singleton
/// </summary>
public class HighlightService : MonoBehaviour
{
    //SINGLETON
    private static HighlightService _instance;
    public static HighlightService Instance
    {
        get { return _instance; }
    }

    public GameObject UIContainer;

/// <summary>
/// Create the singleton instance
/// Hold only one active instance of this class
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

    /// <summary>
    /// Based on the current highlight state of target pathwaySO, calculate and set the new highlight state, update the camera focus
    /// </summary>
    /// <param name="targetPathway"> Pathway to highlight </param>
    /// 
    public void Highlight(PathwaySO targetPathway) {
        Debug.Log("calling highlight on " + targetPathway.name);
        
        if(UIContainer != null) {
            HighlightPathway.HighlightState? currentState = StatusController.Instance.PathwayCheckState(targetPathway);
            HighlightPathway.HighlightState newState = HighlightPathway.HighlightState.Default;

            if (currentState != null){

                switch (currentState)
                {
                    case HighlightPathway.HighlightState.Default:
                        newState = HighlightPathway.HighlightState.Highlighted;
                        break;

                    case HighlightPathway.HighlightState.Highlighted:
                        newState = HighlightPathway.HighlightState.Accented;
                        break;

                    case HighlightPathway.HighlightState.Accented:
                        newState = HighlightPathway.HighlightState.Default;
                        break;
                    default:
                        break;
                }

                StatusController.Instance.SetPathwayState(targetPathway,newState);
                GameObject.Find("MainCamera").GetComponent<CameraController>().MoveCameraToHighlightedTarget();
                //FocusController.Instance.UpdateFocus();
            }
        }
    }



    /// <summary>
    /// Get the renderer components of all highlighted pathways from the StatusController.elementToPathways Dictionary
    /// </summary>
    /// <returns>Renderers of the highlighted pathways</returns>
    /// <exception cref="ArgumentNullException"> Highlighted Pathways are NULL</exception>
    public List<Renderer> GetHighlightedRenderers() {
        //declare renderer accumulator
        List<Renderer> highlightedRenderers = new List<Renderer>();

        IDictionaryEnumerator Enumerator = StatusController.Instance.GetElementToPathwaysEnumerator();

        while (Enumerator.MoveNext()) {                                                                 // Moves on to the next pair in Dict
       
            List<HighlightPathway> currentList = (List<HighlightPathway>) Enumerator.Value;             // List of pathways shared with HighlightHandler
            if ( currentList != null) {
                foreach ( HighlightPathway hlpw in currentList) {                                       // Iterate through the pathways

                    if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // If not highlighted , checks the next one
                        continue;                                                                       // Check the next one
                    }
                    HighlightHandler HH = (HighlightHandler) Enumerator.Key;

                    Renderer currentRenderer = HH.transform.parent.GetComponent<Renderer>();            // If highlgihted, fid Renderer of HighlightPathway
                    highlightedRenderers.Add(currentRenderer);                                          // Add Renderer to list 
                }
                    
            } else {
                Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
                throw new ArgumentNullException(nameof(currentList));
            }
        }

        return highlightedRenderers;
    }


    /// <summary>
    /// Get the Bounds of the renderers of all the highlighted pathways
    /// </summary>
    /// <returns>Bounds of renderers of higlhighted pathways</returns>
    public List<Bounds> GetHighlightedBounds() {
        //declare Bounds accumulator
        List<Bounds> highlightedBounds = new List<Bounds>();
        IDictionaryEnumerator Enumerator = StatusController.Instance.GetElementToPathwaysEnumerator();  // Moves on to the next pair in Dict
        //iterate over status list
        while (Enumerator.MoveNext()) {

            List<HighlightPathway> currentList = (List<HighlightPathway>) Enumerator.Value;             // List of pathways shared with HighlightHandler

            if (currentList == null) {                                                                  // NULL access gate
                Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
                throw new ArgumentNullException(nameof(currentList));
            } else {

                foreach (HighlightPathway hlpw in currentList) {                                        // Iterate through the pathways                                        
                    if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // If not highlighted                                                                                 
                        continue;                                                                       // Check the next one
                    }
                    HighlightHandler HH = (HighlightHandler) Enumerator.Key;
                    Bounds currentBounds = HH.transform.parent.GetComponent<Renderer>().bounds;         // If highlgihted, find Renderer's Bounds of HighlightPathway
                    highlightedBounds.Add(currentBounds);                                               // Add Bounds to list   
                }
            }
        }

        return highlightedBounds;
    }


    // Takes a List of Renders (from HighlightedRenderers()) and returns a list of Bounds corresponding to the renderers !!!
    /// <summary>
    /// Get the bounds of a list of renderes
    /// </summary>
    /// <param name="renderers"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"> renderes are NULL </exception>
    public List<Bounds> GetBounds(List<Renderer> renderers) {
        // Null check
        if (renderers == null) {
            throw new ArgumentNullException(nameof(renderers));
        }

        List<Bounds> highlightedBounds = new List<Bounds>();
        
        foreach (Renderer renderer in renderers) {
            highlightedBounds.Add(renderer.bounds);
        }
        return highlightedBounds;
    }


}