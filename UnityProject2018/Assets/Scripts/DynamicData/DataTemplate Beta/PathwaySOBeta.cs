using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pathway Beta", menuName = "Pathway Beta")]
public class PathwaySOBeta : ScriptableObject
{
    public string QID;
    public string Label;
    public string Description;
    // Note: How to manage edges if there is the connections are dealt with in nodes? we need edges for highlighting 
    public Dictionary<NodeSOBeta, List<EdgeSOBeta>> LocalNetwork;

    public void init(string name){

        this.name = name;
        this.Label = name;
        
        LocalNetwork = new Dictionary<NodeSOBeta, List<EdgeSOBeta>>();
    }

    // if the node ahsnt been added to the pathway, add it to the lcoal network dictionary
    public void AddNodeToPathway(NodeSOBeta node) {
        if (!(LocalNetwork.ContainsKey(node))){
            LocalNetwork.Add(node, new List<EdgeSOBeta>());
        } else {
            Debug.Log("<pathwaySO> node is already in " + this.Label + " - pathway");
        }
    }

    // add an edge to a node inside the Local network dictionary
    public void AddEdgeToPathway(NodeSOBeta parentNode, EdgeSOBeta edge){
        LocalNetwork[parentNode].Add(edge);
    }
    
}
