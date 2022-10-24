using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ScriptableObject for pathways. Child class of ConnectionsSO
 * - Potential Issue: after LocalNetwork is populated, FillLists() must be called for nodes and edges to be non-empty
 */
[CreateAssetMenu(fileName = "New Pathway", menuName = "Pathway")]
public class PathwaySO : ConnectionsSO
{
    public string QID;
    public List<NodeSO> nodes;
    public List<EdgeSO> edges;
    public string Label;
    public string Description;

    public void init(string name, string qid, string desc){

        this.name = name;
        this.Label = name;
        this.QID = qid;
        this.Description = desc;
        nodes = new List<NodeSO>();
        edges = new List<EdgeSO>();
        // MakePathway();
    }

    // a way to create pathways thorugh the local files instead of queries. 
    // goes through the edges in a pathway, and adds the nodes and edges to its dictionary
    public void MakePathway(){
        foreach (EdgeSO edge in edges){
            foreach(NodeSO node in edge.reactants){
                AddNode(node);
                AddEdge(node,edge);
            }
            foreach(NodeSO node in edge.products){
                AddNode(node);
                AddEdge(node,edge);
            }
        }
    }

    // Goes through connections in local network, and adds nodes and edges to nodes and edges
    public void FillLists(){
        foreach(KeyValuePair<NodeSO, HashSet<EdgeSO>> pair in LocalNetwork){
            nodes.Add(pair.Key);
            edges.AddRange(pair.Value);
        }
    }
}
