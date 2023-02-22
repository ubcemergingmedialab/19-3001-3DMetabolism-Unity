using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Display search result as a list of buttons
 */
public class ResultBtnFactory : MonoBehaviour
{
    // singleton instantiation of ButtonFactory
    private static ResultBtnFactory _instance;
    public static ResultBtnFactory Instance
    {
        get { return _instance; }
    }

    public GameObject buttonPrefab;

    // Determines the placement of buttons.
    // X value should remain constant, Y value should differ by the offset
    // at every button generation
    public static float buttonX = 0;
    public static float buttonYOffset = -50;
    public float buttonY;
    public float currentButtonY;

    public Card dataSO;

    //Dictionary<GameObject, PathwaySO> buttons = new Dictionary<GameObject, PathwaySO>();
    List<GameObject> resultBtns = new List<GameObject>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        currentButtonY = buttonY;
        _instance = this;

    }

    // TODO: refactor results
    public void MakeButtons(Dictionary<int, List<ScriptableObject>> paths)
    {
        foreach (KeyValuePair<int, List<ScriptableObject>> path in paths)
        {
            // Debug.Log("adding button for " + pathway);
            resultBtns.Add(GenerateButton(path.Key, path.Value));
            currentButtonY += buttonYOffset;
        }
    }

    // remove existing buttons
    public void ResetButtons()
    {
        foreach (GameObject go in resultBtns)
        {
            Destroy(go);
        }
        currentButtonY = buttonY;
        resultBtns = new List<GameObject>();
    }

    GameObject GenerateButton(int n, List<ScriptableObject> path)
    {
        GameObject generated = InitButtonAndSetPosition();
        SetBtnText(n, path, generated);

        int numPathways = StatusController.Instance.activePathways.Count;

        // TO REFACTOR: Populate with new button logic + actual paths
        //create a new PathwaySO and use that instead of StatusController.Instance.activePathways[n % numPathways];
        PathwaySO pathway = ScriptableObject.CreateInstance<PathwaySO>();
        pathway.init("SearchResult" + n, "SR QID", "Search Result Description");


        //currentPathway.AddNode(currentNode);
        //currentPathway.AddEdge(currentNode, currentEdge);
        for (int i = 0; i < path.Count; i++)
        {
            //add this node to the pathway if it is a node
            if (path[i].GetType() == typeof(NodeSO))
            {
                pathway.AddNode((NodeSO)path[i]);
            }
            else if (path[i].GetType() == typeof(EdgeSO))
            {
                //we are adding the previous node as a parent to this edge
                pathway.AddEdge((NodeSO)path[i - 1], (EdgeSO)path[i]);
            }
        }

        //assign the pathway we just created (with it's resulting nodes and edges)
        generated.GetComponent<PathwayButtonLogic>().pathwaySO = pathway;

        //now query service has a new pathway (not the traditional pre-set pathway).
        //TODO we might not need this since we are updating the status controller anywuays
        //QueryService.PathwaySOs.Add("SearchResult" + n, pathway);

        //Highlight manager needs to knows about this 'pathway'
        StatusController.Instance.AddPathwayToHighlight(pathway);
        //---

        //generated.GetComponent<PathwayButtonLogic>().pathwaySO = StatusController.Instance.activePathways[n % numPathways];
        generated.GetComponent<PathwayButtonLogic>().dataSO = dataSO;
        return generated;
    }

    private static void SetBtnText
        (int n, List<ScriptableObject> path, GameObject generated)
    {
        Text childText = generated.GetComponentInChildren<Text>();
        childText.text = "Path " + n;
        //childText.text += " via " + path[path.Count / 2].name; 
    }

    private GameObject InitButtonAndSetPosition()
    {
        GameObject generated = Instantiate(buttonPrefab, transform);
        RectTransform rect = generated.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(buttonX, currentButtonY, 0);
        return generated;
    }

}
