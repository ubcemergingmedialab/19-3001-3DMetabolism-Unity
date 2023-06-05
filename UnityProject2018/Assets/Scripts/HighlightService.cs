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
            HighlightPathway.HighlightState currentState = StatusController.Instance.PathwayCheckState(targetPathway);
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
                GameObject.Find("MainCamera").GetComponent<CameraController>().MoveCameraToTarget();
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







// HIGHLIGHT CONTROLLER"S OLD FUNCTIONS

    // private Dictionary<PathwaySO, HighlightPathway> pathwayHighlights;

    // private Dictionary<HighlightHandler, List<HighlightPathway>> statusList;

    // public List<PathwaySO> activePathways;


    //void Start()
    //{
        // if(UIContainer == null) {
        //     Debug.LogError("HighlightService needs UIContainer to find HighlightPathway Components");
        // } else {
        //     foreach(HighlightPathway pathwayHL in UIContainer.GetComponentsInChildren<HighlightPathway>()){ 
        //         pathwayHighlights.Add(pathwayHL.pathwayToHighlight, pathwayHL);                     // Adds all the highlighted to dictionary of pathwayHighlights

        //         foreach(NodeSO nodeSO in pathwayHL.pathwayToHighlight.nodes) {                      // For every node in this pathway
        //             GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);
        //             foreach(GameObject node in nodes) {
        //                 if(node != null) {
        //                 HighlightHandler hl = node.GetComponent<HighlightHandler>();
        //                 List<HighlightPathway> pwhls;                                               // PathwayHighlightlist
        //                     if(statusList.TryGetValue(hl, out pwhls)) {                             // Find node in StatusList
        //                         pwhls.Add(pathwayHL);
        //                     } else {
        //                         statusList.Add(hl, new List<HighlightPathway>{pathwayHL});          // Make a new list if node is not yet in the Dictionary 
        //                     }
        //                 }
        //             }
        //         }

        //         foreach(EdgeSO edgeSO in pathwayHL.pathwayToHighlight.edges) {                      // For every edge in this pathway
        //             GameObject[] edges = GameObject.FindGameObjectsWithTag(edgeSO.name);
        //             foreach(GameObject edge in edges){
        //                 if(edge != null) {
        //                     HighlightHandler hl = edge.GetComponent<HighlightHandler>();
        //                     List<HighlightPathway> pwhls;                                           // PathwayHighlightlist
        //                         if(statusList.TryGetValue(hl, out pwhls)) {                         // Find edge in StatusList
        //                             pwhls.Add(pathwayHL);
        //                         } else {
        //                             statusList.Add(hl, new List<HighlightPathway>{pathwayHL});      // Make a new list if edge is not yet in the Dictionary
        //                         }
        //                 }
        //             }
        //         }
        //     }
        // }
    //}

    // Checks the Highlight state of the node/edge of arg (HighlightHandler), and sets the state to be the one with utmost priority (Accent > Highlighted > Default)
    // Assumes that the highlight States of Pathways are already up to date

    // public HighlightPathway.HighlightState CheckState(HighlightHandler highlightHandler) {
        
    //     HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
    //     statusList.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

    //     if ( currentList != null) {
    //         foreach ( HighlightPathway hlpw in currentList) {
    //             HighlightPathway.HighlightState newState = hlpw.state;
    //                 if ( newState > tempState) {
    //                     tempState = newState;
    //                 }
    //         }
    //     } else {
    //         Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
    //     }
    //     return tempState;
    // }



    // private List<Renderer> helper(KeyValuePair<HighlightHandler, List<HighlightPathway>> pair) {

    //     List<Renderer> highlightedRenderers = new List<Renderer>();
    //     List<HighlightPathway> currentList = pair.Value;                                                // List of pathways shared with HighlightHandler
    //         if ( currentList != null) {
    //             foreach ( HighlightPathway hlpw in currentList) {                                       // iterate through the pathways

    //                 if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // if not highlighted , checks the next one
    //                     continue;                                                                       // check the next one
    //                 }
    //                 Renderer currentRenderer = pair.Key.transform.parent.GetComponent<Renderer>();    // if highlgihted, fid Renderer of HighlightPathway
    //                 highlightedRenderers.Add(currentRenderer);                                          // add Renderer to list 
    //             }
                    
    //         } else {
    //             Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
    //             throw new ArgumentNullException(nameof(currentList));
    //         }

    //         return highlightedRenderers; 
    // }





        // public List<Bounds> GetHighlightedBounds() {
        // //declare Bounds accumulator
        // List<Bounds> highlightedBounds = new List<Bounds>();
        // Dictionary<HighlightHandler, List<HighlightPathway>> localElemPath; //!!!!!
        // localElemPath = UIContainer.GetComponent<StatusController>().GetElementToPathways(); //<!>
        // //iterate over status list
        // foreach (KeyValuePair<HighlightHandler, List<HighlightPathway>> pairHH in GetElementToPathways() ) {

        //     List<HighlightPathway> currentList = pairHH.Value;                                          // List of pathways shared with HighlightHandler

        //     if (currentList == null) {                                                                  // NULL access gate
        //         Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
        //         throw new ArgumentNullException(nameof(currentList));
        //     } else {

        //         foreach (HighlightPathway hlpw in currentList) {                                        // iterate through the pathways                                        
        //             if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // if not highlighted                                                                                 
        //                 continue;                                                                       // check the next one
        //             }
        //             Bounds currentBounds = pairHH.Key.transform.parent.GetComponent<Renderer>().bounds; // if highlgihted, find Renderer's Bounds of HighlightPathway
        //             highlightedBounds.Add(currentBounds);                                               // add Bounds to list   
        //         }

        //     }
        // }

        // return highlightedBounds;
    