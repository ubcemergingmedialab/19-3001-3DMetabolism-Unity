using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public IDictionaryEnumerator GetLocalNetworkEnumerator(){
        if(LocalNetwork != null){
           return LocalNetwork.GetEnumerator(); 
        }else{
            Debug.LogError("<!> local network is null, pathwaySO.getLocalNetworkEnum");
            return null;
        }
        
    }

    public void FillLists(){
        foreach(KeyValuePair<NodeSO, List<EdgeSO>> pair in LocalNetwork){
            nodes.Add(pair.Key);
            
            edges.AddRange(pair.Value);
        }
    }
}
