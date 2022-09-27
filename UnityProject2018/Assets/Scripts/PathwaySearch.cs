using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathwaySearch : MonoBehaviour
{

    public GameObject SearchInput;

    // find the scriptable object for the node or edge searched, currently the input needs to match exactly
    public void PrintSearch() {

        string input = SearchInput.GetComponent<TMP_InputField>().text;                 // string user input 
        if (input == "") {return;}

        GameObject obj = GameObject.Find(input);                                        // find the object
        Debug.Log("Search found object: " + obj.name);

        if (obj != null){
            Debug.Log("Searching for node/edge data");
            Transform objTrans = obj.transform;
            Transform nodeTrans = objTrans.Find("NodeTemplate");                        // see if its a node

            if(nodeTrans != null){                                                      // if a node
                NodeSO node = obj.GetComponentInChildren<NodeDataDisplay>().nodeData;   // get the scriptable object
                if(node != null){   
                    Debug.Log( "Search NODE : " + node.Label );
                    // anything else you want to do with the node scriptable object
                }
            }else{                                                                      // if an edge
             
                EdgeSO edge = obj.GetComponentInChildren<EdgeDataDisplay>().edgeData;
                if (edge != null){
                    Debug.Log( "Search EDGE : " + edge.Label);
                    // anything else you want to do with the node scriptable object
                }
            }
        }else{
            Debug.LogError("Search, object is NULL in PrintSearch");
        }
    }
}
