// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using System.IO;
// using UnityEditor;

// // This script will be generalized to communicate with our database

// public class WikiDataConnect : MonoBehaviour
// {
//     public static string fileDestination = "Assets/Resources/Data/query.xml";
//     void Start()
//     {
//         string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";
//         string queryRaw = "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/>" +
// "select distinct ?prefixedEdge" +
// "(strafter(?prefixedEdge,\":\") as ?edgeQID)" +
// "?prefixedMetabolite" +
// "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID)" +
// "?edgeLabel ?metaboliteLabel ?isReactant ?isProduct" +
// "?prefixedEnzyme" +
// "(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) ?enzymeLabel where {" +
// "foaf:Q88 wdt:P4 ?edge." +
// "?edge p:P4 ?statement." +
// "?edge wdt:P14 ?enzyme." +
// "?statement ps:P4 ?metabolite." +
// "?statement pq:P31|pq:P32 ?edge." +
// "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
// "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
// "BIND(replace(str(?enzyme), str(foaf:), \"foaf:\") as ?prefixedEnzyme)" +
// "BIND ( EXISTS { ?statement pq:P31 ?edge } as ?isReactant )" +
// "BIND ( EXISTS { ?statement pq:P32 ?edge } as ?isProduct )" +
// "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }" +
// "}";
//         string queryReady = UnityWebRequest.EscapeURL(queryRaw);
//         Debug.Log(WQS +queryReady);
        
//     }

//     IEnumerator GetRequest(string uri)
//     {
//         using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
//         {
//             // Request and wait for the desired page.
//             webRequest.SetRequestHeader("content-type", "application/sparql-results+json");
//             yield return webRequest.SendWebRequest();
//             string[] pages = uri.Split('/');
//             int page = pages.Length - 1;
//             if (webRequest.isNetworkError)
//             {
//                 Debug.Log(pages[page] + ": Error: " + webRequest.error);
//             }
//             else
//             {
//                 Debug.Log(webRequest.downloadHandler.text);
//                 WriteString(webRequest.downloadHandler.text);
//             }
//         }
//     }

//     static void WriteString(string str)
//     {
//         FileStream filestream = new FileStream(fileDestination, FileMode.Create);
//         StreamWriter writer = new StreamWriter(filestream);
//         writer.Write(str);
//         writer.Close();
//     }

// }