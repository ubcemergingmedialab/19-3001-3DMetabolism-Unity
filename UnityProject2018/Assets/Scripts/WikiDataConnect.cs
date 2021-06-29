using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEditor;

// This script will be generalized to communicate with our database

public class WikiDataConnect : MonoBehaviour
{
    public static string fileDestination = "Assets/Resources/Data/query.xml";
    void Start()
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

    IEnumerator GetRequest(string uri)
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
                WriteString(webRequest.downloadHandler.text);
            }
        }
    }

    static void WriteString(string str)
    {
        FileStream filestream = new FileStream(fileDestination, FileMode.Create);
        StreamWriter writer = new StreamWriter(filestream);
        writer.Write(str);
        writer.Close();
    }

}