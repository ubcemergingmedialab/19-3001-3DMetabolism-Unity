
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
        "?enzymeLabel ?pathwayDesc ?edgeDesc ?metaboliteDesc where {";
    public static string queryRawSecond = " p:P4 ?edgeStatement." +
        "?pathway schema:description ?pathwayDesc."+
        "?edgeStatement ps:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge schema:description ?edgeDesc." +
        "?edge wdt:P14 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?metabolite schema:description ?metaboliteDesc." +
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

        if (GUILayout.Button("connect eligible scriptable objects to prefabs"))
        {
            //PrefabService prefabService = new PrefabService();
            GameObject.Find("QueryService").GetComponent<PrefabService>().PrefabAssignment();
            // prefabService.PrefabAssignment();
        }

        // active pathways are now filled with SOs from query using this button 
        // TODO: active pathways needs to be cleared if the SOs are deleted, this is done manually atm
        if (GUILayout.Button("Update active pathways in StatusController"))
        {
            Dictionary<string,PathwaySO> tempDict = QueryService.PathwaySOs;
            //GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Clear();
            foreach(KeyValuePair<string,PathwaySO> pair in tempDict)
            {
                Debug.Log("<Test> pw name : " + pair.Value.Label);
                GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Add(pair.Value);

            }
        }

        
    }




}
