using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrefabService
{
    public void PrefabAssignment(){
        List<PathwaySO> pathways = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;
        foreach (PathwaySO pathway in pathways){
            foreach (KeyValuePair<NodeSO, List<EdgeSO>> pair in pathway.LocalNetwork){
                FindNodeSOGameObject(pair.Key);
                foreach(EdgeSO edge in pair.Value){
                    FindEdgeSOGameObject(edge);
                }
            }
        }
        
    }

    public void FindNodeSOGameObject(NodeSO node) {
        string nodeName = node.Label; 
        GameObject obj = GameObject.Find(nodeName);
        if (obj != null) {
            if(obj.GetComponentInChildren<NodeDataDisplay>().nodeData == null) {
                obj.GetComponentInChildren<NodeDataDisplay>().nodeData = node;
            }
        } else {
            Debug.LogError("Node scriptable object not connected to prefab :" + nodeName);
        }
    }

    public void FindEdgeSOGameObject(EdgeSO edge) {
        string edgeName = edge.Label; 
        GameObject[] objs = GameObject.FindGameObjectsWithTag(edgeName);
        foreach (GameObject obj in objs) 
        {    
            if (obj != null) {
                if(obj.GetComponentInChildren<EdgeDataDisplay>().edgeData == null) {
                    obj.GetComponentInChildren<EdgeDataDisplay>().edgeData = edge;
                }
            } else {
                Debug.LogError("Edge scriptable object not connected to prefab :" + edgeName);
            }
        }
    }
}
