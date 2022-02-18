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
        string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?query=" +
            "select%20*%20where%20%7B%20%3Chttp%3A%2F%2Fwikibase-3dm.eml.ubc.ca%2Fentity%2FQ88%3E%20%3Chttp%3A%2F%2Fwikibase-3dm.eml.ubc.ca%2Fprop%2Fdirect%2FP4%3E%20%3Fc%7D";
        StartCoroutine(GetRequest(WQS));
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
                Debug.Log(webRequest.downloadHandler.text);
                WriteString(webRequest.downloadHandler.text);
                Application.LoadLevel("_Main");
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