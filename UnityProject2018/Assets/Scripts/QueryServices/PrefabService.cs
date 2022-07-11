using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrefabService : MonoBehaviour
{
    public void PrefabAssignment(){
        List<PathwaySO> pathways = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;


        // for testing purposes, checks if pathways and pathway local networks exist and shos count
        Debug.Log("<PrefabService Test> pathway count: " + pathways.Count);

        if (pathways[0].LocalNetwork == null) {
            Debug.Log("pw.network is NULL !!");
        }else{
            foreach (PathwaySO pathway in pathways){
                IDictionaryEnumerator LNEnumerator = pathway.GetLocalNetworkEnumerator();


                while(LNEnumerator.MoveNext()){
                    FindNodeSOGameObject((NodeSO) LNEnumerator.Key);
                    foreach(EdgeSO edge in ((List<EdgeSO>) LNEnumerator.Value)){
                        FindEdgeSOGameObject(edge);
                    }
                }
                // foreach (KeyValuePair<NodeSO, List<EdgeSO>> pair in pathway.LocalNetwork){
                //     FindNodeSOGameObject(pair.Key);
                //     foreach(EdgeSO edge in pair.Value){
                //         FindEdgeSOGameObject(edge);
                //     }
                // }
            }
        }
    }
 
    public void FindNodeSOGameObject(NodeSO node) {
        string nodeName = node.Label; 
        GameObject obj =  GameObject.Find(nodeName);
        if (obj != null) {
            if(obj.GetComponentInChildren<NodeDataDisplay>().nodeData == null) {
                obj.GetComponentInChildren<NodeDataDisplay>().nodeData = node;
                Debug.Log("Attached node " + nodeName);
            }
        } else {
            Debug.LogError("Node scriptable object not connected to prefab: " + nodeName);
        }
    }

    public void FindEdgeSOGameObject(EdgeSO edge) {
        string edgeName = edge.name; 
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
