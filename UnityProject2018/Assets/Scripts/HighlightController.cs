using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{

    private static HighlightController _instance;
    public static HighlightController Instance
    {
        get { return _instance; }
    }

    public GameObject UIContainer;
    private Dictionary<PathwaySO, HighlightPathway> pathwayHighlights;
    // Start is called before the first frame update
    private Dictionary<HighlightHandler, List<HighlightPathway>> statusList;

    void Awake()  
    {   // Singleton
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
        if(UIContainer == null) {
            Debug.Log("HighlightController needs UIContainer to find HighlightPathway Components");
        } else {
            foreach(HighlightPathway pathwayHL in UIContainer.GetComponentsInChildren<HighlightPathway>()){ 
                pathwayHighlights.Add(pathwayHL.pathwayToHighlight, pathwayHL);                     // Adds all the highlighted to dictionary of pathwayHighlights

                foreach(NodeSO nodeSO in pathwayHL.pathwayToHighlight.nodes) {                      // For every node in this pathway
                    GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);
                    foreach(GameObject node in nodes) {
                        if(node != null) {
                        HighlightHandler hl = node.GetComponent<HighlightHandler>();
                        List<HighlightPathway> pwhls;                                               // PathwayHighlightlist
                            if(statusList.TryGetValue(hl, out pwhls)) {                             // Find node in StatusList
                                pwhls.Add(pathwayHL);
                            } else {
                                statusList.Add(hl, new List<HighlightPathway>{pathwayHL});          // Make a new list if node is not yet in the Dictionary 
                            }
                        }
                    }
                }

                foreach(EdgeSO edgeSO in pathwayHL.pathwayToHighlight.edges) {                      // For every edge in this pathway
                    GameObject[] edges = GameObject.FindGameObjectsWithTag(edgeSO.name);
                    foreach(GameObject edge in edges){
                        if(edge != null) {
                            HighlightHandler hl = edge.GetComponent<HighlightHandler>();
                            List<HighlightPathway> pwhls;                                           // PathwayHighlightlist
                                if(statusList.TryGetValue(hl, out pwhls)) {                         // Find edge in StatusList
                                    pwhls.Add(pathwayHL);
                                } else {
                                    statusList.Add(hl, new List<HighlightPathway>{pathwayHL});      // Make a new list if edge is not yet in the Dictionary
                                }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // takes care of all the highlights management, including keeping track of all 3 states and coordinating changes
    public void Highlight(PathwaySO targetPathway) {
        Debug.Log("calling highlight on " + targetPathway.name);

        // change state of target pathway, in case of accented, unaccent all other accented pathways:
        if(UIContainer != null) {
            pathwayHighlights.TryGetValue(targetPathway, out HighlightPathway found);
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

    // Checks the Highlight state of the node/edge of arg (HighlightHandler), and sets the state to be the one with utmost priority (Accent > Highlighted > Default)
    // Assumes that the highlight States of Pathways are already up to date
    public HighlightPathway.HighlightState CheckState(HighlightHandler highlightHandler) {
        
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        statusList.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

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
}

