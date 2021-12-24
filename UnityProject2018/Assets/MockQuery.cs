using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockQuery : MonoBehaviour
{
    public List<EdgeSOBeta> edges;
    public List<NodeSOBeta> node;
    // Start is called before the first frame update

    /* TODO:
        - we check for visited in both search and find childern and I think thats an issue
        - write assertion statements for the BFS
    */
    void Start()
    {  
        //Glycogen sythanse pathway
        PathwaySOBeta glycogenSynthasePathway = ScriptableObject.CreateInstance<PathwaySOBeta>();
        glycogenSynthasePathway.LocalNetwork = new Dictionary<NodeSOBeta, List<EdgeSOBeta>>();
        // hexokinase , glucose -> glucose6phospahate 
        EdgeSOBeta hexokinase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        hexokinase.Label = "hexokinase";

        NodeSOBeta glucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose.Label = "glucose";
        NodeSOBeta glucose6phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose6phosphate.Label = "glucose-6-phosphate";

        hexokinase.reactants = new List<NodeSOBeta>();
        hexokinase.reactants.Add(glucose);
        hexokinase.products = new List<NodeSOBeta>();
        hexokinase.products.Add(glucose6phosphate);

        glycogenSynthasePathway.LocalNetwork.Add(glucose, new List<EdgeSOBeta>{hexokinase});
        glycogenSynthasePathway.LocalNetwork.Add(glucose6phosphate, new List<EdgeSOBeta>{hexokinase});

        // Phosphoglucose mutase , glucose 6-phosphate => glucose 1-phosphate
        EdgeSOBeta phosphoglucoseMutase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        phosphoglucoseMutase.Label = "phosphoglucose mutase";

        NodeSOBeta glucose1phosphate = ScriptableObject.CreateInstance<NodeSOBeta>();
        glucose1phosphate.Label = "glucose-1-phosphate";

        phosphoglucoseMutase.reactants = new List<NodeSOBeta>();
        phosphoglucoseMutase.reactants.Add(glucose6phosphate);
        phosphoglucoseMutase.products = new List<NodeSOBeta>();
        phosphoglucoseMutase.products.Add(glucose1phosphate);

        glycogenSynthasePathway.LocalNetwork[glucose6phosphate].Add(phosphoglucoseMutase);
        glycogenSynthasePathway.LocalNetwork.Add(glucose1phosphate, new List<EdgeSOBeta>{phosphoglucoseMutase});

        // UDP-glucose pyrophosphorylase , UTP + glucose 1-phosphate <=> UDP-glucose + PPi     --> bi directional!
        EdgeSOBeta UDPGlucosePyrophosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        UDPGlucosePyrophosphorylase.Label = "UDP-glucose pyrophosphorylase";

        NodeSOBeta UDPglucose = ScriptableObject.CreateInstance<NodeSOBeta>();
        UDPglucose.Label = "UDP-glucose";

        UDPGlucosePyrophosphorylase.reactants = new List<NodeSOBeta>();
        UDPGlucosePyrophosphorylase.reactants.Add(glucose1phosphate);
        UDPGlucosePyrophosphorylase.products = new List<NodeSOBeta>();
        UDPGlucosePyrophosphorylase.products.Add(UDPglucose);
        UDPGlucosePyrophosphorylase.bidirectional = true;

        glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(UDPGlucosePyrophosphorylase);
        glycogenSynthasePathway.LocalNetwork.Add(UDPglucose, new List<EdgeSOBeta>{UDPGlucosePyrophosphorylase});


        // glycogen synthase, glycogen (n residues) + UDP-glucose => UDP + glycogen (n+1 residues)
        EdgeSOBeta glycogenSynthase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        glycogenSynthase.Label = "glycogenSynthase";

        NodeSOBeta glycogen_n1 = ScriptableObject.CreateInstance<NodeSOBeta>();
        glycogen_n1.Label = "glycogen(n+1)";
        NodeSOBeta glycogen_n = ScriptableObject.CreateInstance<NodeSOBeta>();
        glycogen_n.Label = "glycogen(n)";


        glycogenSynthase.reactants = new List<NodeSOBeta>();
        glycogenSynthase.reactants.Add(glycogen_n);
        glycogenSynthase.reactants.Add(UDPglucose);
        glycogenSynthase.products = new List<NodeSOBeta>();
        glycogenSynthase.products.Add(glycogen_n1);

        glycogenSynthasePathway.LocalNetwork.Add(glycogen_n1, new List<EdgeSOBeta>{glycogenSynthase});
        glycogenSynthasePathway.LocalNetwork.Add(glycogen_n, new List<EdgeSOBeta>{glycogenSynthase});
        glycogenSynthasePathway.LocalNetwork[UDPglucose].Add(glycogenSynthase);
        // glycogen phosphorylase, glycogen (n+1 residues) + Pi => glycogen (n residues) + glucose 1-phosphate  --> not in wikibase!, needs to be checked with a theory
        EdgeSOBeta glycogenPhosphorylase = ScriptableObject.CreateInstance<EdgeSOBeta>();
        glycogenPhosphorylase.Label = "glycogen Phosphorylase";

        glycogenPhosphorylase.reactants = new List<NodeSOBeta>();
        glycogenPhosphorylase.reactants.Add(glycogen_n1);
        glycogenPhosphorylase.products = new List<NodeSOBeta>();
        glycogenPhosphorylase.products.Add(glycogen_n);
        glycogenPhosphorylase.products.Add(glucose1phosphate);

        glycogenSynthasePathway.LocalNetwork[glycogen_n1].Add(glycogenPhosphorylase);
        glycogenSynthasePathway.LocalNetwork[glucose1phosphate].Add(glycogenPhosphorylase);        
        glycogenSynthasePathway.LocalNetwork[glycogen_n].Add(glycogenPhosphorylase);
    }

      list<ScriptableObject> SearchForPath(PathwaySOBeta pathway, NodeSOBeta nodeRoot, NodeSOBeta nodeToFind) {

       Queue<List<ScriptableObject>> BFSQueue = new Queue<List<ScriptableObject>>();
       Dictionary<string,bool> visited = new Dictionary<string, bool>();

       BFSQueue.Enqueue(new list<ScriptableObject>(){nodeRoot});
       visited.Add(nodeRoot.Label,true);

       while (BFSQueue.Count > 0) {
           List<ScriptableObject> currentPath = BFSQueue.Dequeue();
           NodeSOBeta currentNode = currentPath[currentPath.Count - 1];
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
            // new list of SO =of current path, enqueu this instead

            List<ScriptableObject> newPath = new List<ScriptableObject>(currentPath);
            foreach(KeyValuePair <EdgeSOBeta, List<NodeSOBeta>> entry in children) {
                newPath.Add(entry.Key);
                newPath.AddRange(entry.Value);

                if (visited.ContainsKey(newPath[newPath.Count - 1])){
                    continue;
                }
                BFSQueue.Enqueue(newPath);
            }
            
           
       }
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
                        if(!visited.ContainsKey(node.Label)){
                            nodesByEdge[currentEdge].Add(node);
                            visited.Add(node.Label,true);
                        }
                    }
                } else {
                    if (currentEdge.reactants.Contains(current)){

                        foreach (NodeSOBeta node in currentEdge.products){
                            if(!visited.ContainsKey(node.Label)){
                                nodesByEdge[currentEdge].Add(node);
                                visited.Add(node.Label,true);
                            }
                        }
                    }
                }
            } else {
                if (currentEdge.reactants.Contains(current)){

                    foreach (NodeSOBeta node in currentEdge.products){
                        if(!visited.ContainsKey(node.Label)){
                            nodesByEdge[currentEdge].Add(node);
                            visited.Add(node.Label,true);
                        }
                    }
                }
            }
        }
        return nodesByEdge;
    }






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
