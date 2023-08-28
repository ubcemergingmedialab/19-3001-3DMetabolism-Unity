using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class PrefabService : MonoBehaviour
{
    GameObject edgeLabelsObject;

    public void PrefabAssignment(){
        List<PathwaySO> pathways = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;
        edgeLabelsObject = GameObject.Find("EdgeLabels");

        // for testing purposes, checks if pathways and pathway local networks exist and shos count
        Debug.Log("<PrefabService Test> pathway count: " + pathways.Count);

        if (pathways[0].LocalNetwork == null) {
            Debug.Log("pw.network is NULL !!");
        }else{
            foreach (PathwaySO pathway in pathways){
                IDictionaryEnumerator LNEnumerator = pathway.GetLocalNetworkEnumerator();


                while(LNEnumerator.MoveNext()){
                    FindNodeSOGameObject((NodeSO) LNEnumerator.Key);
                    foreach(EdgeSO edge in ((HashSet<EdgeSO>) LNEnumerator.Value)){
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
        string nodeName = node.name; 
        GameObject obj =  GameObject.Find(nodeName);
        if (obj != null) {
            if(obj.GetComponentInChildren<NodeDataDisplay>().nodeData == null) {
                obj.GetComponentInChildren<NodeDataDisplay>().nodeData = node;
                // Debug.Log("Attached node " + nodeName);
            }
            AttachOutlineScript(obj);
        } else {
            Debug.LogWarning("Node scriptable object not connected to prefab: " + nodeName);
        }
    }



    public void FindEdgeSOGameObject(EdgeSO edge) {
        string edgeName = edge.name;
        GameObject obj = GameObject.Find(edgeName);

        int sharedEdgeEnzymes = 0;

        if (obj == null)
        {
            int maxIterations = 10;

            for (int i = 1; i < maxIterations; i++)
            {
                if (GameObject.Find(edgeName + ".00" + i.ToString()))
                    sharedEdgeEnzymes = i;
            }
        }

        if (sharedEdgeEnzymes > 0)
        {
            for (int i = 1; i < sharedEdgeEnzymes + 1; i++)
            {
                obj = GameObject.Find(edgeName + ".00" + i.ToString());
                AssignEdgeData(obj, edge);
            }
        }
        else
        {
            AssignEdgeData(obj, edge);
        }
    }

    void AssignEdgeData(GameObject obj, EdgeSO edge)
    {
        if (obj != null)
        {
            if (obj.GetComponentInChildren<EdgeDataDisplay>().edgeData == null)
            {

                // Assign Edge data to Edge gameobject
                obj.GetComponentInChildren<EdgeDataDisplay>().edgeData = edge;

                // Instantiate edge label on top of the edge
                obj.GetComponentInChildren<EdgeDataDisplay>().InstantiateEdgeLabel(edgeLabelsObject);

                // Instantiate Cofactors
                //obj.GetComponentInChildren<EdgeDataDisplay>().InstantiateCofactors();

            }
            AttachOutlineScript(obj.transform.gameObject);
        }
        else
        {
            Debug.LogError("Edge scriptable object not connected to prefab :" + edge.name);
        }
    }

    /// <summary>
    /// Attaches the Outline script that is from unity asset store to the gameobject
    /// </summary>
    /// <param name="obj"></param>
    public void AttachOutlineScript(GameObject obj)
    {
        if (obj.GetComponent<Outline>())
            return;
        // assign outline shader here
        Outline objOutline = obj.AddComponent<Outline>();

        if (objOutline != null)
        {
            objOutline.OutlineMode = Outline.Mode.OutlineAll;
            objOutline.OutlineWidth = 3;
            objOutline.enabled = false;
        }
        
    }

    /// <summary>
    /// disables all Outline components in the game
    /// </summary>
    public void DisableAllOutline()
    {
        Outline[] listOfOutline = Object.FindObjectsOfType<Outline>();
        foreach (Outline outline in listOfOutline)
        {
            outline.enabled = false;
        }
    }
}
