using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pathway", menuName = "Pathway")]
public class PathwaySO : ScriptableObject
{
    public string QID;
    public List<NodeSO> nodes;
    public List<EdgeSO> edges;
    public string Label;
    public string Description;
    // Note: How to manage edges if there is the connections are dealt with in nodes? we need edges for highlighting 
    public Dictionary<NodeSO, List<EdgeSO>> LocalNetwork;

    public void init(string name, string qid){

        this.name = name;
        this.Label = name;
        this.QID = qid;
        
        LocalNetwork = new Dictionary<NodeSO, List<EdgeSO>>();
        //MakePathway();
    }

    // if the node ahsnt been added to the pathway, add it to the lcoal network dictionary
    public void AddNodeToPathway(NodeSO node) {
        if (!(LocalNetwork.ContainsKey(node))){
            LocalNetwork.Add(node, new List<EdgeSO>());
        } else {
            Debug.Log("<pathwaySO> node is already in " + this.Label + " - pathway");
        }
    }

    // add an edge to a node inside the Local network dictionary
    public void AddEdgeToPathway(NodeSO parentNode, EdgeSO edge){
        LocalNetwork[parentNode].Add(edge);
    }

    // a way to create pathways thorugh the local files instead of queries.
    // goes through the edges in a pathway, and adds the nodes and edges to its dictionary
    public void MakePathway(){
        foreach (EdgeSO edge in edges){
            foreach(NodeSO node in edge.reactants){
                AddNodeToPathway(node);
                AddEdgeToPathway(node,edge);
            }
            foreach(NodeSO node in edge.products){
                AddNodeToPathway(node);
                AddEdgeToPathway(node,edge);
            }
        }
    }
}
