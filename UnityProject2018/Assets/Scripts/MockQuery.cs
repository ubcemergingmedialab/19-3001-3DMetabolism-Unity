using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockQuery : MonoBehaviour
{
    public List<EdgeSOBeta> edges;
    public List<NodeSOBeta> nodes;
    // Start is called before the first frame update

    /* TODO:
        - we check for visited in both search and find childern and I think thats an issue
        - write assertion statements for the BFS
    */
    void Start()
    {  
        //Glycogen sythanse pathway

        PathwaySOBeta glycogenSynthasePathway = ScriptableObject.CreateInstance<PathwaySOBeta>();
        glycogenSynthasePathway.init("glycogen Synthase pathway");
        //glycogenSynthasePathway.LocalNetwork = new Dictionary<NodeSOBeta, List<EdgeSOBeta>>();
        // hexokinase , glucose -> glucose6phospahate 
        EdgeSOBeta hexokinase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        hexokinase.init("hexokinase");
        // hexokinase.Label = "hexokinase";
        // hexokinase.name = "hexokinase";
        edges.Add(hexokinase);
        

        NodeSOBeta glucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(glucose);
        glucose.init("Glucose");
        // glucose.Label = "glucose";
        // glucose.name = "glucose";
        NodeSOBeta glucose6phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(glucose6phosphate);
        glucose6phosphate.init("glucose-6-phosphate");
        // glucose6phosphate.Label = "glucose-6-phosphate";
        // glucose6phosphate.name = "glucose-6-phosphate";
        hexokinase.AddReactant(glucose);
        hexokinase.AddProduct(glucose6phosphate);
        // hexokinase.reactants = new List<NodeSOBeta>();
        // hexokinase.reactants.Add(glucose);
        // hexokinase.products = new List<NodeSOBeta>();
        // hexokinase.products.Add(glucose6phosphate);
        glycogenSynthasePathway.AddNodeToPathway(glucose);
        glycogenSynthasePathway.AddEdgeToPathway(glucose,hexokinase);
        glycogenSynthasePathway.AddNodeToPathway(glucose6phosphate);
        glycogenSynthasePathway.AddEdgeToPathway(glucose6phosphate,hexokinase);
    
        // glycogenSynthasePathway.LocalNetwork.Add(glucose, new List<EdgeSOBeta>{hexokinase});
        // glycogenSynthasePathway.LocalNetwork.Add(glucose6phosphate, new List<EdgeSOBeta>{hexokinase});

        // Phosphoglucose mutase , glucose 6-phosphate => glucose 1-phosphate
        EdgeSOBeta phosphoglucoseMutase = ScriptableObject.CreateInstance<EdgeSOBeta>();        
        edges.Add(phosphoglucoseMutase);
        phosphoglucoseMutase.init("phosphoglucose mutase");
        // phosphoglucoseMutase.Label = "phosphoglucose mutase";
        // phosphoglucoseMutase.name = "phosphoglucose mutase";


        NodeSOBeta glucose1phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(glucose1phosphate);
        glucose1phosphate.init("glucose-1-phosphate");
        // glucose1phosphate.Label = "glucose-1-phosphate";
        // glucose1phosphate.name = "glucose-1-phosphate";
        phosphoglucoseMutase.AddReactant(glucose6phosphate);
        phosphoglucoseMutase.AddProduct(glucose1phosphate);
        // phosphoglucoseMutase.reactants = new List<NodeSOBeta>();
        // phosphoglucoseMutase.reactants.Add(glucose6phosphate);
        // phosphoglucoseMutase.products = new List<NodeSOBeta>();
        // phosphoglucoseMutase.products.Add(glucose1phosphate);

        glycogenSynthasePathway.AddEdgeToPathway(glucose6phosphate,phosphoglucoseMutase);
        //glycogenSynthasePathway.LocalNetwork[glucose6phosphate].Add(phosphoglucoseMutase);
        glycogenSynthasePathway.AddNodeToPathway(glucose1phosphate);
        glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,phosphoglucoseMutase);
        //glycogenSynthasePathway.LocalNetwork.Add(glucose1phosphate, new List<EdgeSOBeta>{phosphoglucoseMutase});

        // UDP-glucose pyrophosphorylase , UTP + glucose 1-phosphate <=> UDP-glucose + PPi     --> bi directional!
        EdgeSOBeta UDPGlucosePyrophosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        edges.Add(UDPGlucosePyrophosphorylase);
        UDPGlucosePyrophosphorylase.init("UDP-glucose pyrophosphorylase",true);
        // UDPGlucosePyrophosphorylase.Label = "UDP-glucose pyrophosphorylase";
        // UDPGlucosePyrophosphorylase.name = "UDP-glucose pyrophosphorylase";

        NodeSOBeta UDPglucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(UDPglucose);
        UDPglucose.init("UDP-glucose");
        // UDPglucose.Label = "UDP-glucose";
        // UDPglucose.name = "UDP-glucose";
        UDPGlucosePyrophosphorylase.AddReactant(glucose1phosphate);
        UDPGlucosePyrophosphorylase.AddProduct(UDPglucose);
        // UDPGlucosePyrophosphorylase.reactants = new List<NodeSOBeta>();
        // UDPGlucosePyrophosphorylase.reactants.Add(glucose1phosphate);
        // UDPGlucosePyrophosphorylase.products = new List<NodeSOBeta>();
        // UDPGlucosePyrophosphorylase.products.Add(UDPglucose);
        // UDPGlucosePyrophosphorylase.bidirectional = true;
        //glycogenSynthasePathway.AddNodeToPathway(glucose1phosphate);
        glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,UDPGlucosePyrophosphorylase);
        glycogenSynthasePathway.AddNodeToPathway(UDPglucose);
        glycogenSynthasePathway.AddEdgeToPathway(UDPglucose,UDPGlucosePyrophosphorylase);

        // glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(UDPGlucosePyrophosphorylase);
        // glycogenSynthasePathway.LocalNetwork.Add(UDPglucose, new List<EdgeSOBeta>{UDPGlucosePyrophosphorylase});


        // glycogen synthase, glycogen (n residues) + UDP-glucose => UDP + glycogen (n+1 residues)
        EdgeSOBeta glycogenSynthase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        edges.Add(glycogenSynthase);
        glycogenSynthase.init("glycogenSynthase");
        // glycogenSynthase.Label = "glycogenSynthase";
        // glycogenSynthase.name = "glycogenSynthase";

        NodeSOBeta glycogen_n1 = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(glycogen_n1);
        glycogen_n1.init("glycogen(n+1)");
        // glycogen_n1.Label = "glycogen(n+1)";
        // glycogen_n1.name = "glycogen(n+1)";
        NodeSOBeta glycogen_n = ScriptableObject.CreateInstance<NodeSOBeta>();
        nodes.Add(glycogen_n);
        glycogen_n.init("glycogen(n)");
        // glycogen_n.Label = "glycogen(n)";
        // glycogen_n.name = "glycogen(n)";

        glycogenSynthase.AddReactant(glycogen_n);
        glycogenSynthase.AddReactant(UDPglucose);
        // glycogenSynthase.reactants = new List<NodeSOBeta>();
        // glycogenSynthase.reactants.Add(glycogen_n);
        // glycogenSynthase.reactants.Add(UDPglucose);
        glycogenSynthase.AddProduct(glycogen_n1);
        // glycogenSynthase.products = new List<NodeSOBeta>();
        // glycogenSynthase.products.Add(glycogen_n1);

        glycogenSynthasePathway.AddNodeToPathway(glycogen_n1);
        glycogenSynthasePathway.AddNodeToPathway(glycogen_n);
        glycogenSynthasePathway.AddEdgeToPathway(glycogen_n,glycogenSynthase);
        glycogenSynthasePathway.AddEdgeToPathway(glycogen_n1,glycogenSynthase);
        glycogenSynthasePathway.AddEdgeToPathway(UDPglucose,glycogenSynthase);
        // glycogenSynthasePathway.LocalNetwork.Add(glycogen_n1, new List<EdgeSOBeta>{glycogenSynthase});
        // glycogenSynthasePathway.LocalNetwork.Add(glycogen_n, new List<EdgeSOBeta>{glycogenSynthase});
        // glycogenSynthasePathway.LocalNetwork[UDPglucose].Add(glycogenSynthase);

        // glycogen phosphorylase, glycogen (n+1 residues) + Pi => glycogen (n residues) + glucose 1-phosphate  --> not in wikibase!, needs to be checked with a theory
        EdgeSOBeta glycogenPhosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        edges.Add(glycogenPhosphorylase);
        glycogenPhosphorylase.init("glycogen Phosphorylase");

        // glycogenPhosphorylase.Label = "glycogen Phosphorylase";
        // glycogenPhosphorylase.name = "glycogen Phosphorylase";

        glycogenPhosphorylase.AddReactant(glycogen_n1);
        glycogenPhosphorylase.AddProduct(glycogen_n);
        glycogenPhosphorylase.AddProduct(glucose1phosphate);

        // glycogenPhosphorylase.reactants = new List<NodeSOBeta>();
        // glycogenPhosphorylase.reactants.Add(glycogen_n1);
        // glycogenPhosphorylase.products = new List<NodeSOBeta>();
        // glycogenPhosphorylase.products.Add(glycogen_n);
        // glycogenPhosphorylase.products.Add(glucose1phosphate);
        glycogenSynthasePathway.AddEdgeToPathway(glycogen_n,glycogenPhosphorylase);
        glycogenSynthasePathway.AddEdgeToPathway(glycogen_n1,glycogenPhosphorylase);
        glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,glycogenPhosphorylase);
        // glycogenSynthasePathway.LocalNetwork[glycogen_n1].Add(glycogenPhosphorylase);
        // glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(glycogenPhosphorylase);        
        // glycogenSynthasePathway.LocalNetwork[glycogen_n].Add(glycogenPhosphorylase);




        BFSTest(glycogenSynthasePathway,glucose6phosphate,glycogen_n);
    }

      List<ScriptableObject> SearchForPath(PathwaySOBeta pathway, NodeSOBeta nodeRoot, NodeSOBeta nodeToFind) {

       Queue<List<ScriptableObject>> BFSQueue = new Queue<List<ScriptableObject>>();
       Dictionary<string,bool> visited = new Dictionary<string, bool>();

       BFSQueue.Enqueue(new List<ScriptableObject>(){nodeRoot});
       visited.Add(nodeRoot.Label,true);

       while (BFSQueue.Count > 0) {
           List<ScriptableObject> currentPath = BFSQueue.Dequeue();
           NodeSOBeta currentNode = (NodeSOBeta) currentPath[currentPath.Count - 1];
           if (currentNode.Label == nodeToFind.Label){
               // print the path later
               Debug.Log("found the node : " + currentNode.Label + " = " + nodeToFind.Label);
               return currentPath;
           }

           Dictionary<EdgeSOBeta,List<NodeSOBeta>> children = FindChildren(pathway,visited,currentNode);
            //iterate through the dictionary
            // add the key and the each of the values to the current path list
            // currentpath += Key + value of all dict elements
            // if last elemetn of current path is in visited then ignore 
            // else :
            //
            // BFSQueue.Enqueue(currentPath)

            // in case this doesnt duplicate, create a new list per iteration and enqueue .
            // new list of SO =of current path, enqueue this instead

            
            foreach(KeyValuePair <EdgeSOBeta, List<NodeSOBeta>> entry in children) {
                

                foreach (NodeSOBeta curr in entry.Value){
                    List<ScriptableObject> newPath = new List<ScriptableObject>(currentPath);
                    newPath.Add(entry.Key);
                    newPath.Add(curr);
                    //string lastNodeLabel = newPath[newPath.Count - 1].Label;
                    if (visited.ContainsKey(curr.Label)){
                    continue;
                    }
                    visited.Add(curr.Label,true);
                    BFSQueue.Enqueue(newPath);
                }
                
            }
            
           
       }
       Debug.Log("No Path found and null is returned");
       return null;
    }

    /*
        go through all edges
        make a new keyvalue pair per edge
        add all the nodes eligible connected to this edge as value
        return the dictionarry
    */
    Dictionary<EdgeSOBeta,List<NodeSOBeta>> FindChildren(PathwaySOBeta pathway,Dictionary<string,bool> visited, NodeSOBeta current)
    {
        
        Dictionary<EdgeSOBeta,List<NodeSOBeta>> nodesByEdge = new Dictionary<EdgeSOBeta,List<NodeSOBeta>>();
        List<EdgeSOBeta> interactedEdges = pathway.LocalNetwork[current];
        foreach (EdgeSOBeta currentEdge in interactedEdges){

            nodesByEdge.Add(currentEdge, new List<NodeSOBeta>());
            if (currentEdge.bidirectional){
                Debug.Log("BFS found bidirectional edge");
                //search products and reactants, return the one where we dont find node in

                if (currentEdge.products.Contains(current)){

                    foreach (NodeSOBeta node in currentEdge.reactants){
                        nodesByEdge[currentEdge].Add(node);

                    }
                } else {
                    if (currentEdge.reactants.Contains(current)){

                        foreach (NodeSOBeta node in currentEdge.products){
                            // if(!visited.ContainsKey(node.Label)){
                                nodesByEdge[currentEdge].Add(node);
                        //         visited.Add(node.Label,true);
                        //     }
                        }
                    }
                }
            } else {
                if (currentEdge.reactants.Contains(current)){

                    foreach (NodeSOBeta node in currentEdge.products){
                        // if(!visited.ContainsKey(node.Label)){
                            nodesByEdge[currentEdge].Add(node);
                        //     visited.Add(node.Label,true);
                        // }
                    }
                }
            }
        }
        return nodesByEdge;
    }




    public void BFSTest(PathwaySOBeta pathway, NodeSOBeta start, NodeSOBeta end) {
        List<ScriptableObject> result =  SearchForPath(pathway,start,end);
        string printResult = "search in " + pathway.name + "from node:" + start.Label +  " - end node:" + end.Label;
        int n = 1;
        foreach (ScriptableObject step in result){
            printResult += "\n" + n + " - " + step.name; //GetType().ToString();
            n++;   
        }
        Debug.Log(printResult);
    }

    // // add new pathwaySO
    // public void AddPathwaySO(string name){
    //     PathwaySOBeta pathway = ScriptableObject.CreateInstance<PathwaySOBeta>();
    //     pathway.LocalNetwork = new Dictionary<NodeSOBeta, List<EdgeSOBeta>>();

    //     pathway.name = name;
    //     pathway.Label = name;
    // }


    // public void AddNodeToPathway(PathwaySOBeta pathway, NodeSOBeta node){
    //     if (!(pathway.LocalNetwork.ContainsKey(node))){
    //         pathway.LocalNetwork.Add(node, new List<EdgeSOBeta>());  
    //     }
    // }

    // Add new node to a pathway
    // public void AddNodeSO(string name, List<NodeSOBeta> trackerList, PathwaySOBeta pathway) {
        
    //     NodeSOBeta node = ScriptableObject.CreateInstance<NodeSOBeta>();
    //     trackerList.Add(node);
    //     node.Label = name;
    //     node.name = name;

    //     pathway.LocalNetwork.Add(node, new List<EdgeSOBeta>());
    // }

    // public void AddEdgeSO (string name, List<EdgeSOBeta> trackerList, PathwaySOBeta pathway, bool bidirectional, List<NodeSOBeta> reactantNodes, List<NodeSOBeta> productNodes) {

    //     EdgeSOBeta edge = ScriptableObject.CreateInstance<EdgeSOBeta>();
    //     edge.reactants = new List<NodeSOBeta>();
    //     edge.products = new List<NodeSOBeta>();

    //     trackerList.Add(edge);
    //     edge.Label = name;
    //     edge.name = name;
    //     edge.bidirectional = bidirectional;
    
    //     edge.reactants.AddRange(reactantNodes);    
    //     edge.products.AddRange(productNodes);
        
    //     // Link the edge ti the nodes it is connected to through the pathway dictionary  
    //     foreach (NodeSOBeta node in reactantNodes) {
    //         pathway.LocalNetwork[node].Add(edge);
    //     }
    //     foreach (NodeSOBeta node in productNodes) {
    //         pathway.LocalNetwork[node].Add(edge);
    //     }
    // }


    // void FindChildren(PathwaySOBeta pathway,Dictionary<string,bool> visited, Queue<List<ScriptableObject>> queue, NodeSOBeta current)
    // {

    //     List<EdgeSOBeta> interactedEdges = pathway.LocalNetwork[current];
    //     foreach (EdgeSOBeta currentEdge in interactedEdges)
    //     {   
            
    //         Debug.Log("BFS on edge: " + currentEdge.Label);
    //         if (currentEdge.bidirectional)
    //         {
    //             Debug.Log("BFS found bidirectional edge");
    //             //search products, return the one where we dont find node in
    //             if (currentEdge.products.Contains(current))
    //             {
    //                 foreach (NodeSOBeta node in currentEdge.reactants){
    //                     if(!visited.ContainsKey(node.Label)){

    //                         List<ScriptableObject> currentPath = new List<ScriptableObject>(){currentEdge};
    //                         currentPath.Add(node);

    //                         queue.Enqueue(currentPath);
    //                         visited.Add(node.Label,true);
    //                         Debug.Log("BFS added to queue: " + node.Label);
    //                     }
    //                 }
    //             }
    //             else{
    //                 if (currentEdge.reactants.Contains(current))
    //                 {
    //                     foreach (NodeSOBeta node in currentEdge.products){
    //                         if(!visited.ContainsKey(node.Label)){

    //                             List<ScriptableObject> currentPath = new List<ScriptableObject>(){currentEdge};
    //                             currentPath.Add(node);

    //                             queue.Enqueue(node);
    //                             visited.Add(node.Label,true);
    //                             Debug.Log("BFS added to queue: " + node.Label);
    //                         }
    //                     }
    //                 }
    //             }
    //         } 
    //         else
    //         {          // always search reactants
    //             if (currentEdge.reactants.Contains(current))
    //             {
    //                 foreach (NodeSOBeta node in currentEdge.products){
    //                     if(!visited.ContainsKey(node.Label)){
    //                         queue.Enqueue(node);
    //                         visited.Add(node.Label,true);
    //                         Debug.Log("BFS added to queue: " + node.Label);
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

}
