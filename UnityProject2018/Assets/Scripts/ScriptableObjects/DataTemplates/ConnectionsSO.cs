using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Connections", menuName = "Connections")]

/***
* General class for node-edge connections, used for globalNetwork
*/
public class ConnectionsSO : ScriptableObject
{

    // Note: How to manage edges if there is the connections are dealt with in nodes? we need edges for highlighting 
    public Dictionary<NodeSO, List<EdgeSO>> LocalNetwork;

    public ConnectionsSO(){
        LocalNetwork = new Dictionary<NodeSO, List<EdgeSO>>() ;
    }

    // if the node ahsnt been added to the pathway, add it to the lcoal network dictionary
    public void AddNode(NodeSO node) {
        if (!(LocalNetwork.ContainsKey(node))){
            LocalNetwork.Add(node, new List<EdgeSO>());
            // Debug.Log(node.Label + " added to " + this.Label);
        } else {
            // Debug.Log("<pathwaySO> node " + node.Label + " is already in " + this.Label + " - pathway");
        }
    }

    // add an edge to a node inside the Local network dictionary
    public void AddEdge(NodeSO parentNode, EdgeSO edge){
        LocalNetwork[parentNode].Add(edge);
    }
}
