// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MockQuery : MonoBehaviour
// {
//     public List<EdgeSO> edges;
//     public List<NodeSO> nodes;
//     void Start()
//     {  
//         //Glycogen sythanse pathway

//         PathwaySO glycogenSynthasePathway = ScriptableObject.CreateInstance<PathwaySO>();
//         glycogenSynthasePathway.init("glycogen Synthase pathway");  
//         // hexokinase , glucose -> glucose6phospahate 
//         EdgeSO hexokinase = ScriptableObject.CreateInstance<EdgeSO>();
//         hexokinase.init("hexokinase");
//         edges.Add(hexokinase);
        

//         NodeSO glucose = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(glucose);
//         glucose.init("Glucose");

//         NodeSO glucose6phosphate = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(glucose6phosphate);
//         glucose6phosphate.init("glucose-6-phosphate");
//         hexokinase.AddReactant(glucose);
//         hexokinase.AddProduct(glucose6phosphate);

//         glycogenSynthasePathway.AddNodeToPathway(glucose);
//         glycogenSynthasePathway.AddEdgeToPathway(glucose,hexokinase);
//         glycogenSynthasePathway.AddNodeToPathway(glucose6phosphate);
//         glycogenSynthasePathway.AddEdgeToPathway(glucose6phosphate,hexokinase);

//         // Phosphoglucose mutase , glucose 6-phosphate => glucose 1-phosphate
//         EdgeSO phosphoglucoseMutase = ScriptableObject.CreateInstance<EdgeSO>();        
//         edges.Add(phosphoglucoseMutase);
//         phosphoglucoseMutase.init("phosphoglucose mutase");


//         NodeSO glucose1phosphate = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(glucose1phosphate);
//         glucose1phosphate.init("glucose-1-phosphate");
//         phosphoglucoseMutase.AddReactant(glucose6phosphate);
//         phosphoglucoseMutase.AddProduct(glucose1phosphate);

//         glycogenSynthasePathway.AddEdgeToPathway(glucose6phosphate,phosphoglucoseMutase);
//         glycogenSynthasePathway.AddNodeToPathway(glucose1phosphate);
//         glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,phosphoglucoseMutase);

//         // UDP-glucose pyrophosphorylase , UTP + glucose 1-phosphate <=> UDP-glucose + PPi     --> bi directional!
//         EdgeSO UDPGlucosePyrophosphorylase = ScriptableObject.CreateInstance<EdgeSO>();
//         edges.Add(UDPGlucosePyrophosphorylase);
//         UDPGlucosePyrophosphorylase.init("UDP-glucose pyrophosphorylase",true);

//         NodeSO UDPglucose = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(UDPglucose);
//         UDPglucose.init("UDP-glucose");
//         UDPGlucosePyrophosphorylase.AddReactant(glucose1phosphate);
//         UDPGlucosePyrophosphorylase.AddProduct(UDPglucose);

//         glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,UDPGlucosePyrophosphorylase);
//         glycogenSynthasePathway.AddNodeToPathway(UDPglucose);
//         glycogenSynthasePathway.AddEdgeToPathway(UDPglucose,UDPGlucosePyrophosphorylase);

//         // Glycogen synthase, glycogen (n residues) + UDP-glucose => UDP + glycogen (n+1 residues)
//         EdgeSO glycogenSynthase = ScriptableObject.CreateInstance<EdgeSO>();
//         edges.Add(glycogenSynthase);
//         glycogenSynthase.init("glycogenSynthase");

//         NodeSO glycogen_n1 = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(glycogen_n1);
//         glycogen_n1.init("glycogen(n+1)");

//         NodeSO glycogen_n = ScriptableObject.CreateInstance<NodeSO>();
//         nodes.Add(glycogen_n);
//         glycogen_n.init("glycogen(n)");

//         glycogenSynthase.AddReactant(glycogen_n);
//         glycogenSynthase.AddReactant(UDPglucose);
//         glycogenSynthase.AddProduct(glycogen_n1);


//         glycogenSynthasePathway.AddNodeToPathway(glycogen_n1);
//         glycogenSynthasePathway.AddNodeToPathway(glycogen_n);
//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n,glycogenSynthase);
//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n1,glycogenSynthase);
//         glycogenSynthasePathway.AddEdgeToPathway(UDPglucose,glycogenSynthase);

//         //Glycogen phosphorylase, glycogen (n+1 residues) + Pi => glycogen (n residues) + glucose 1-phosphate  --> not in wikibase!, needs to be checked with a theory
//         EdgeSO glycogenPhosphorylase = ScriptableObject.CreateInstance<EdgeSO>();
//         edges.Add(glycogenPhosphorylase);
//         glycogenPhosphorylase.init("glycogen Phosphorylase");

//         glycogenPhosphorylase.AddReactant(glycogen_n1);
//         glycogenPhosphorylase.AddProduct(glycogen_n);
//         glycogenPhosphorylase.AddProduct(glucose1phosphate);

//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n,glycogenPhosphorylase);
//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n1,glycogenPhosphorylase);
//         glycogenSynthasePathway.AddEdgeToPathway(glucose1phosphate,glycogenPhosphorylase);

//         // Glycogen Debranching enzyme, Glycogen (n+1) -> Glucose + glycogen (n)
//         EdgeSO glycogenDebranching = ScriptableObject.CreateInstance<EdgeSO>();
//         glycogenDebranching.init("Glycogen Debranching");
//         glycogenDebranching.AddReactant(glycogen_n1);
//         glycogenDebranching.AddProduct(glycogen_n);
//         glycogenDebranching.AddProduct(glucose);

//         glycogenSynthasePathway.AddEdgeToPathway(glucose,glycogenDebranching);
//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n,glycogenDebranching);
//         glycogenSynthasePathway.AddEdgeToPathway(glycogen_n1,glycogenDebranching);

//         BFSTest(glycogenSynthasePathway,glycogen_n1,glycogen_n);
//     }

//     Dictionary<int,List<ScriptableObject>> SearchForPath(PathwaySO pathway, NodeSO nodeRoot, NodeSO nodeToFind) {

//         int pathNum = 1;
//         Dictionary<int,List<ScriptableObject>> CorrectPaths = new Dictionary<int,List<ScriptableObject>>();

//         Queue<List<ScriptableObject>> BFSQueue = new Queue<List<ScriptableObject>>();
//         Dictionary<string,bool> visited = new Dictionary<string, bool>();

//         BFSQueue.Enqueue(new List<ScriptableObject>(){nodeRoot});
//         visited.Add(nodeRoot.Label,true);

//         while (BFSQueue.Count > 0) {
//             List<ScriptableObject> currentPath = BFSQueue.Dequeue();
//             NodeSO currentNode = (NodeSO) currentPath[currentPath.Count - 1];
//             if (currentNode.Label == nodeToFind.Label){
                
//                 CorrectPaths.Add(pathNum,currentPath);
//                 pathNum++;
//                 //Debug.Log("found the node : " + currentNode.Label + " = " + nodeToFind.Label);
//                 //return currentPath;
//             }

//             Dictionary<EdgeSO,List<NodeSO>> children = FindChildren(pathway,visited,currentNode);
                
//                 foreach(KeyValuePair <EdgeSO, List<NodeSO>> entry in children) {
                    

//                     foreach (NodeSO curr in entry.Value){
//                         List<ScriptableObject> newPath = new List<ScriptableObject>(currentPath);
//                         newPath.Add(entry.Key);
//                         newPath.Add(curr);

//                         if (visited.ContainsKey(curr.Label)){
//                         continue;
//                         }
//                         if (curr.Label != nodeToFind.Label){
//                             visited.Add(curr.Label,true);
//                         }
//                         BFSQueue.Enqueue(newPath);
//                     }
                    
//                 }

//        }
       
//        if(CorrectPaths.Count == 0){
//            Debug.Log("no paths found !");
//        }
//        return CorrectPaths;
//     }

//     /*
//         go through all edges
//         make a new keyvalue pair per edge
//         add all the nodes eligible connected to this edge as value
//         return the dictionary
//     */
//     Dictionary<EdgeSO,List<NodeSO>> FindChildren(PathwaySO pathway,Dictionary<string,bool> visited, NodeSO current)
//     {
        
//         Dictionary<EdgeSO,List<NodeSO>> nodesByEdge = new Dictionary<EdgeSO,List<NodeSO>>();
//         List<EdgeSO> interactedEdges = pathway.LocalNetwork[current];
//         foreach (EdgeSO currentEdge in interactedEdges){

//             nodesByEdge.Add(currentEdge, new List<NodeSO>());
//             if (currentEdge.bidirectional){
//                 Debug.Log("BFS found bidirectional edge ");
//                 //search products and reactants, return the one where we dont find node in

//                 if (currentEdge.products.Contains(current)){

//                     foreach (NodeSO node in currentEdge.reactants){
//                         nodesByEdge[currentEdge].Add(node);

//                     }
//                 } else {
//                     if (currentEdge.reactants.Contains(current)){

//                         foreach (NodeSO node in currentEdge.products){
//                                 nodesByEdge[currentEdge].Add(node);
//                         }
//                     }
//                 }
//             } else {
//                 if (currentEdge.reactants.Contains(current)){

//                     foreach (NodeSO node in currentEdge.products){
//                             nodesByEdge[currentEdge].Add(node);
//                     }
//                 }
//             }
//         }
//         return nodesByEdge;
//     }




//     public void BFSTest(PathwaySO pathway, NodeSO start, NodeSO end) {
//         Dictionary<int,List<ScriptableObject>> result =  SearchForPath(pathway,start,end);

//         string printResult = "<BFS> search in " + pathway.name + "from node:" + start.Label +  " - end node:" + end.Label;

//         foreach(KeyValuePair<int,List<ScriptableObject>> path in result){
//         int n = 1;
//         Debug.Log("<BFS> count =" + result.Count);
//         printResult += "\npath number : " + path.Key;
//             foreach (ScriptableObject step in path.Value){
//                 printResult += "\n" + n + " - " + step.name; //GetType().ToString();
//                 n++;   
//             }
//         }
//         Debug.Log(printResult);
//     }

// }
