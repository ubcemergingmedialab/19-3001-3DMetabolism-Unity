using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// General class for node-edge connections, extended by PathwaySO. 
/// </summary>
[CreateAssetMenu(fileName = "New Connections", menuName = "Connections")]
[System.Serializable]
public class ConnectionsSO : GenericSO
{
    /// <summary>
    /// Stores edges of each node as a Dictionary
    /// </summary>
    public Dictionary<NodeSO, HashSet<EdgeSO>> LocalNetwork; 

    public ConnectionsSO(){
        LocalNetwork = new Dictionary<NodeSO, HashSet<EdgeSO>>() ;
    }

    /// <summary>
    /// Add node to the lcoal network dictionary
    /// </summary>
    /// <param name="node"> Node to be added </param>
    public void AddNode(NodeSO node) {
        if (!(LocalNetwork.ContainsKey(node))){
            LocalNetwork.Add(node, new HashSet<EdgeSO>());
        }
    }

    /// <summary>
    /// Add an edge to a node inside the Local network dictionary, 
    /// skip if already added
    /// </summary>
    /// <param name="parentNode"> parent node to the edge </param>
    /// <param name="edge"> edge connected to parent node </param>
    public void AddEdge(NodeSO parentNode, EdgeSO edge){
        LocalNetwork[parentNode].Add(edge);
    }

    /// <summary>
    /// Return all edges connected to a node in List<T> Type
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<EdgeSO> GetConnectedEdges(NodeSO node)
    {
        List<EdgeSO> list = new List<EdgeSO>();
        if (LocalNetwork.ContainsKey(node))
        {
            
            foreach(EdgeSO edge in LocalNetwork[node])
            {
                list.Add(edge); 
            }
        }
        return list;
    }

    /// <summary>
    /// Search for a collection of node labels based on keyword
    /// </summary>
    /// <param name="keyword"> Keyword pattern to match </param>
    /// <returns> List of node labels </returns>
    public List<string> GetMatchingNodes(string keyword)
    {
        IEnumerable<NodeSO> query = LocalNetwork.Keys.Where<NodeSO>(node => node.Label == "something"); // TODO: change to regex

        List<string> matchedNodes = new List<string>();
        foreach (NodeSO node in query)
        {
            matchedNodes.Add(node.Label);
        }
        return matchedNodes;
    }
    
    /// <summary>
    /// Find node in network that has the same label as keyword
    /// </summary>
    /// <returns>Instance of matching keyword or null</returns>
    public NodeSO getNodeByName(string keyword)
    {
        List<string> matchedNodes = new List<string>();
        foreach (NodeSO node in LocalNetwork.Keys)
        {
            if (node.Label == keyword)
            {
                return node; 
            }
        }
        return null; 
    }

    /// <returns> LocalNetwork as a Dictionary Emunerator </returns>
    public IDictionaryEnumerator GetLocalNetworkEnumerator()
    {
        if (LocalNetwork != null)
        {
            return LocalNetwork.GetEnumerator();
        }
        else
        {
            Debug.LogError("<!> local network is null, pathwaySO.getLocalNetworkEnum");
            return null;
        }

    }
}
