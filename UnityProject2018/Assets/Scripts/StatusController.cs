using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  NOTE: UPDATE IF FUNCTIONALITY CHANGES
    2021-10-07 
        This Script is a Centralized point of access for Any Status within the program. This includes:
        Active pathways, pathway Highlight states, favourite pathways etc.
        currently getting optimized for Handeling highlights. functions/fields to be added:
            - node/edge Status lists
            - ElementCheckState
            - PAthwaySOCheckState
            - Calc max/ min highlight state -> possibly in HighlightService
            - list of active PAthways
            - intialize a highlightPathway per pathwaySO ( dict<PWSO, HighlightPAthway)
*/
public class StatusController : MonoBehaviour
{
    //SINGELTON
    private static StatusController _instance;
    public static StatusController Instance
    {
        get { return _instance; }
    }

    //fields
    GameObject UIContainer;
    private Dictionary<HighlightHandler, List<HighlightPathway>> elementsNetwork; // key = nodes/edges , entry = list of pathways connected to it
    private Dictionary<PathwaySO, HighlightPathway> highlightPathways;            // Status controller intializes and connects each highlitpathway to the pathwaySOs

    public List<PathwaySO> activePathways;                                      // <> to be filled manually in unity 

    void Awake(){
        if (_instance != null && _instance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
        _instance = this;   
        DontDestroyOnLoad(this.gameObject);

        elementsNetwork = new Dictionary<HighlightHandler, List<HighlightPathway>>();

        // fill the elements network 
        // NOTE: <> currently set up assuming that HighlightPathway is extending MonoBehaviour

        if(UIContainer == null) {
            Debug.Log("StatusController needs UIContainer to find HighlightPathway Components");

        } else {
            foreach(HighlightPathway pathwayHL in UIContainer.GetComponentsInChildren<HighlightPathway>()){ 
                pathwayHighlights.Add(pathwayHL.pathwayToHighlight, pathwayHL);                     // Adds all the highlighted to dictionary of pathwayHighlights

                foreach(NodeSO nodeSO in pathwayHL.pathwayToHighlight.nodes) {                      // For every node in this pathway
                    GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);
                    foreach(GameObject node in nodes) {
                        if(node != null) {
                        HighlightHandler hl = node.GetComponent<HighlightHandler>();
                        List<HighlightPathway> sharingPathways;                                               // PathwayHighlightlist
                            if(elementsNetwork.TryGetValue(hl, out sharingPathways)) {                        // Find node in elementsNetwork
                                sharingPathways.Add(pathwayHL);
                            } else {
                                elementsNetwork.Add(hl, new List<HighlightPathway>{pathwayHL});          // Make a new list if node is not yet in the Dictionary 
                            }
                        }
                    }
                }

                foreach(EdgeSO edgeSO in pathwayHL.pathwayToHighlight.edges) {                      // For every edge in this pathway
                    GameObject[] edges = GameObject.FindGameObjectsWithTag(edgeSO.name);
                    foreach(GameObject edge in edges){
                        if(edge != null) {
                            HighlightHandler hl = edge.GetComponent<HighlightHandler>();
                            List<HighlightPathway> sharingPathways;                                           // PathwayHighlightlist
                                if(elementsNetwork.TryGetValue(hl, out sharingPathways)) {                         // Find edge in elementsNetwork
                                    sharingPathways.Add(pathwayHL);
                                } else {
                                    elementsNetwork.Add(hl, new List<HighlightPathway>{pathwayHL});      // Make a new list if edge is not yet in the Dictionary
                                }
                        }
                    }
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (PathwaySO pathway in activePathways){
            // initialize a highlightPathway per active pathway
            HighlightPathway highlightPathway = new HighlightPathway(pathway);
            highlightPathways.Add(new KeyValuePair<PathwaySO, HighlightPathway>(pathway,highlightPathway));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HighlightPathway.HighlightState ElementCheckState(HighlightHandler highlightHandler) {
        
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        elementsNetwork.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

        if ( currentList != null) {
            foreach ( HighlightPathway hlpw in currentList) {
                HighlightPathway.HighlightState newState = hlpw.state;
                    if ( newState > tempState) {
                        tempState = newState;
                    }
            }
        } else {
            Debug.Log("no pathwaylist are to be found on the stateList Dictionary (NULL access)");
        }
        return tempState;
    }

// <> returns the current state of a given pathway through its SO
    public HighlightPathway.HighlightState PathwayCheckState(PathwaySO pathway){
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        highlightPathways.TryGetValue(pathway, out List<HighlightPathway> highlightpathway);

        if (highlightpathway != null ){
            tempState = highlightpathway.state;
        } else{
            Debug.LogError("no highlight pathway linked to this PathwaySO");
        }

        return tempState;
    }
}
