
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// [CustomEditor(typeof(QueryEditor))]
public class QueryCustomEditor : EditorWindow
{

    string targetPathwayQID = "ALL";
    public static string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";
    public static string queryRawFirst = "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " + 
        "select distinct " +
        "?pathwayLabel (STRAFTER(?prefixedPathway, \":\") AS ?pathwayQID) "+
        "(strafter(?prefixedEdge,\":\") as ?edgeQID) " +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID) " +
        "?edgeLabel ?metaboliteLabel ?enzymeLabel ?isBidirectional " +
        "?metaboliteMoleFormula ?metaboliteIUPAC ?metaboliteStrucDesc ?metaboliteCharge ?metabolitePubchem " +
        "?edgeEnzymeTypeLabel ?edgeCofactorsLabel ?edgeEnergyReqLabel ?edgePubchem ?edgeRegulation " +
        "?isReactant ?isProduct ?isEnzyme "+
        "?pathwayDesc ?edgeDesc ?metaboliteDesc where { ";
    public static string queryRawSecond = " p:P4 ?edgeStatement." +
        "?pathway schema:description ?pathwayDesc."+
        "?edgeStatement ps:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge schema:description ?edgeDesc." +
        "?edge p:P4 ?enzymeStatement." +
        "?enzymeStatement ps:P4 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?metabolite schema:description ?metaboliteDesc." +
        "?metabolite wdt:P37 ?metaboliteMoleFormula." + 
        "?metabolite wdt:P38 ?metaboliteIUPAC." + 
        "?metabolite wdt:P44 ?metaboliteStrucDesc." + 
        "?metabolite wdt:P27 ?metaboliteCharge." + 
        "?metabolite wdt:P45 ?metabolitePubchem." + 
        "?statement (pq:P31|pq:P32) ?edge." +
        "?enzymeStatement (pq:P42) ?edge." +
        "?edge wdt:P14 ?edgeEnzymeType." + 
        "?edge wdt:P22 ?edgeCofactors." + 
        "?edge wdt:P13 ?edgeEnergyReq." + 
        "?edge wdt:P45 ?edgePubchem." + 
        "?edge wdt:P43 ?edgeRegulation." + 
        "BIND(REPLACE(STR(?pathway), STR(foaf:), \"foaf:\") AS ?prefixedPathway) " +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND ( EXISTS { ?statement pq:P31 ?edge. } as ?isReactant )" +
        "BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge. } as ?isProduct )" +
        "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". } \n }" ;

    public static string qRawFull =   "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " + 
        "select distinct " +
        "?pathwayLabel (STRAFTER(?prefixedPathway, \":\") AS ?pathwayQID) "+
        "(strafter(?prefixedEdge,\":\") as ?edgeQID) " +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID) " +
        "?edgeLabel ?metaboliteLabel ?enzymeLabel ?isBidirectional " +
        "?metaboliteMoleFormula ?metaboliteIUPAC ?metaboliteStrucDesc ?metaboliteCharge ?metabolitePubchem " +
        "?edgeEnzymeTypeLabel ?edgeCofactorsLabel ?edgeEnergyReq ?edgePubchem ?edgeRegulation " +
        "?isReactant ?isProduct ?isEnzyme "+
        "?pathwayDesc ?edgeDesc ?metaboliteDesc where {" +
        "?pathway p:P4 ?edgeStatement." +
        "?pathway schema:description ?pathwayDesc."+
        "?edgeStatement ps:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge schema:description ?edgeDesc." +
        "?edge p:P4 ?enzymeStatement." +
        "?enzymeStatement ps:P4 ?enzyme." +
        "?statement ps:P4 ?metabolite." +
        "?metabolite schema:description ?metaboliteDesc." +
        "?metabolite wdt:P37 ?metaboliteMoleFormula." + // new
        "?metabolite wdt:P38 ?metaboliteIUPAC." + // new
        "?metabolite wdt:P44 ?metaboliteStrucDesc." + // new
        "?metabolite wdt:P27 ?metaboliteCharge." + //new
        "?metabolite wdt:P45 ?metabolitePubchem." + //new
        "?statement (pq:P31|pq:P32) ?edge." +
        "?enzymeStatement (pq:P42) ?edge." +
        "?edge wdt:P14 ?edgeEnzymeType." + // new
        "?edge wdt:P22 ?edgeCofactors." + // new
        "?edge wdt:P13 ?edgeEnergyReq." + //new
        "?edge wdt:P45 ?edgePubchem." + //new
        "?edge wdt:P43 ?edgeRegulation." + //new
        "BIND(REPLACE(STR(?pathway), STR(foaf:), \"foaf:\") AS ?prefixedPathway) " +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND ( EXISTS { ?statement pq:P31 ?edge. } as ?isReactant )" +
        "BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge. } as ?isProduct )" +
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
        
        if (GUILayout.Button("delete current scriptable objects"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().ClearQueryData();
        }

         if (GUILayout.Button("delete query xml"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().DeleteQueryXml();
        }

        if (GUILayout.Button("run query"))
        { 
            string qRawFull = queryRawFirst + temp + queryRawSecond ;

            GameObject.Find("QueryService").GetComponent<QueryService>().RunQuery(WQS,qRawFull);
        }
  
        // if (GUILayout.Button("Update active pathways in StatusController"))
        // {
        //     Dictionary<string,PathwaySO> tempDict = QueryService.PathwaySOs;
        //     GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Clear();
        //     foreach(KeyValuePair<string,PathwaySO> pair in tempDict)
        //     {
        //         Debug.Log("<StatusContorller List> pw name being added : " + pair.Value.Label);
        //         GameObject.Find("StatusController").GetComponent<StatusController>().activePathways.Add(pair.Value);

        //     }
        // }

        if (GUILayout.Button("Print local networks"))
        {
            List<PathwaySO> tempList = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;

            foreach(PathwaySO pw in tempList){
                Debug.Log("<!> local network count : " + pw.LocalNetwork.Count);
                if(pw.LocalNetwork == null){
                    Debug.Log("<!> local network NULL");
                }
                foreach(KeyValuePair<NodeSO, List<EdgeSO>> pair in pw.LocalNetwork){
                    Debug.Log("pathway: " + pw.Label + " network \n" + "node: " + pair.Key + " edge :");
                    foreach(EdgeSO edge in pair.Value){
                        Debug.Log(edge.Label);
                    }
                }
            }
            // Dictionary<string,PathwaySO> tempDict = QueryService.PathwaySOs;
            // foreach(KeyValuePair<string,PathwaySO> pw in tempDict){
            //     Debug.Log("<!> local network count : " + pw.Value.LocalNetwork);
            //     if(pw.Value.LocalNetwork == null){
            //         Debug.Log("<!> local network empty");
            //     }
            //     foreach(KeyValuePair<NodeSO, List<EdgeSO>> pair in pw.Value.LocalNetwork){
            //         Debug.Log("pathway: " + pw.Key + " network \n" + "node: " + pair.Key + " edge :");
            //         foreach(EdgeSO edge in pair.Value){
            //             Debug.Log(edge.Label);
            //         }
            //     }            
            // }
        }

        // if (GUILayout.Button("connect eligible scriptable objects to prefabs"))
        // {
        //     //PrefabService prefabService = new PrefabService();
        //     GameObject.Find("PrefabService").GetComponent<PrefabService>().PrefabAssignment();
        //     // prefabService.PrefabAssignment();
        // }

        // // active pathways are now filled with SOs from query using this button 
        // // TODO: active pathways needs to be cleared if the SOs are deleted, this is done manually atm
        

         

        //  if (GUILayout.Button("Fill pathway list (last click)"))
        // {
        //    GameObject.Find("QueryService").GetComponent<QueryService>().FillPathwayList();
        // }
        //  if (GUILayout.Button("Fill node and edges list (statusController)"))
        // {
        //    GameObject.Find("StatusController").GetComponent<StatusController>().FillItemReferenceList();
        // }
    }




}
