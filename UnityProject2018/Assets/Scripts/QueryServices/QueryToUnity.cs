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

public static string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";
public static string queryRaw = "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/>" +
        "select distinct ?prefixedEdge" +
        "(strafter(?prefixedEdge,\":\") as ?edgeQID)" +
        "?prefixedMetabolite" +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID)" +
        "?edgeLabel ?metaboliteLabel ?isReactant ?isProduct" +
        "?prefixedEnzyme" +
        "(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) ?enzymeLabel where {" +
        "foaf:Q88 wdt:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge wdt:P14 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?statement pq:P31|pq:P32 ?edge." +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND(replace(str(?enzyme), str(foaf:), \"foaf:\") as ?prefixedEnzyme)" +
        "BIND ( EXISTS { ?statement pq:P31 ?edge } as ?isReactant )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge } as ?isProduct )" +
        "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }" +
        "}";

string ResourceFolderPath = "Assets/Resources/Data/TestQuerySO/";

 void Start() {

    // EdgeSOs = new Dictionary<string, EdgeSO>();
    // NodeSOs = new Dictionary<string, NodeSO>();
    
}

// create an EdgeSo instance from the text given in the query Json , unless the edge already exists
public void EdgeSOInit(WikibaseBinding item){
    if (!(EdgeSOs.ContainsKey(item.edgeLabel.value))){
        EdgeSO edge = ScriptableObject.CreateInstance<EdgeSO>();
        edge.init(item.edgeLabel.value,item.edgeQID.value);
        EdgeSOs.Add(item.edgeLabel.value,edge);
        string newPath = ResourceFolderPath + item.enzymeLabel.value + ".asset";
        AssetDatabase.CreateAsset(edge,newPath);
    }
}

// create NodeSO from the Json Query if the node doesnt exists. Add the node to reactant ror products of the edge its invloved in.
// in case the edge doesnt exists, call EdgeSOInit to create the edge. 
public void NodeSOInit(WikibaseBinding item){
    if (!(NodeSOs.ContainsKey(item.metaboliteLabel.value))){
        
        string newPath = ResourceFolderPath + item.metaboliteLabel.value + ".asset";
        EdgeSO currentEdge;
        NodeSO node = ScriptableObject.CreateInstance<NodeSO>();
        node.init(item.metaboliteLabel.value,item.metaboliteQID.value);
        NodeSOs.Add(item.metaboliteLabel.value,node);
        AssetDatabase.CreateAsset(node,newPath);

        if (EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge)){
            if(item.isProduct.value == "true"){
                currentEdge.AddProduct(node);
            }else if(item.isReactant.value == "true"){
                currentEdge.AddReactant(node);
            }

        }else{
            EdgeSOInit(item);
            EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge);
            if(item.isProduct.value == "true"){
                currentEdge.AddProduct(node);
            }else if(item.isReactant.value == "true"){
                currentEdge.AddReactant(node);
            }
        }
    }
}

// make the query request ready and pass it to GetRequest(uri)
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
                Debug.Log("<json>" + item.metaboliteLabel.value);
                NodeSOInit(item);

            }
            }
        }
    }


}