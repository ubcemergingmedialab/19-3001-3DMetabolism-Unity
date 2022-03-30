using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;


public class QueryToUnity : MonoBehaviour 
{

static Dictionary<string,EdgeSO> EdgeSOs = new Dictionary<string, EdgeSO>();
static Dictionary<string,NodeSO> NodeSOs = new Dictionary<string, NodeSO>();

string ResourceFolderPath = "Assets/Resources/Data/TestQuerySO/";

 void Start() {
    
}

// create an EdgeSo instance from the text given in the query Json , unless the edge already exists
public void EdgeSOInit(WikibaseBinding item){
    if (!(EdgeSOs.ContainsKey(item.edgeLabel.value))){
        EdgeSO edge = ScriptableObject.CreateInstance<EdgeSO>();
        bool direction = false;
          
        if(item.isBidirectional.value == "true"){
            direction = true;
        }
        Debug.Log(item.isBidirectional.value + " direction var = " + direction);
        edge.init(item.edgeLabel.value,item.edgeQID.value,direction);
        string newPath = ResourceFolderPath + item.enzymeLabel.value + ".asset";
        AssetDatabase.CreateAsset(edge,newPath);
        EdgeSOs.Add(item.edgeLabel.value,edge);
        Debug.Log("edge added");
    }
}

// create NodeSO from the Json Query if the node doesnt exists. Add the node to reactant ror products of the edge its invloved in.
// in case the edge doesnt exists, call EdgeSOInit to create the edge. 
public void NodeSOInit(WikibaseBinding item){
    NodeSO currentNode;
    EdgeSO currentEdge;
    if (!(NodeSOs.ContainsKey(item.metaboliteLabel.value))){
        
        string newPath = ResourceFolderPath + item.metaboliteLabel.value + ".asset";
        currentNode = ScriptableObject.CreateInstance<NodeSO>();
        currentNode.init(item.metaboliteLabel.value,item.metaboliteQID.value);
        NodeSOs.Add(item.metaboliteLabel.value,currentNode);
        AssetDatabase.CreateAsset(currentNode,newPath);
    }else{
        NodeSOs.TryGetValue(item.metaboliteLabel.value, out currentNode);
    }
    
    if (!(EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge))){
        EdgeSOInit(item);
        EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge);
    }
    if(item.isProduct.value == "true"){  
        currentEdge.AddProduct(currentNode);
    }else if(item.isReactant.value == "true"){
        currentEdge.AddReactant(currentNode);   
    }
    
}

// empties the dictionaries holding the scriptable objects
public async void ClearQueryData(){
    NodeSOs.Clear();
    EdgeSOs.Clear();

    DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/TestQuerySO/");
    int i = 0;
    foreach(FileInfo fi in dir.GetFiles())
    {
        fi.Delete();
        i++;
        
    }
    Debug.Log("deleted: " + i + " files");
}

// make the query URI ready and pass it to GetRequest(uri)
public void RunQuery(string WQSLink , string raw){
    string queryReady = UnityWebRequest.EscapeURL(raw);
    StartCoroutine(GetRequest(WQSLink + queryReady));
    
}  

// request the query and save and parse the json file
IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("content-type", "application/sparql-results+json");
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                WikibaseResult result = JsonUtility.FromJson<WikibaseResult>(webRequest.downloadHandler.text);

            Debug.Log("<json>" + result.results.bindings.Count);

            foreach( WikibaseBinding item in result.results.bindings){
                //Debug.Log("<json>" + item.metaboliteLabel.value + " with edge of: " + item.enzymeLabel.value);
                NodeSOInit(item);

            }
            
  
            }
        }
    }


}