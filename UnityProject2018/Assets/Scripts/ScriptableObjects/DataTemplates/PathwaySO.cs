using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for pathways. Inherits all members of ConnectionSO
/// </summary>
/// <remarks>
/// Potential Issue: after LocalNetwork is populated, FillLists() must be called for nodes and edges to be non-empty
/// </remarks>
[CreateAssetMenu(fileName = "New Pathway", menuName = "Pathway")]
public class PathwaySO : ConnectionsSO
{
    public string QID;
    public List<NodeSO> nodes;
    public List<EdgeSO> edges;
    public string Label;
    public string Description;

    /// <summary>
    /// Initialize PathwaySO with name, QID and desription
    /// </summary>
    public void init(string name, string qid, string desc){

        this.name = name;
        this.Label = name;
        this.QID = qid;
        this.Description = desc;
        nodes = new List<NodeSO>();
        edges = new List<EdgeSO>();
        // MakePathway();
    }

    /// <summary> 
    /// Goes through the fields "nodes" and "edges", and adds the nodes and edges to LocalNetwork
    /// </summary>
    /// <remarks>
    /// A way to create pathways thorugh the local files instead of queries.
    /// </remarks>
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

    /// <summary>
    /// Goes through connections in local network, and adds nodes and edges to nodes and edges
    /// </summary>
    public void FillLists(){
        foreach(KeyValuePair<NodeSO, HashSet<EdgeSO>> pair in LocalNetwork){
            nodes.Add(pair.Key);
            edges.AddRange(pair.Value);
        }
    }
}
