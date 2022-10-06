using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  NOTE: UPDATE IF FUNCTIONALITY CHANGES
    2021-10-14 
        This Script is a Centralized point of access for Any Status within the program. This includes:
        Active pathways, pathway Highlight states, favourite pathways etc.
        currently getting optimized for Handeling highlights. functions/fields related to highlighting to be added:
            done:
            - Calc max/ min highlight state -> ElementCheckState 
            - node/edge Status lists
            - ElementCheckState
            - PathwaySOCheckState
            - list of active Pathways
            - intialize a highlightPathway per pathwaySO ( dict<PWSO, HighlightPAthway>)

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
    private Dictionary<HighlightHandler, List<HighlightPathway>> elementToPathways;     // key = nodes/edges hh , entry = list of highlightPathways connected to it
    private Dictionary<PathwaySO, HighlightPathway> highlightByPathwaySO;               // PathwaySO linked to its HighlightPathway Instance
    private List<HighlightPathway> highlightPathways;                                   // list of all highlightPathways initialized

    public List<PathwaySO> activePathways;                                              // filled now using th query service editor (was fiiled manual in unity previously)
    
    public ConnectionsSO globalPathway;

    //temp nodes edge list for testing
    public List<EdgeSO> AllEdgeSOs;
    public List<NodeSO> AllNodeSOs;



    public GameObject tempObjectHolder;                                                 // temporary, manually testing highlighting till buttons are developed
    int num = 0; //temp
    
    /*
    On Awake() StatusController does the following :
        - Initialization of the fields
        - instantiates a highlightPathway instance per a PathwaySO and links them in the highlightByPathwaySO Dictionary
        - keeps a list of all HighlightPathway instances
        - for every node/edge, it grabs the HighlightHandler component and the list of pathways it is apart of and links them in elementsToPathways dict
    */
    void Awake() 
    {
        if (_instance != null && _instance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
        _instance = this;   
        DontDestroyOnLoad(this.gameObject);
        

        elementToPathways = new Dictionary<HighlightHandler, List<HighlightPathway>>();
        highlightByPathwaySO = new Dictionary<PathwaySO, HighlightPathway>();
        highlightPathways = new List<HighlightPathway>();

         if (activePathways == null || activePathways.Count == 0) {
            Debug.LogError("StatusCtrl: activePathways is null/empty ");
        }


        // Fill the elements network 
        foreach (PathwaySO pathwaySO in this.activePathways) {

            if (pathwaySO == null){
                Debug.LogError("<!> Status controller : pathway scriptable object in active pathways is NULL");
            }
            
            HighlightPathway highlightPathway = new HighlightPathway(pathwaySO);                                    // initialize a highlightPathway per active pathway
            highlightByPathwaySO.Add(pathwaySO,highlightPathway);                                                   // link the pathwaySO to its highlightPathway
            highlightPathways.Add(highlightPathway);                                                                // add the new highlight pathway to the list that keeps track of them
            

            List<NodeSO> listOfNodes = new List<NodeSO>();                                                          
            List<EdgeSO> listOfEdges = new List<EdgeSO>();

            IDictionaryEnumerator networkEnumerator = pathwaySO.GetLocalNetworkEnumerator();
            
            if (pathwaySO.LocalNetwork != null){
               while(networkEnumerator.MoveNext()){
                    listOfNodes.Add( (NodeSO) networkEnumerator.Key); 
                    listOfEdges.AddRange( (List<EdgeSO>) networkEnumerator.Value);
                }
                //networkEnumerator.Reset();
            }else{
                Debug.LogError("<!>  StatusController : Local network in pathway is empty");
            }

                 

            foreach(NodeSO nodeSO in listOfNodes) {                                                                 // For every nodeSO in this pathway
                GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);

                foreach(GameObject node in nodes) {
                    if(node != null) {
                        HighlightHandler hl = node.GetComponent<HighlightHandler>();                                // find the nodes handler
                        if(hl == null) {
                            Debug.LogError("StatusController :higlightHandler is null ");
                        }
                        List<HighlightPathway> sharingPathways;                                                     

                        if(elementToPathways.TryGetValue(hl, out sharingPathways)) {                                // Find node in elementToPathways
                            sharingPathways.Add(highlightPathway);                                                  // add the highlightPathway to the element's list of hpw
                        } else {
                            elementToPathways.Add(hl, new List<HighlightPathway>{highlightPathway});                // Make a new list if node is not yet in the Dictionary 
                        }
                    }
                }
            }

                                              

            foreach(EdgeSO edgeSO in listOfEdges) {                                                             // For every edge in this pathway (same proccess as nodes)
                GameObject[] edges = GameObject.FindGameObjectsWithTag(edgeSO.name);

                foreach(GameObject edge in edges){
                    if(edge != null) {
                        HighlightHandler hl = edge.GetComponent<HighlightHandler>();
                        List<HighlightPathway> sharingPathways; 

                        if(elementToPathways.TryGetValue(hl, out sharingPathways)) {                  
                            sharingPathways.Add(highlightPathway);
                        } else {
                            elementToPathways.Add(hl, new List<HighlightPathway>{highlightPathway});      
                        }
                    }
                }
            }

            pathwaySO.FillLists();
        }
    }

    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {   
        // PipelineTest();             // for manual testing of the highlight functionality
        
    }

    // Given a PathwaySO and State, find the HighlightPathway Instance of the Pathway and calls Set<state>() for said pathway. 
    // If accented, it sets all the others into single higlhighted
    public void SetPathwayState(PathwaySO targetPathwaySO, HighlightPathway.HighlightState state){
        HighlightPathway highlightPathway = GetHighlightByPathwaySO(targetPathwaySO);

        if(highlightPathway != null){

            switch(state)
            {
                case HighlightPathway.HighlightState.Default:
                    highlightPathway.SetDefault();
                    
                    break;
                
                case HighlightPathway.HighlightState.Highlighted:
                    highlightPathway.SetHighlighted();
                    break;

                case HighlightPathway.HighlightState.Accented:
                    highlightPathway.SetAccented();
                    foreach(KeyValuePair<PathwaySO, HighlightPathway> entry in highlightByPathwaySO) {
                        if(entry.Value.state == HighlightPathway.HighlightState.Accented && entry.Value.pathwayToHighlight.name != targetPathwaySO.name) {
                            entry.Value.SetHighlighted(); //downgrade all other accented pathways
                        }
                    }
                    break;
                        default:
                            break;
            }
        } else {
            Debug.LogError("<!> StatusCtrl.SetPathwayState(), null highlightPathway");
        }

    }


    // Returns the max state of an element (node/edge) based on the pathway its connected to. (Accent > Highlighted > Default)
    public HighlightPathway.HighlightState ElementCheckState(HighlightHandler highlightHandler) {
        
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
         if (highlightHandler ==null) {
            Debug.LogError("StatusController.ElementCheckState : pathway null");
        }   

        if (highlightByPathwaySO == null) {
            Debug.LogError("StatusController.ElementCheckState : highlightByPathwaySO dictionary empty");
        }
        elementToPathways.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

        if ( currentList != null) {
            foreach ( HighlightPathway hlpw in currentList) {
                HighlightPathway.HighlightState newState = hlpw.state;
                    if ( newState > tempState) {
                        tempState = newState;
                    }
            }
        } else {
            Debug.Log("StatusController.ElementCheckState : no pathwaylist are to be found on the elementToPathways Dictionary (NULL access)");
        }
        return tempState;
    }

    // Returns the current state of a pathway given a PathwaySO
    public HighlightPathway.HighlightState PathwayCheckState(PathwaySO pathway) {
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        highlightByPathwaySO.TryGetValue(pathway, out HighlightPathway highlightpathway);

        if (highlightpathway != null ){
            tempState = highlightpathway.state;
        } else{
            Debug.LogError(" StatusController.PathwayCheckState : no highlight pathway linked to this PathwaySO");
        }

        return tempState;
    }

    // Returns an Enumarator for the elementToPathways Dictionary, (for remote traversing)
    public IDictionaryEnumerator GetElementToPathwaysEnumerator(){
        return elementToPathways.GetEnumerator();
    }

    // Returns the count of elementToPathways Dictionary;
    public int GetCountElementToPathways(){
        return highlightByPathwaySO.Count;
    }

    // Given a HighlightHandler (node/edge) , Returns the ListOf HighlightPathway the element is apart of
    public List<HighlightPathway> GetElementToPathways(HighlightHandler highlightHandler) {
        
        elementToPathways.TryGetValue(highlightHandler, out List<HighlightPathway> listOfHPW);

        return listOfHPW;
    }

    // Given a PathwaySO, Returns its linked HighlightPathway instance
    public HighlightPathway GetHighlightByPathwaySO(PathwaySO pathwaySO){
        highlightByPathwaySO.TryGetValue(pathwaySO, out HighlightPathway value);
        return value;
    }

    // fills up all nodes and edges list in status controller in order to hold a ref to the SOs , from active pathways.
    // not currently used
    public void FillItemReferenceList(){
        
        foreach(PathwaySO pw in activePathways){
            IDictionaryEnumerator networkEnumerator = pw.GetLocalNetworkEnumerator();
            if (pw.LocalNetwork != null){
               while(networkEnumerator.MoveNext()){
                    AllNodeSOs.Add( (NodeSO) networkEnumerator.Key); 
                    AllEdgeSOs.AddRange( (List<EdgeSO>) networkEnumerator.Value);
                }
            }
        
        }
    }

// function for manually testing the higlhight pipeline with num keys activating pathways in the pathway list
    // private void PipelineTest(){
    //     //0
    //     if (Input.GetKeyDown(KeyCode.Alpha0)) {
    //     num = 0;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //1
    //     if (Input.GetKeyDown(KeyCode.Alpha1)) {
    //     num = 1;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //2
    //     if (Input.GetKeyDown(KeyCode.Alpha2)) {
    //     num = 2;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //3
    //     if (Input.GetKeyDown(KeyCode.Alpha3)) {
    //     num = 3;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //4
    //     if (Input.GetKeyDown(KeyCode.Alpha4)) {
    //     num = 4;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //5
    //     if (Input.GetKeyDown(KeyCode.Alpha5)) {
    //     num = 5;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }
    //     //6
    //     if (Input.GetKeyDown(KeyCode.Alpha6)) {
    //     num = 6;
    //     tempObjectHolder.GetComponentInChildren<HighlightService>().Highlight(activePathways[num]);
    //     }

    // }
}
