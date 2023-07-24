
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// [CustomEditor(typeof(QueryEditor))]
public class QueryCustomEditor : EditorWindow
{
    string targetTag = "alanine aminotransferase";
    string targetPathwayQID = "ALL";
    public static string WQS = "http://wikibase-3dm.eml.ubc.ca:8282/proxy/wdqs/bigdata/namespace/wdq/sparql?format=json&query=";

    public static string qRawFull =   "PREFIX foaf: <http://wikibase-3dm.eml.ubc.ca/entity/> " + 
        "select distinct " +
        "?pathwayLabel (STRAFTER(?prefixedPathway, \":\") AS ?pathwayQID) "+
        "(strafter(?prefixedEdge,\":\") as ?edgeQID) " +
        "(strafter(?prefixedMetabolite,\":\") as ?metaboliteQID) " +
        "?edgeLabel ?metaboliteLabel ?enzymeLabel ?isBidirectional " +
        "?metaboliteMoleFormula ?metaboliteIUPAC ?metaboliteStrucDesc ?metaboliteCharge ?metabolitePubchem " +
        "?edgeEnzymeTypeLabel ?cofactorLabel ?edgeEnergyReq ?edgePubchem ?edgeRegulation " +
        "?isReactant ?isProduct ?isEnzyme ?isCofactorReactant ?isCofactorProduct" +
        "?pathwayDesc ?edgeDesc ?metaboliteDesc where {" +
        "?pathway p:P4 ?edgeStatement." +
        "?pathway schema:description ?pathwayDesc."+
        "?edgeStatement ps:P4 ?edge." +
        "?edge p:P4 ?statement." +
        "?edge schema:description ?edgeDesc." +
        "?edge p:P4 ?enzymeStatement." +
        "?edge p:P22 ?cofactorStatement." +
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
        "?cofactorStatement ps:P22 ?cofactor." +//new
         "OPTIONAL { ?cofactorStatement(pq:P31|pq:P32) ?edge. " +//new
         "BIND(EXISTS {?cofactorStatement pq:P31 ?edge} as ?isCofactorReactant) " +//new
         "BIND(EXISTS {?cofactorStatement pq:P32 ?edge} as ?isCofactorProduct) } " +//new
        "BIND(REPLACE(STR(?pathway), STR(foaf:), \"foaf:\") AS ?prefixedPathway) " +
        "BIND(replace(str(?edge), str(foaf:), \"foaf:\") as ?prefixedEdge)" +
        "BIND(replace(str(?metabolite), str(foaf:), \"foaf:\") as ?prefixedMetabolite)" +
        "BIND ( EXISTS { ?statement pq:P31 ?edge. } as ?isReactant )" +
        "BIND ( EXISTS {?edgeStatement pq:P40 ?edgeDirection } as ?isBidirectional )" +
        "BIND ( EXISTS { ?statement pq:P32 ?edge. } as ?isProduct )" +
        "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". } \n }" ;

    [MenuItem("Window/QueryServiceEditor")]
    public static void ShowWindow ()
    {
        GetWindow<QueryCustomEditor>("Query Service");
    }

    void OnGUI ()
    {   
        GUILayout.Label("Query to Unity", EditorStyles.boldLabel);
        targetTag = EditorGUILayout.TextField("Target object tag:", targetTag);
        
        
        if (GUILayout.Button("delete current scriptable objects"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().ClearQueryData();
        }

         if (GUILayout.Button("delete query xml"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().DeleteQueryXml();
            Debug.Log("Deleted XML");
        }

        if (GUILayout.Button("run query"))
        {
            GameObject.Find("QueryService").GetComponent<QueryService>().RunQuery(WQS,qRawFull);
            Debug.Log("RunQuery Complete");
        }
 

        if (GUILayout.Button("Print local networks"))
        {
            List<PathwaySO> tempList = GameObject.Find("StatusController").GetComponent<StatusController>().activePathways;

            foreach(PathwaySO pw in tempList){
                Debug.Log("<!> local network count : " + pw.LocalNetwork.Count);
                if(pw.LocalNetwork == null){
                    Debug.Log("<!> local network NULL");
                }
                foreach(KeyValuePair<NodeSO, HashSet<EdgeSO>> pair in pw.LocalNetwork){
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
        if (GUILayout.Button("find tagged objs"))
        {
            GameObject[] tagged = GameObject.FindGameObjectsWithTag(targetTag);
            
            foreach(GameObject obj in tagged)
            {
                Debug.Log("obj:" + obj.name);
            }
        }


    }
}
