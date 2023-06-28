using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Xml;
using System;

public class SyncDataHandler : MonoBehaviour
{
    public static string path = "Assets/Resources/Data/query.xml";
    public static string fileDestination = "Assets/Resources/Data/query.xml";
    public List<PathwaySO> pathways;
    public QueriesSO queries;

    // private XmlDocument 
    void Start()
    {
        UpdatePathways();
    }

    void UpdatePathways() 
    {
        foreach(PathwaySO pathway in pathways)
        {
            UpdateNodes(pathway.nodes);
            UpdateEdges(pathway.edges);
        }
    }

    void UpdateEdges(List<EdgeSO> edges) 
    {
        foreach (EdgeSO edge in edges)
        {
            //TODO change query according to database.
            string query = QueryStatement(edge.QID, queries.testOnWikiData);
            StartCoroutine(UpdateEdgeSO(edge, query));
        }
    }

    void UpdateNodes(List<NodeSO> nodes) 
    {
        foreach(NodeSO node in nodes) 
        {
            //TODO change query according to database.
            string query = QueryStatement(node.QID, queries.testOnWikiData);
            StartCoroutine(UpdateNodeSO(node, query));
        }
    }

    string QueryStatement(string qid, string query)
    {
        string WQS = "http://query.wikidata.org/sparql?query=";
        query = query.Replace("ID", qid);
        string queryReady = UnityWebRequest.EscapeURL(query);
        return WQS + queryReady;
    }

    IEnumerator UpdateNodeSO(NodeSO node, string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(webRequest.downloadHandler.text);
                XmlNodeList queriedData = doc.GetElementsByTagName("result")[0].ChildNodes;
                //TODO update parameters that we want to update
                node.Label = queriedData[0].InnerText;
                node.Description = queriedData[1].InnerText;
            }
        }
    }

    IEnumerator UpdateEdgeSO(EdgeSO edge, string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(webRequest.downloadHandler.text);
                XmlNodeList queriedData = doc.GetElementsByTagName("result")[0].ChildNodes;
                //TODO update parameters that we want to update
                edge.Label = queriedData[0].InnerText;
            }
        }
    }

    //function to help debug
    static void SaveXML(string str) {
        string fileDestination = "Assets/Resources/Data/query.xml";
        FileStream filestream = new FileStream(fileDestination, FileMode.Create);
        StreamWriter writer = new StreamWriter(filestream);
        str = str.Replace("http://www.wikidata.org/entity/Q","");
        writer.Write(str);
        writer.Close();
    }
}

