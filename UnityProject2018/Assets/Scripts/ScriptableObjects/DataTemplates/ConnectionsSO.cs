using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Connections", menuName = "Connections")]

/***
* General class for node-edge connections, extended by PathwaySO
*  - Stores edges of a node as an adjacency matrix
*/
public class ConnectionsSO : ScriptableObject
{
    public Dictionary<NodeSO, HashSet<EdgeSO>> LocalNetwork; 

    public ConnectionsSO(){
        LocalNetwork = new Dictionary<NodeSO, HashSet<EdgeSO>>() ;
    }

    // Add node to the lcoal network dictionary
    public void AddNode(NodeSO node) {
        if (!(LocalNetwork.ContainsKey(node))){
            LocalNetwork.Add(node, new HashSet<EdgeSO>());
        }
    }

    // add an edge to a node inside the Local network dictionary,
    // skip if already added
    public void AddEdge(NodeSO parentNode, EdgeSO edge){
        LocalNetwork[parentNode].Add(edge);
    }

    // Return all edges connected to a node in List<T> Type
    public List<EdgeSO> GetEdgesAsList(NodeSO node)
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
