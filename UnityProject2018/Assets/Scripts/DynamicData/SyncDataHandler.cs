using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class SyncDataHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static string path = "Assets/Resources/Data/query.xml";
    public List<PathwaySO> pathways;
    void Start()
    {
        
        LoadXML();
    }

    void QueryData()
    {
        string WQS = "http://query.wikidata.org/sparql?query=";
        string queryRaw = "SELECT ?item ?itemLabel " +
            "WHERE {wd:Q50293158 wdt:P527 ?item. " +
            "SERVICE wikibase:label { bd:serviceParam wikibase:language \"[AUTO_LANGUAGE],en\". " +
            "} " +
            "}";
        string queryReady = UnityWebRequest.EscapeURL(queryRaw);
        StartCoroutine(GetRequest(WQS + queryReady));
    }

    private void LoadXML()
    {
        /*
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        //the name of the object
        XmlNodeList elem = doc.GetElementsByTagName("literal");
        List<string> node_names = new List<string>();
        foreach (XmlNode node in elem)
            node_names.Add(node.InnerText);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Label = node_names[i];
        }
        */

    }
}
