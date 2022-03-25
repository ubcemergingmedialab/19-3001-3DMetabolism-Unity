
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//[CustomEditor(typeof(QueryEditor))]
public class QueryEditor : EditorWindow
{

    string targetPathwayQID = "Here";
    public static string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";
    public static string queryRawFirst = "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/>" +
        "select distinct ?prefixedEdge" +
        "(strafter(?prefixedEdge,\":\") as ?edgeQID)" +
        "?prefixedMetabolite" +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID)" +
        "?edgeLabel ?metaboliteLabel ?isReactant ?isProduct ?isBidirectional" +
        "?prefixedEnzyme" +
        "(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) ?enzymeLabel where {" +
        "foaf:";
    public static string queryRawSecond = " wdt:P4 ?edge." +
        "foaf:";
    public static string queryRawThird =
        " p:P4 ?edgeStatement." +
        "?edge p:P4 ?statement." +
        "?edge wdt:P14 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?statement pq:P31|pq:P32 ?edge." +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND(replace(str(?enzyme), str(foaf:), \"foaf:\") as ?prefixedEnzyme)" +
        "BIND ( EXISTS { ?statement pq:P31 ?edge } as ?isReactant )" +
        "BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge } as ?isProduct )" +
        "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }" +
        "}";

    public static string qRawTemp =   "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " +
"select distinct ?prefixedEdge" +
"(strafter(?prefixedEdge,\":\") as ?edgeQID)" +
"?prefixedMetabolite" +
"(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID)" +
"?edgeLabel ?metaboliteLabel ?isBidirectional ?isReactant ?isProduct"+
"?prefixedEnzyme" +
"(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) ?enzymeLabel where {" +
"foaf:Q88 p:P4 ?edgeStatement." +
"?edgeStatement ps:P4 ?edge." +
"?edge p:P4 ?statement." +
"?edge wdt:P14 ?enzyme." +
"?statement ps:P4 ?metabolite." +
"?statement pq:P31|pq:P32 ?edge." +
"BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
"BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
"BIND(replace(str(?enzyme), str(foaf:), \"foaf:\") as ?prefixedEnzyme)"+
"BIND ( EXISTS { ?statement pq:P31 ?edge } as ?isReactant )" +
"BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
"BIND ( EXISTS { ?statement pq:P32 ?edge } as ?isProduct )" +
"SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". } \n }" ;

    [MenuItem("Window/QueryService")]
    public static void ShowWindow ()
    {
        GetWindow<QueryEditor>("Query Service");
    }

    void OnGUI ()
    {
        GUILayout.Label("Query to Unity", EditorStyles.boldLabel);

        targetPathwayQID = EditorGUILayout.TextField("Target pathway QID:",targetPathwayQID);

        

        if (GUILayout.Button("run query and create Scriptable objects"))
        { 
            string qRawFull = queryRawFirst + targetPathwayQID + queryRawSecond + targetPathwayQID + queryRawThird;
            GameObject.Find("PathwayMock").GetComponent<QueryToUnity>().RunQuery(WQS,qRawTemp);
        }
  
        if (GUILayout.Button("delete current scriptable objects"))
        {
            GameObject.Find("PathwayMock").GetComponent<QueryToUnity>().ClearQueryData();
        }
        
    }




}
