using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2021-10-14
/// Centeralized point of access for status of SOs on all levels, pathways or components.
/// - Calc max/ min highlight state -> ElementCheckState 
/// - Element and Pathway CheckState and Status lists
/// - List of active Pathways
/// - Intialize a highlightPathway per pathwaySO ( dict (PWSO, HighlightPAthway))
/// </summary>
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

    private List<PathwaySO> searchResultPathwaySOList;                                  //List of created PathwaySO from search results.  Used to remove itself from highlightByPathwaySO when finished
    private List<HighlightPathway> searchResultHighlightPathwayList;                    //List of created Highlights from search results.  Remove from highlightPathways when done.

    public List<PathwaySO> activePathways;                                              // filled now using th query service editor (was fiiled manual in unity previously)

    public ConnectionsSO globalPathway;

    //temp nodes edge list for testing
    public List<EdgeSO> AllEdgeSOs;
    public List<NodeSO> AllNodeSOs;

    public GameObject tempObjectHolder;                                                 // temporary, manually testing highlighting till buttons are developed

    /// <summary>
    /// - Initialization of the fields
    /// - instantiates a highlightPathway instance per a PathwaySO and links them in the highlightByPathwaySO Dictionary
    /// - keeps a list of all HighlightPathway instances
    /// - for every node/edge, it grabs the HighlightHandler component and the list of pathways it is apart of and links them in elementsToPathways dict
    /// - Create the singleton instance. Hold only one active instance of this class
    /// The main step in intialization and accessibility of the network's backbone (components)
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

        elementToPathways = new Dictionary<HighlightHandler, List<HighlightPathway>>();
        highlightByPathwaySO = new Dictionary<PathwaySO, HighlightPathway>();
        highlightPathways = new List<HighlightPathway>();

        if (activePathways == null || activePathways.Count == 0)
        {
            Debug.LogError("StatusCtrl: activePathways is null/empty ");
        }


        // Fill the elements network 
        foreach (PathwaySO pathwaySO in this.activePathways)
        {

            if (pathwaySO == null)
            {
                Debug.LogError("<!> Status controller : pathway scriptable object in active pathways is NULL");
            }

            //add pathway to highlight, not from search function
            AddPathwayToHighlight(pathwaySO, false);
        }
    }

    public void ResetSearchPathwayHighlights()
    {
        if (null != searchResultHighlightPathwayList && searchResultHighlightPathwayList.Count != 0)
        {
            for (int i = 0; i < searchResultHighlightPathwayList.Count; i++)
            {
                HighlightPathway hp = searchResultHighlightPathwayList[i];
                PathwaySO pathwaySO = searchResultPathwaySOList[i];
                if (hp != null && pathwaySO != null)
                {
                    //Takes the current state, and set it again
                    SetPathwayState(pathwaySO, hp.state);
                }
            }
        }
    }

    /// <summary>
    /// - instantiates a highlightPathway instance per a PathwaySO and links them in the highlightByPathwaySO Dictionary
    /// - keeps a list of all HighlightPathway instances
    /// - for every node/edge, it grabs the HighlightHandler component and the list of pathways it is apart of and links them in elementsToPathways dict
    /// - maintains two lists of search results used to highlight pathways (searchResultPathwaySOList and searchResultPathwaySOList).  List will not be used for normal pathways (from wikibase)
    /// </summary>
    /// <param name="inputPathway">The created PathwaySO pathway from search</param>
    /// <param name="isFromSearch">Is the created inputPathway from search</param>
    public void AddPathwayToHighlight(PathwaySO inputPathway, bool isFromSearch)
    {
        HighlightPathway highlightPathway = new HighlightPathway(inputPathway);                                    // initialize a highlightPathway per active pathway
        highlightByPathwaySO.Add(inputPathway, highlightPathway);                                                   // link the pathwaySO to its highlightPathway
        highlightPathways.Add(highlightPathway);                                                                // add the new highlight pathway to the list that keeps track of them

        //add it this list (because these were artificially generated from search results)
        //to be removed later
        if (isFromSearch)
        {
            if (null == searchResultPathwaySOList)
            {
                searchResultPathwaySOList = new List<PathwaySO>();

            }
            if (null == searchResultHighlightPathwayList)
            {
                searchResultHighlightPathwayList = new List<HighlightPathway>();
            }
            searchResultPathwaySOList.Add(inputPathway);
            searchResultHighlightPathwayList.Add(highlightPathway);
        }

        List<NodeSO> listOfNodes = new List<NodeSO>();
        List<EdgeSO> listOfEdges = new List<EdgeSO>();

        IDictionaryEnumerator networkEnumerator = inputPathway.GetLocalNetworkEnumerator();

        if (inputPathway.LocalNetwork != null)
        {
            while (networkEnumerator.MoveNext())
            {
                listOfNodes.Add((NodeSO)networkEnumerator.Key);
                listOfEdges.AddRange((HashSet<EdgeSO>)networkEnumerator.Value);
            }
            //networkEnumerator.Reset();
        }
        else
        {
            Debug.LogError("<!>  StatusController : Local network in pathway is empty");
        }


        foreach (NodeSO nodeSO in listOfNodes)
        {                                                                 // For every nodeSO in this pathway
            GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeSO.name);

            foreach (GameObject node in nodes)
            {
                if (node != null)
                {
                    HighlightHandler hl = node.GetComponent<HighlightHandler>();                                // find the nodes handler
                    if (hl == null)
                    {
                        Debug.LogError("StatusController :higlightHandler is null ");
                    }
                    List<HighlightPathway> sharingPathways;

                    if (elementToPathways.TryGetValue(hl, out sharingPathways))
                    {                                // Find node in elementToPathways
                        sharingPathways.Add(highlightPathway);                                                  // add the highlightPathway to the element's list of hpw
                    }
                    else
                    {
                        elementToPathways.Add(hl, new List<HighlightPathway> { highlightPathway });                // Make a new list if node is not yet in the Dictionary 
                    }
                }
            }
        }

        foreach (EdgeSO edgeSO in listOfEdges)
        {                                                             // For every edge in this pathway (same proccess as nodes)
            GameObject[] edges = GameObject.FindGameObjectsWithTag(edgeSO.name);

            foreach (GameObject edge in edges)
            {
                if (edge != null)
                {
                    HighlightHandler hl = edge.GetComponent<HighlightHandler>();
                    List<HighlightPathway> sharingPathways;

                    if (elementToPathways.TryGetValue(hl, out sharingPathways))
                    {
                        sharingPathways.Add(highlightPathway);
                    }
                    else
                    {
                        elementToPathways.Add(hl, new List<HighlightPathway> { highlightPathway });
                    }
                }
            }
        }
        inputPathway.FillLists();
    }

    /// <summary>
    /// Finds the highlight object for each pathway from search, then sets it to the default state.
    /// After, it will be removed from the list in highlightPathways and highlightByPathwaySO.
    /// Those lists need to be maintained otherwise list will forever grow.
    /// </summary>
    public void ResetHighlightingFromSearchResultPathways()
    {
        //Remove added pathwaySO and highlightpathways that came from search results
        if (searchResultPathwaySOList != null && searchResultHighlightPathwayList != null)
        {
            foreach (PathwaySO pathwaySO in searchResultPathwaySOList)
            {
                highlightByPathwaySO.Remove(pathwaySO);
            }

            foreach (HighlightPathway highlightPathway in searchResultHighlightPathwayList)
            {
                //invoke SetPathwayState for each pathway, set the state to DEFAULT
                highlightPathway.SetDefault();
                highlightPathways.Remove(highlightPathway);
            }
            //clear the list of pathways/highlights that were created from search.
            searchResultPathwaySOList.Clear();
            searchResultHighlightPathwayList.Clear();
        }
        else
        {
            //create new list here if it's null (first time running search)
            searchResultPathwaySOList = new List<PathwaySO>();
            searchResultHighlightPathwayList = new List<HighlightPathway>();
        }
    }

    /// <summary>
    /// find the highlightApthway instance of target pathway and Set its state.
    /// If accented , set all other highlighted pathways to single highlighted
    /// </summary>
    /// <param name="targetPathwaySO">Pathway that is being set</param>
    /// <param name="state"> highlight state of the target pathway</param>
    public void SetPathwayState(PathwaySO targetPathwaySO, HighlightPathway.HighlightState state)
    {
        HighlightPathway highlightPathway = GetHighlightByPathwaySO(targetPathwaySO);

        if (highlightPathway != null)
        {

            switch (state)
            {
                case HighlightPathway.HighlightState.Default:
                    highlightPathway.SetDefault();

                    break;

                case HighlightPathway.HighlightState.Highlighted:
                    highlightPathway.SetHighlighted();
                    break;

                case HighlightPathway.HighlightState.Accented:
                    highlightPathway.SetAccented();
                    foreach (KeyValuePair<PathwaySO, HighlightPathway> entry in highlightByPathwaySO)
                    {
                        if (entry.Value.state == HighlightPathway.HighlightState.Accented && entry.Value.pathwayToHighlight.name != targetPathwaySO.name)
                        {
                            entry.Value.SetHighlighted(); //downgrade all other accented pathways
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogError("<!> StatusCtrl.SetPathwayState(), null highlightPathway");
        }

    }

    /// <summary>
    /// Returns the max highlight state of an element (node/edge) based on the pathway its connected to. (Accent > Highlighted > Default)
    /// <param name="highlightHandler"> pathway component</param>
    /// <returns> max highlight state </returns>
    public HighlightPathway.HighlightState ElementCheckState(HighlightHandler highlightHandler)
    {

        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        if (highlightHandler == null)
        {
            Debug.LogError("StatusController.ElementCheckState : pathway null");
        }

        if (highlightByPathwaySO == null)
        {
            Debug.LogError("StatusController.ElementCheckState : highlightByPathwaySO dictionary empty");
        }
        elementToPathways.TryGetValue(highlightHandler, out List<HighlightPathway> currentList);

        if (currentList != null)
        {
            foreach (HighlightPathway hlpw in currentList)
            {
                HighlightPathway.HighlightState newState = hlpw.state;
                if (newState > tempState)
                {
                    tempState = newState;
                }
            }
        }
        else
        {
            //Debug.Log("StatusController.ElementCheckState : no pathwaylist are to be found on the elementToPathways Dictionary (NULL access)");
        }
        return tempState;
    }

    /// <summary>
    /// Gets the current highlight state of the pathwaySO
    /// </summary>
    /// <param name="pathway"></param>
    /// <returns> Highlight state </returns>
    public HighlightPathway.HighlightState PathwayCheckState(PathwaySO pathway)
    {
        HighlightPathway.HighlightState tempState = HighlightPathway.HighlightState.Default;
        highlightByPathwaySO.TryGetValue(pathway, out HighlightPathway highlightpathway);

        if (highlightpathway != null)
        {
            tempState = highlightpathway.state;
        }
        else
        {
            Debug.LogError(" StatusController.PathwayCheckState : no highlight pathway linked to this PathwaySO");
        }

        return tempState;
    }

    /// <summary>
    /// Gets the elementToPathway Dictionary Enumarator for external access 
    /// </summary>
    /// <returns></returns>
    public IDictionaryEnumerator GetElementToPathwaysEnumerator()
    {
        return elementToPathways.GetEnumerator();
    }

    /// <summary>
    /// Gets elementToPathways Dictionary count.
    /// </summary>
    /// <returns> Member count</returns>
    public int GetCountElementToPathways()
    {
        return highlightByPathwaySO.Count;
    }

    /// <summary>
    /// Gets highlightPathway instances of all the pathways the component (node/edge)
    /// </summary>
    /// <param name="highlightHandler"></param>
    /// <returns>list of highlightPathway instances</returns>
    public List<HighlightPathway> GetElementToPathways(HighlightHandler highlightHandler)
    {

        elementToPathways.TryGetValue(highlightHandler, out List<HighlightPathway> listOfHPW);

        return listOfHPW;
    }

    /// <summary>
    /// gets the instacne corresponding to the given pathwaySO
    /// </summary>
    /// <param name="pathwaySO"></param>
    /// <returns>highlightPathway instance</returns>
    public HighlightPathway GetHighlightByPathwaySO(PathwaySO pathwaySO)
    {
        highlightByPathwaySO.TryGetValue(pathwaySO, out HighlightPathway value);
        return value;
    }

    /// <summary>
    /// puts all nodes and edges of each active pathwaySO in their corresponding list on the SO, in ortder to keep a refrence to the components
    /// </summary>
    public void FillItemReferenceList()
    {

        foreach (PathwaySO pw in activePathways)
        {
            IDictionaryEnumerator networkEnumerator = pw.GetLocalNetworkEnumerator();
            if (pw.LocalNetwork != null)
            {
                while (networkEnumerator.MoveNext())
                {
                    AllNodeSOs.Add((NodeSO)networkEnumerator.Key);
                    AllEdgeSOs.AddRange((HashSet<EdgeSO>)networkEnumerator.Value);
                }
            }

        }
    }
}
