using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchController : MonoBehaviour
{

    public TMP_InputField FromInput;
    public TMP_InputField ToInput;
    NodeSO fromNode;
    NodeSO toNode;

    // find the scriptable object for the node or edge searched, currently the input needs to match exactly
    public void OnClickSearch()
    {
        if (FromInput.text == "" || ToInput.text == "")
        {
            Debug.LogError("Search input cannot be empty! ");
            NetworkSearch.LogPathways(); 
            return; 
        }

        NodeSO fromNode;
        NodeSO toNode;

        if (TryFindNode(FromInput.text, out fromNode) && 
            TryFindNode(ToInput.text, out toNode))
        {
            NetworkSearch.Instance.BFSTest(
                StatusController.Instance.globalPathway, fromNode, toNode
            );

            Dictionary<int, List<ScriptableObject>> result = NetworkSearch.Instance.SearchForPath(
                StatusController.Instance.globalPathway, fromNode, toNode
            );

            ResultBtnFactory.Instance.ResetButtons(); 
            ResultBtnFactory.Instance.MakeButtons(result);

        }

    }

    /// <summary>
    /// Find a nodeSO in the gameworld with exact match to nodeName
    /// </summary>
    /// <param name="nodeName"> Exact scientific name of the node </param>
    /// <param name="node">  </param>
    /// TODO: search by qid instead!
    bool TryFindNode(string nodeName, out NodeSO node)
    {
        GameObject obj = GameObject.Find(nodeName);                                        // find the object
        node = StatusController.Instance.globalPathway.getNodeByName(nodeName); 
        if (node != null)
        {
            return true; 
        }
        Debug.LogError("SearchController: " + nodeName + " is not a valid node name");
        return false;
    }


    // Deprecated: find the scriptable object for the node or edge searched, currently the input needs to match exactly
    //public void PrintSearch()
    //{

    //    string input = SearchInput.GetComponent<TMP_InputField>().text;                 // string user input 
    //    if (input == "") { return; }

    //    GameObject obj = GameObject.Find(input);                                        // find the object
    //    Debug.Log("Search found object: " + obj.name);

    //    if (obj != null)
    //    {
    //        Debug.Log("Searching for node/edge data");
    //        Transform objTrans = obj.transform;
    //        Transform nodeTrans = objTrans.Find("NodeTemplate");                        // see if its a node

    //        if (nodeTrans != null)
    //        {                                                      // if a node
    //            NodeSO node = obj.GetComponentInChildren<NodeDataDisplay>().nodeData;   // get the scriptable object
    //            if (node != null)
    //            {
    //                Debug.Log("Search NODE : " + node.Label);
    //                // anything else you want to do with the node scriptable object
    //            }
    //        }
    //        else
    //        {                                                                      // if an edge

    //            EdgeSO edge = obj.GetComponentInChildren<EdgeDataDisplay>().edgeData;
    //            if (edge != null)
    //            {
    //                Debug.Log("Search EDGE : " + edge.Label);
    //                // anything else you want to do with the node scriptable object
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Search, object is NULL in PrintSearch");
    //    }
    //}
}
