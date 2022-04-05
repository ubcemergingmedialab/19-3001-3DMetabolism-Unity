
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//[CustomEditor(typeof(QueryEditor))]
public class QueryCustomEditor : EditorWindow
{

    string targetPathwayQID = "Here";
    public static string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";
    public static string queryRawFirst = "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " +
        "select distinct " +
        "?pathwayLabel (STRAFTER(?prefixedPathway, \":\") AS ?pathwayQID) "+
        "(strafter(?prefixedEdge,\":\") as ?edgeQID) " +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID) " +
        "(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) " +
        "?edgeLabel ?metaboliteLabel ?isBidirectional ?isReactant ?isProduct "+
        "?enzymeLabel where {";
    public static string queryRawSecond = " p:P4 ?edgeStatement." +
        "?edgeStatement ps:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge wdt:P14 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?statement pq:P31|pq:P32 ?edge." +
        "BIND(REPLACE(STR(?pathway), STR(foaf:), \"foaf:\") AS ?prefixedPathway) " +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND(replace(str(?enzyme), str(foaf:), \"foaf:\") as ?prefixedEnzyme)"+
        "BIND ( EXISTS { ?statement pq:P31 ?edge } as ?isReactant )" +
        "BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge } as ?isProduct )" +
        "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". } \n }" ;

    public static string qRawFull =   "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " +
        "select distinct " +
        "?pathwayLabel (strafter(?prefixedEdge,\":\") as ?edgeQID) " +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID) " +
        "(strafter(?prefixedEnzyme,\":\") as ?enzymeQID) " +
        "?edgeLabel ?metaboliteLabel ?isBidirectional ?isReactant ?isProduct "+
        "?enzymeLabel where {" +
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
        GetWindow<QueryCustomEditor>("Query Service");
    }

    void OnGUI ()
    {   
        string temp;
        GUILayout.Label("Query to Unity", EditorStyles.boldLabel);
        GUILayout.Label("Put  \"ALL\" to query all pathways \n currently only work with ALL");
        targetPathwayQID = EditorGUILayout.TextField("Target pathway QID:",targetPathwayQID);
        
        if(targetPathwayQID == "ALL"){
            temp = "?pathway";
        } else {
            temp = "foaf:" + targetPathwayQID;
        }
        

        if (GUILayout.Button("run query and create Scriptable objects"))
        { 
            string qRawFull = queryRawFirst + temp + queryRawSecond ;

            GameObject.Find("QueryService").GetComponent<QueryService>().RunQuery(WQS,qRawFull);
        }
  
        if (GUILayout.Button("delete current scriptable objects"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().ClearQueryData();
        }
        
    }




}
