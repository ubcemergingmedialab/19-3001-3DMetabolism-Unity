using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class TestQuery : MonoBehaviour 
{
string path =  "Assets/Scripts/QueryServices/TestQueryJson.json";

Dictionary<string,EdgeSO> EdgeSOs;
Dictionary<string,NodeSO> NodeSOs; 

 void Start() {

    EdgeSOs = new Dictionary<string, EdgeSO>();
    NodeSOs = new Dictionary<string, NodeSO>();

    StreamReader reader = new StreamReader(path);
    string jsonString = reader.ReadToEnd();
    reader.Close();
    Debug.Log("<json>" + jsonString);

    WikibaseResults results = JsonUtility.FromJson<WikibaseResults>(jsonString);

    Debug.Log("<json>" + results.resultList.Count);

    foreach( ResultItem item in results.resultList){
        Debug.Log("<json>" + item.metaboliteLabel);
        NodeSOInit(item);

    }
    
}

public void EdgeSOInit(ResultItem item){
    if (!(EdgeSOs.ContainsKey(item.edgeLabel))){
        EdgeSO edge = ScriptableObject.CreateInstance<EdgeSO>();
        edge.init(item.edgeLabel);
        EdgeSOs.Add(item.edgeLabel,edge);
    }
}

public void NodeSOInit(ResultItem item){
    if (!(NodeSOs.ContainsKey(item.metaboliteLabel))){
        
        EdgeSO currentEdge;
        NodeSO node = ScriptableObject.CreateInstance<NodeSO>();
        node.init(item.metaboliteLabel);
        NodeSOs.Add(item.metaboliteLabel,node);

        if (EdgeSOs.TryGetValue(item.edgeLabel, out currentEdge)){
            if(item.isProduct == "true"){
                currentEdge.AddProduct(node);
            }else if(item.isReactant == "true"){
                currentEdge.AddReactant(node);
            }

        }else{
            EdgeSOInit(item);
            EdgeSOs.TryGetValue(item.edgeLabel, out currentEdge);
            if(item.isProduct == "true"){
                currentEdge.AddProduct(node);
            }else if(item.isReactant == "true"){
                currentEdge.AddReactant(node);
            }
        }
    }
}


}