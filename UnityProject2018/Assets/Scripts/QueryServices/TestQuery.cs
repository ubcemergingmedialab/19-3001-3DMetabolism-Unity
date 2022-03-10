using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;



public class TestQuery : MonoBehaviour 
{
string path =  "Assets/Scripts/QueryServices/TestQueryJson.json";

Dictionary<string,EdgeSO> EdgeSOs;
Dictionary<string,NodeSO> NodeSOs; 

string ResourceFolderPath = "Assets/Resources/Data/TestQuerySO/";

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

// create an EdgeSo instance from the text given in the query Json , unless the edge already exists
public void EdgeSOInit(ResultItem item){
    if (!(EdgeSOs.ContainsKey(item.edgeLabel))){
        EdgeSO edge = ScriptableObject.CreateInstance<EdgeSO>();
        edge.init(item.edgeLabel,item.edgeQID);
        EdgeSOs.Add(item.edgeLabel,edge);
        string newPath = ResourceFolderPath + item.enzymeLabel + ".asset";
        AssetDatabase.CreateAsset(edge,newPath);
    }
}

// create NodeSO from the Json Query if the node doesnt exists. Add the node to reactant ror products of the edge its invloved in.
// in case the edge doesnt exists, call EdgeSOInit to create the edge. 
public void NodeSOInit(ResultItem item){
    if (!(NodeSOs.ContainsKey(item.metaboliteLabel))){
        
        string newPath = ResourceFolderPath + item.metaboliteLabel + ".asset";
        EdgeSO currentEdge;
        NodeSO node = ScriptableObject.CreateInstance<NodeSO>();
        node.init(item.metaboliteLabel,item.metaboliteQID);
        NodeSOs.Add(item.metaboliteLabel,node);
        AssetDatabase.CreateAsset(node,newPath);

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