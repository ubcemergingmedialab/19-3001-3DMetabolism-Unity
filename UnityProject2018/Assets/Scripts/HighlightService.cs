using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// old highlight controller.
// 
public class HighlightService : MonoBehaviour
{

    private static HighlightService _instance;
    public static HighlightService Instance
    {
        get { return _instance; }
    }

    public GameObject UIContainer;


    void Awake()  
    {   
        // Singleton
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        pathwayHighlights = new Dictionary<PathwaySO, HighlightPathway>();         
        statusList = new Dictionary<HighlightHandler, List<HighlightPathway>>(); // key = nodes/edges , entry = list of pathways connected to it 
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // takes care of all the highlights management, including keeping track of all 3 states and coordinating changes

    // <> NOTE: it needs to change so that it access the HiglhightPAthways in Status Controller instead of pathwayhiglights in 52
    public void Highlight(PathwaySO targetPathway) {
        Debug.Log("calling highlight on " + targetPathway.name);

        // change state of target pathway, in case of accented, unaccent all other accented pathways:
        if(UIContainer != null) {
            pathwayHighlights.TryGetValue(targetPathway, out HighlightPathway found);
            if (found != null){
                switch (found.state)
                {
                    case HighlightPathway.HighlightState.Default:
                        found.SetHighlighted();
                        break;
                    case HighlightPathway.HighlightState.Highlighted:
                        found.SetAccented();
                        foreach(KeyValuePair<PathwaySO, HighlightPathway> entry in pathwayHighlights) {
                            if(entry.Value.state == HighlightPathway.HighlightState.Accented && entry.Value.pathwayToHighlight.name != targetPathway.name) {
                                entry.Value.SetHighlighted(); //downgrade all other accented pathways
                            }
                        }
                        break;
                    case HighlightPathway.HighlightState.Accented:
                        found.SetDefault();
                        break;
                    default:
                        break;
                }
            }
        }
    }


    // returns a list of renders of the highlighted pathways
    // <> needs to access the new highlightpathway through statusController ir change the statuslist
    public List<Renderer> GetHighlightedRenderers() {
        //declare renderer accumulator
        List<Renderer> highlightedRenderers = new List<Renderer>();
        //iterate over status list
        foreach (KeyValuePair<HighlightHandler, List<HighlightPathway>> pairHH in statusList) {
       
            List<HighlightPathway> currentList = pairHH.Value;                                          // List of pathways shared with HighlightHandler
            if ( currentList != null) {
                foreach ( HighlightPathway hlpw in currentList) {                                       // iterate through the pathways 

                    if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // if not highlighted , checks the next one
                        continue;                                                                       // check the next one
                    }
                    Renderer currentRenderer = pairHH.Key.transform.parent.GetComponent<Renderer>();    // if highlgihted, fid Renderer of HighlightPathway
                    highlightedRenderers.Add(currentRenderer);                                          // add Renderer to list 
                }
                    
            } else {
                Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
                throw new ArgumentNullException(nameof(currentList));
            }
        }

        return highlightedRenderers;
    }

    // returns the list of Bounds of highlighted Pathways 
    // <> needs to access the new highlightpathway through statusController
    public List<Bounds> GetHighlightedBounds() {
        //declare Bounds accumulator
        List<Bounds> highlightedBounds = new List<Bounds>();
        //iterate over status list
        foreach (KeyValuePair<HighlightHandler, List<HighlightPathway>> pairHH in statusList) {

            List<HighlightPathway> currentList = pairHH.Value;                                          // List of pathways shared with HighlightHandler

            if (currentList == null) {                                                                  // NULL access gate
                Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
                throw new ArgumentNullException(nameof(currentList));
            } else {

                foreach (HighlightPathway hlpw in currentList) {                                        // iterate through the pathways                                        
                    if (hlpw.state == HighlightPathway.HighlightState.Default) {                        // if not highlighted                                                                                 
                        continue;                                                                       // check the next one
                    }
                    Bounds currentBounds = pairHH.Key.transform.parent.GetComponent<Renderer>().bounds; // if highlgihted, find Renderer's Bounds of HighlightPathway
                    highlightedBounds.Add(currentBounds);                                               // add Bounds to list   
                }

            }
        }

        return highlightedBounds;
    }


    // takes a List of Renders (from HighlightedRenderers()) and returns a list of Bounds corresponding to the renderers
    public List<Bounds> getBounds(List<Renderer> renderers) {
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