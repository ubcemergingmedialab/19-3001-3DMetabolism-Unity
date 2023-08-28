using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;


public class QueryService : MonoBehaviour
{

    static Dictionary<string, EdgeSO> EdgeSOs = new Dictionary<string, EdgeSO>();
    static Dictionary<string, NodeSO> NodeSOs = new Dictionary<string, NodeSO>();
    public static Dictionary<string, PathwaySO> PathwaySOs = new Dictionary<string, PathwaySO>();
    static ConnectionsSO globalPathway;

    public TextAsset queryxml;

    static string ResourceFolderPath = "Assets/Resources/Data/";
    static string JsonFileDestination = "Assets/Resources/Data/query.xml";
    // static string JsonFileDestination = "query.xml";


    /*
        1-  serialize the json
        2- create SOs
        3- add pathways to pathway list
        4- connect the SOs to prefabs
        */
    void Awake() {
        SerializeAndCreate();
        PathwaysToActive();
        AttachScriptableObjectToPrefab();
    }


    void OnApplicationQuit() {
        ClearQueryData();
    }



    // make the query URI ready and pass it to GetRequest(uri)
    public void RunQuery(string WQSLink, string raw) {
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
                WriteString(webRequest.downloadHandler.text);
            }
        }
    }

    // write the json file as a xml to be parsed
    static void WriteString(string str)
    {
        FileStream filestream = new FileStream(JsonFileDestination, FileMode.Create);
        StreamWriter writer = new StreamWriter(filestream);
        writer.Write(str);
        writer.Close();
    }

    // initiate the scriptable object creating after serialization 
    void SerializeAndCreate()
    {
        // FileStream fileStream = new FileStream(JsonFileDestination, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        // var streamReader = new StreamReader(fileStream);
        // string xmlToString = streamReader.ReadToEnd();

        string xmlToString = queryxml.text;

        WikibaseResult result = JsonUtility.FromJson<WikibaseResult>(xmlToString);

        globalPathway = ScriptableObject.CreateInstance<ConnectionsSO>();
        foreach (WikibaseBinding item in result.results.bindings) {
            NodeSOInit(item);
        }
    }



    // fill active pathways in statuscontroller after querying 
    void PathwaysToActive() {
        FillPathwayList(); //TODO: list usage seems to be unused, added for testing MockSearch()
        GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Clear();
        foreach (KeyValuePair<string, PathwaySO> pair in PathwaySOs)
        {
            GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Add(pair.Value);

        }
        GameObject.Find("StatusController").GetComponent<StatusController>().globalPathway = globalPathway;
    }


    // call prefabassigment to attach scriptable object to prefabs
    void AttachScriptableObjectToPrefab() {
        GameObject.Find("PrefabService").GetComponent<PrefabService>().PrefabAssignment();
    }


    public void FillPathwayList() {
        foreach (KeyValuePair<string, PathwaySO> item in PathwaySOs)
        {
            item.Value.FillLists();
        }
    }

    // create an EdgeSo instance from the text given in the query Json , unless the edge already exists
    public void EdgeSOInit(WikibaseBinding item) {
        bool direction;
        if (!(EdgeSOs.ContainsKey(item.edgeLabel.value))) {
            EdgeSO edge = ScriptableObject.CreateInstance<EdgeSO>();
            direction = false;

            if (item.isBidirectional.value == "true") {
                direction = true;
            }

            edge.init(item.edgeLabel.value, item.edgeQID.value, item.edgeDesc.value, item.enzymeLabel.value, item.edgeEnzymeTypeLabel.value, item.edgeEnergyReqLabel.value, item.edgePubchem.value, item.edgeRegulation.value, direction);

            string newPath = ResourceFolderPath + "EdgeSO/" + item.enzymeLabel.value + ".asset";
            //AssetDatabase.CreateAsset(edge,newPath);
            EdgeSOs.Add(item.edgeLabel.value, edge);

        }
    }




// TODO: update init to fit new pathway logic
// create NodeSO from the Json Query if the node doesnt exists. Add the node to reactant ror products of the edge its invloved in.
// in case the edge doesnt exists, call EdgeSOInit to create the edge.
// add cofactors to the edge
public void NodeSOInit(WikibaseBinding item){

    NodeSO currentNode;
    EdgeSO currentEdge;
    PathwaySO currentPathway;
    Cofactor currentCofactor;

    if (!(NodeSOs.TryGetValue(item.metaboliteLabel.value, out currentNode))) 
        {
        string newPath = ResourceFolderPath + "NodeSO/" + item.metaboliteLabel.value + ".asset";
        currentNode = ScriptableObject.CreateInstance<NodeSO>();
        currentNode.init(item.metaboliteLabel.value, item.metaboliteQID.value, item.metaboliteDesc.value, item.metaboliteMoleFormula.value, item.metaboliteIUPAC.value, item.metaboliteStrucDesc.value, item.metaboliteCharge.value, item.metabolitePubchem.value, item.metaboliteCID.value);
        NodeSOs.Add(item.metaboliteLabel.value, currentNode);
        //AssetDatabase.CreateAsset(currentNode,newPath);
        }


    // Creating the edge and adding the node as product/reactant
    if (!(EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge))){
    EdgeSOInit(item);
    EdgeSOs.TryGetValue(item.edgeLabel.value, out currentEdge);
    }
    
    if(item.isProduct.value == "true"){  
        currentEdge.AddProduct(currentNode);
    }else if(item.isReactant.value == "true"){
        currentEdge.AddReactant(currentNode);   
    }
    // make cofactor 
    if (item.isCofactorProduct.value == "true")
    {
        currentCofactor = new Cofactor(item.cofactorLabel.value, false);
    }
    else if (item.isCofactorReactant.value == "true")
    {
        currentCofactor = new Cofactor(item.cofactorLabel.value, true);
    }
    else
    {
        currentCofactor = null;
    }
    // add cofactor
    if (currentCofactor != null)
    {
        currentEdge.AddCofactor(currentCofactor);
    }
        
    if (item.pathwayLabel.value == "Fatty Acid Synthesis")
        {
            if (item.metaboliteLabel.value == "malate")
            {

            }
        }
    
    if (!(PathwaySOs.TryGetValue(item.pathwayLabel.value, out currentPathway))){
        PathwaySOInit(item);
        PathwaySOs.TryGetValue(item.pathwayLabel.value, out currentPathway);
    }

    // adds the node and edge to the pathway localnetwork dictionary 
    currentPathway.AddNode(currentNode);
    currentPathway.AddEdge(currentNode, currentEdge);

    // add nodes and edges to global pathway
    globalPathway.AddNode(currentNode);
    globalPathway.AddEdge(currentNode, currentEdge);
    
}

public void PathwaySOInit(WikibaseBinding item){
    if (!(PathwaySOs.ContainsKey(item.pathwayLabel.value))){
        PathwaySO pathway = ScriptableObject.CreateInstance<PathwaySO>();
        pathway.init(item.pathwayLabel.value,item.pathwayQID.value,item.pathwayDesc.value);
        string newPath = ResourceFolderPath + "PathwaySO/"+ item.pathwayLabel.value + ".asset";
        //AssetDatabase.CreateAsset(pathway,newPath);
        PathwaySOs.Add(item.pathwayLabel.value,pathway);
    }

}

// empties the dictionaries holding the scriptable objects
public async void ClearQueryData(){
    NodeSOs.Clear();
    EdgeSOs.Clear();
    PathwaySOs.Clear();

    DirectoryInfo pathwayDir = new DirectoryInfo("Assets/Resources/Data/PathwaySO/");
    DirectoryInfo edgeDir = new DirectoryInfo("Assets/Resources/Data/EdgeSO/");
    DirectoryInfo nodeDir = new DirectoryInfo("Assets/Resources/Data/NodeSO/");
    List<DirectoryInfo> dirs = new List<DirectoryInfo>(){pathwayDir,edgeDir, nodeDir};
    int i = 0;
    foreach(DirectoryInfo dir in dirs){
        foreach(FileInfo fi in dir.GetFiles())
        {
            fi.Delete();
            i++;
            
        }  
    }
    
    Debug.Log("deleted: " + i + " files");
}

public async void DeleteQueryXml(){

    File.Delete("Assets/Resources/Data/query.xml");
    File.Delete("Assets/Resources/query.xml");
}

}